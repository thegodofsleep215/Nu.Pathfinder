<template>
    <div class="card">
        <div class="container">
            <div class="form-grid-container">
                <div>
                    <h3>{{shipName}}</h3>
                </div>
                <div>
                    <button style="align-self:center" class="update-button" v-on:click="saveShipStats"> </button>
                    <button style="align-self:center" class="delete-button" v-on:click="deleteShipStats"></button>
                </div>
                <div style="grid-column-start: 1">
                    <table>
                        <tr>
                            <td>
                                <label class="stat-label" style="float:right">Ship Type:</label>
                            </td>
                            <td>
                                <select id="selectShipTypes" v-model="stats.shipType" style="float:left" v-on:change="templateChanged">
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="stat-label" style="float:right">Ship Size:</label>
                            </td>
                            <td>
                                <label style="float:left">{{template.shipSize}}</label>
                            </td>
                        </tr>
                        <tr>
                            <td>

                                <label class="stat-label" style="float:right">Hull Hit Points:</label>
                            </td>
                            <td>
                                <label style="float:left">{{template.hullHitPoints}}</label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="stat-label" style="float:right">Officer Size:</label>
                            </td>
                            <td>
                                <label style="float:left">{{template.officerSize}}</label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="stat-label" style="float:right">Crew Size:</label>
                            </td>
                            <td>
                                <label style="float:left">{{template.crewSize}}</label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="stat-label" style="float:right">Cargo Points:</label>
                            </td>
                            <td>
                                <label style="float:left">{{template.cargoPoints}}</label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="stat-label" style="float:right">Passengers:</label>
                            </td>
                            <td>
                                <label style="float:left">{{template.passengers}}</label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="stat-label" style="float:right">Ship DC:</label>
                            </td>
                            <td>
                                <label style="float:left">{{template.shipDc}}</label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="stat-label" style="float:right">Ship Piloting Bonus:</label>
                            </td>
                            <td>
                                <input type="number" v-model="stats.shipPilotingBonus" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="stat-label" style="float:right">Ship Quality:</label>
                            </td>
                            <td>
                                <input type="number" v-model="stats.shipQuality" />
                            </td>
                        </tr>
                    </table>
                    <div class="card">
                        <div class="container">
                            <div><h4>Propulsion Types</h4></div>
                            <div v-for="prop in updatedShip.propulsionTypes" v-bind:key="prop.propulsionType" class="card">
                                <div class="container">
                                    <p class="sub-stat"><span class="stat-label">Type:</span> {{prop.propulsionType}}</p>
                                    <p class="sub-stat"><span class="stat-label">Ship Speed:</span> {{prop.shipSpeed}}</p>
                                    <p class="sub-stat"><span class="stat-label">Hit Points:</span> {{prop.propulsionHitPoints}}</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    import * as shipTemplates from '../scripts/ShipTypes.js'
    import * as axios from '../scripts/axios.min.js'
    export default {
        name: 'ShipStatDetails',
        props: {
            shipName: String
        },
        data: function () {
            return {
                templates: shipTemplates.getShipTypes(),
                stats: {},
                template: {},
            }
        },
        computed: {
            updatedShip() {
                var newShip = JSON.parse(JSON.stringify(this.template));
                newShip.name = this.stats.name;
                newShip.shipPilotingBonus = this.stats.shipPilotingBonus;
                newShip.shipQuality = this.stats.shipQuality;
                return newShip;
            }
        },
        mounted: function () {
            var s = document.getElementById("selectShipTypes");
            this.templates.forEach(x => {
                var option = document.createElement("option");
                option.value = x.shipType;
                option.text = x.shipType;
                s.append(option);
            });
        },
        watch: {
            shipName: function () {
                this.loadShip(this.shipName);
            },
        },
        methods: {
            loadShip(name) {
                var self = this;
                if (this.shipName.length > 0) {
                    axios.get('/ShipStats?name=' + name).then(r => {
                        self.stats = r.data;
                        this.templateChanged();
                    });
                }

            },
            templateChanged() {
                var t = JSON.stringify(this.templates.filter(x => x.shipType == this.stats.shipType)[0]);
                this.template = JSON.parse(t);
            },
            saveShipStats() {
                fetch('/ShipStats/Update', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(this.updatedShip)
                });
            },
            deleteShipStats() {
                if (confirm("Do the thing?")) {
                    this.$emit("delete-shipstats", this.shipName);
                    self.stats = undefined;
                    self.template = undefined;
                }
            }

        }
    }
</script>

<style>
    .content-grid-container {
        display: grid;
        grid-template-areas: 'header header' 'left right';
        grid-column-gap: 15px;
        grid-row-gap: 5px;
        align-content: center;
        justify-content: center;
    }
</style>