<template>
    <div class="card">
        <div class="container">
            <div class="form-grid-container">

                <div>
                    <h3>{{ship.name}} - {{ship.shipName}}</h3>
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
                                        <option value="Command">Command</option>
                                        <option value="Manage">Manage</option>
                                        <option value="Pilot">Pilot</option>
                                        <option value="Watch">Watch</option>
                                        <option value="Navigate">Navigate</option>
                                        <option value="Maintain">Maintain</option>
                                        <option value="Discipline">Discipline</option>
                                        <option value="Cook">Cook</option>
                                        <option value="Heal">Heal</option>
                                        <option value="Stow">Stow</option>
                                        <option value="Unload">Unload</option>
                                        <option value="RepairHull">RepairHull</option>
                                        <option value="RepairSails">RepairSails</option>
                                        <option value="RepairSeigeEngine">RepairSeigeEngine</option>
                                        <option value="Procure">Procure</option>
                                        <option value="Ministrel">Ministrel</option>
                                        <option value="Drill">Drill</option>
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
                    <CrewMemberLoadout v-for="cm in ship.crewMembers"
                                       v-bind:key="cm.name"
                                       v-bind:crewMember="cm"
                                       @delete_crew="removeCrewMember"></CrewMemberLoadout>
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
                names: []
            };
        },
        mounted: function () {
            var self = this;
            fetch('/CrewMemberStats/Names').then(r => r.json()).then(d => self.names = d);

        },
        components: {
            CrewMemberLoadout
        },
        props: {
            ship: {}

        },
        methods: {
            closeNewCrewMember() {
                    this.newCrewMember = { name: "", dutyType: "", isAssistant: false };
            },
            updateShip() {
                fetch('/ShipLoadout/Update', {

                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(this.ship)
                });
            },
            saveNewCrewMember() {

                if (this.names.includes(this.newCrewMember.name)) {
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
                    .filter(cm => cm.name != crewMember.name);// && cm.dutyType != crewMember.dutyType && cm.isAssistant != crewMember.isAssistant);
                this.updateShip();
            }
        }
    }
</script>

