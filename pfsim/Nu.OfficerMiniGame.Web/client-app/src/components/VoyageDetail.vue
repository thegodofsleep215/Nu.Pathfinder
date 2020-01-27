<template>
    <div class="card">
        <div class="container">
            <div class="form-grid-container">
                <div>
                    <h3 style="display: inline">{{value.name}}</h3>
                    <h3 style="display: inline" v-if="!canEdit"> - Voyage is Underweigh</h3>
                </div>

                <div>
                    <button style="align-self:center" class="update-button" v-on:click="updateVoyage(value)" v-if="canEdit"> </button>
                    <button style="align-self:center" class="delete-button" v-on:click="deleteVoyage()"></button>
                </div>

                <div>
                    <table align="center">
                        <tr>
                            <td>
                                <label class="stat-label" style="float: right">Year:</label>
                            </td>
                            <td>
                                <input type="text" style="float:left" v-model="date.year" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="stat-label" style="float: right">Month</label>
                            </td>
                            <td>
                                <input type="text" style="float:left" v-model="date.month" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="stat-label" style="float: right">Day</label>
                            </td>
                            <td>
                                <input type="text" style="float:left" v-model="date.day" />
                            </td>
                        </tr>

                        <tr>
                            <td style="text-align:right">
                                <label class="stat-label">Port: </label>
                            </td>
                            <td>
                                <div><input type="text" style="float:left" v-model="value.port" /></div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align:right">
                                <label class="stat-label">Destination Port: </label>
                            </td>
                            <td>
                                <div><input type="text" style="float:left" v-model="value.destinationPort" /></div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align:right">
                                <label class="stat-label">Days Planned: </label>
                            </td>
                            <td>
                                <div><input type="text" style="float:left" v-model="value.daysPlanned" /></div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align:right">
                                <label class="stat-label">Night Status: </label>
                            </td>
                            <td>
                                <select v-model="value.nightStatus" style="float:left">
                                    <option value="Anchored" selected>Anchored</option>
                                    <option value="Drifting">Drifting</option>
                                    <option value="Underweigh">Underweigh</option>
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align:right">
                                <label class="stat-label">Discipline Standards: </label>
                            </td>
                            <td>
                                <select v-model="value.disciplineStandards" style="float:left">
                                    <option value="Normal" selected>Normal</option>
                                    <option value="Lax">Lax</option>
                                    <option value="Strict">Strict</option>
                                </select>
                            </td>
                        </tr>

                        <tr>
                            <td style="text-align:right">
                                <label class="stat-label">Open Ocean: </label>
                            </td>
                            <td>
                                <div><input type="checkbox" style="float:left" v-model="value.openOcean" /></div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align:right">
                                <label class="stat-label">Shallow Water: </label>
                            </td>
                            <td>
                                <div><input type="checkbox" style="float:left" v-model="value.shallowWater" /></div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align:right">
                                <label class="stat-label">Narrow Passage: </label>
                            </td>
                            <td>
                                <div><input type="checkbox" style="float:left" v-model="value.narrowPassage" /></div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="stat-label">Ship Loadouts</span>
                            </td>
                            <td>
                                <select id="msShipLoadouts" multiple v-model="value.shipLoadouts" v-on:change="shipLoadoutsChanged">
                                    <option v-for="l in loadouts" :value="l" v-bind:key="l">{{l}}</option>
                                </select>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="grid-column-start:1">
                    <div v-for="ship in value.swabbies" v-bind:key="ship.loadoutName" class="card">
                        <div class="container">
                            <h2>{{ship.loadoutName}}</h2>
                            <table>
                                <tr>
                                    <td>
                                        <label class="stat-label">Swabbies: </label>
                                    </td>
                                    <td>
                                        <input type="number" v-model="ship.swabbies" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <div style="grid-column-start: 1">
                    <button v-on:click="hoistAnchor" v-if="canEdit">Hoist Anchor!</button>
                    <button v-on:click="gotoSailing" v-else>Continue Sailing.</button>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        name: "VoyageDetail",
        props: {
            value: {}
        },
        data: function () {
            return {
                loadouts: [],
                date: {}
            }
        },
        computed: {
            canEdit() {
                return !this.value.underweigh;
            },
            serializedDate() {
                return this.date.year + "/" + this.date.month + "/" + this.date.day + " 0:0:0";
            }
        },
        mounted: function () {
            var self = this;
            fetch('/ShipLoadout/Names').then(r => r.json()).then(d => {
                self.loadouts = d;
            });
            this.shipLoadoutsChanged();
        },
        updated: function () {
            if (this.value.shipLoadouts == null) {
                this.value.shipLoadouts = [];
            }
            // Was having trouble with vue clearing out the multi select when you jumped from one voyage to another.
            Array.from(document.getElementById("msShipLoadouts").options).forEach(x => x.selected = this.value.shipLoadouts.includes(x.text));
        },
        methods: {
            shipLoadoutsChanged() {
                var date = /(\d+)\/(\d+)\/(\d+)/;
                var match = this.value.startDate.match(date);
                this.date.year = match[1];
                this.date.month = match[2];
                this.date.day = match[3];


                if (this.value.swabbies == null) {
                    this.value.swabbies = [];
                }
                // add new ones
                this.value.shipLoadouts.filter(x => !this.value.swabbies.map(x => x.loadoutName).includes(x))
                    .forEach(x => this.value.swabbies.push({ loadoutName: x, swabbies: 0 }));

                // remove old ones
                this.value.swabbies = this.value.swabbies.filter(x => this.value.shipLoadouts.includes(x.loadoutName));
            },
            updateVoyage(v) {
                v.startDate = this.serializedDate;
                fetch('/Voyage/Update', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(v)
                });
            },
            deleteVoyage() {
                if (confirm("Do the thing?")) {
                    this.$emit("delete-voyage", this.value.name);
                    self.value = undefined;
                }
            },
            gotoSailing() {
                this.$router.push({ path: "Sailing/" + this.value.name, params: { id: this.value.name } });
            },
            hoistAnchor() {
                if (this.value.shipLoadouts === undefined || this.value.shipLoadouts.length == 0) {
                    alert("You must select at least one loadout.")
                }
                else if (this.value.daysPlanned <= 0) {
                    alert("Days Planned must be greater than 0.")
                }
                else {
                    if (confirm("Hoisting Anchor means the voyage can no logger be edited.")) {
                        this.value.underweigh = true;
                        this.updateVoyage(this.value);
                        this.gotoSailing();
                    }
                }
            }
        }
    }
</script>