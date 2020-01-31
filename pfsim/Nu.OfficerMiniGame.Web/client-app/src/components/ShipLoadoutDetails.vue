<template>
    <div class="card">
        <div class="container">
            <div class="form-grid-container">

                <div>
                    <h3 style="display:inline">{{value}} - </h3>
                    <select id="selectShips" style="display:inline" v-model="ship.shipName" @change="updateShip">
                        <option v-for="s in ships" :value="s" v-bind:key="s">{{s}}</option>
                    </select>
                </div>

                <div class="card" style="grid-column-start: 1">
                    <div class="container">
                        <table align="center">
                            <tr>
                                <td>
                                    <label class="stat">Name: </label>
                                    <select v-model="newCrewMember.name">
                                        <option v-for="name in names" :value="name" v-bind:key="name">{{name}}</option>
                                    </select>
                                </td>
                                <td>
                                    <label class="stat">Duty: </label>
                                    <select v-model="newCrewMember.dutyType">
                                        <option v-for="d in duties" v-bind:key="d" :value="d">{{d}}</option>
                                    </select>
                                </td>
                                <td>
                                    <label class="stat">Is Assistant: </label>
                                    <input type="checkbox" v-model="newCrewMember.isAssistant" />
                                </td>
                                <td>
                                    <button class="small-add-button" v-on:click="saveNewCrewMember"></button>
                                </td>
                                <td>
                                    <button class="small-delete-button" v-on:click="closeNewCrewMember"></button>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div style="grid-column-start: 1">
                    <div v-for="d in duties" v-bind:key="d" class="card">
                        <div style="padding-top: 10px; background-color: #EEE">{{d}}</div>
                        <CrewMemberLoadout v-for="cm in crewForDuty(d)"
                                           v-bind:key="cm.key"
                                           v-bind:crewMember="cm"
                                           @delete_crew="removeCrewMember"></CrewMemberLoadout>

                        <CrewMemberLoadout v-for="cm in assitantsForDuty(d)"
                                           v-bind:key="cm.key"
                                           v-bind:crewMember="cm"
                                           @delete_crew="removeCrewMember"></CrewMemberLoadout>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    import CrewMemberLoadout from './CrewMemberLoadout.vue'
    export default {
        name: 'ShipLoadoutDetails',
        data: function () {
            return {
                newCrewMember: { name: "", dutyType: "", isAssistant: false },
                names: [],
                ships: [],
                ship: {},
                duties: ["Command", "Manage", "Pilot", "Watch", "Navigate",
                    "Maintain", "Discipline", "Cook", "Heal", "Ministrel",]
            };
        },
        mounted: function () {
            this.load(this.value)
            var self = this;
            fetch('/CrewMemberStats/Names').then(r => r.json()).then(d => self.names = d);
            fetch('/ShipStats/Names').then(r => r.json()).then(d => self.ships = d);
        },
        watch: {
            value() {
                this.load(this.value);
            }
        },
        components: {
            CrewMemberLoadout
        },
        props: {
            value: { type: String }
        },
        methods: {
            crewForDuty(duty) {
                return this.ship.crewMembers.filter(x => x.dutyType == duty && !x.isAssistant);
            },
            assitantsForDuty(duty) {
                return this.ship.crewMembers.filter(x => x.dutyType == duty && x.isAssistant);
            },
            load(name) {
                var self = this;
                fetch('/ShipLoadout?name=' + name).then(r => r.json()).then(d => self.ship = d);
            },
            closeNewCrewMember() {
                this.newCrewMember = { name: "", dutyType: "", isAssistant: false, key: "" };
            },
            updateShip() {
                fetch('/ShipLoadout/Update', {

                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(this.ship)
                }).then(r => {
                    if (r.status != 200) {
                        r.json().then(d => {
                            var msg = "";
                            d.forEach(x => msg += x + "\r\n");
                            alert(msg);
                            load(this.value);
                        });
                    }
                });
            },
            saveNewCrewMember() {

                if (this.names.includes(this.newCrewMember.name)) {
                    var ncm = this.newCrewMember;
                    ncm.key = ncm.name + " - " + ncm.dutyType;
                    this.ship.crewMembers.push(this.newCrewMember);
                    this.updateShip();
                    this.closeNewCrewMember();
                }
                else {
                    alert("That crew member does not exist.");
                }
            },
            removeCrewMember(crewMember) {
                this.ship.crewMembers = this.ship.crewMembers
                    .filter(cm => cm.key != crewMember.key);
                this.updateShip();
            }
        }
    }
</script>

