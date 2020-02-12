<template>
    <div>
        <div class="modal-backdrop" v-show="isNewVoyageVisable">
            <div class="modal">
                <section class="modal-body">
                    <slot name="body">
                        <SetCourse v-model="voyage"></SetCourse>
                    </slot>

                </section>
                <footer class="modal-footer">
                    <slot name="footer">
                        <button type="button" @click="onAlterVoyageOk">Ok</button>
                        <button type="button" @click="onAlterVoyageCancel">Cancel</button>
                    </slot>
                </footer>
            </div>
        </div>
        <div>
            <h3>Sailing on the Voyage - {{voyageName}}</h3>
            <button v-on:click="sail">Sail!</button>
            <button v-on:click="alterVoyage">Change Course</button>
        </div>

        <div class="grid-container">
            <div class="left-grid">
                <div class="card">
                    <div class="container">
                        <table align="center">
                            <tr>
                                <td style="text-align:right">
                                    <label class="stat-label">Current Date: </label>
                                </td>
                                <td>
                                    <label type="text" style="float:left">{{sailResult.state.currentDate}}</label>
                                </td>
                            </tr>
                            <tr>

                                <td style="text-align:right">
                                    <label class="stat-label">Travel Progress: </label>
                                </td>
                                <td>
                                    <label type="text" style="float:left">{{sailResult.state.progressMade}}/{{voyage.daysPlanned}}</label>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align:right">
                                    <label class="stat-label">Night Status: </label>
                                </td>
                                <td>
                                    <select v-model="voyage.nightStatus" style="float:left">
                                        <option value="Anchored" selected>Anchored</option>
                                        <option value="Drifting">Drifting</option>
                                        <option value="Underweigh">Underweigh</option>
                                    </select>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align:right">
                                    <label class="stat-label">Open Ocean: </label>
                                </td>
                                <td>
                                    <div><input type="checkbox" style="float:left" v-model="voyage.openOcean" /></div>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align:right">
                                    <label class="stat-label">Shallow Water: </label>
                                </td>
                                <td>
                                    <div><input type="checkbox" style="float:left" v-model="voyage.shallowWater" /></div>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align:right">
                                    <label class="stat-label">Narrow Passage: </label>
                                </td>
                                <td>
                                    <div><input type="checkbox" style="float:left" v-model="voyage.narrowPassage" /></div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
            <div class="right-grid">
                <div class="card" v-for="ship in shipModifiers" v-bind:key="ship.name">
                    <div class="container">
                        <h4>{{ship.name}}</h4>
                        <table align="center">
                            <tr>
                                <td>
                                    <label class="stat-label" style="float: right">Command Modifier: </label>
                                </td>
                                <td>
                                    <input type="number" v-model="ship.commandModifier" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label class="stat-label" style="float: right">Discipline Modifier: </label>
                                </td>
                                <td>
                                    <input type="number" v-model="ship.disciplineModifier" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label class="stat-label" style="float: right">Morale Modifier: </label>
                                </td>
                                <td>
                                    <input type="number" v-model="ship.moraleModifier" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label class="stat-label" style="float: right">Swabbies: </label>
                                </td>
                                <td>
                                    <input type="number" v-model="ship.swabbies" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label class="stat-label" style="float: right">Unfit Crew: </label>
                                </td>
                                <td>
                                    <input type="number" v-model="ship.numberOfCrewUnfitForDuty" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label class="stat-label" style="float: right">Diseased Crew: </label>
                                </td>
                                <td>
                                    <input type="number" v-model="ship.diseasedCrew" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label class="stat-label" style="float: right">Discipline Standards: </label>
                                </td>
                                <td>
                                    <select v-model="ship.disciplineStandards" style="float:left">
                                        <option value="Normal">Normal</option>
                                        <option value="Lax">Lax</option>
                                        <option value="Strict">Strict</option>
                                    </select>

                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>

        <div>
            <WeatherResult v-model="sailResult.state.weatherConditions"></WeatherResult>
        </div>

        <div v-for="sr in sailResult.results" v-bind:key="sr.loadout">
            <h3>{{sr.loadout}}</h3>
            <p style="text-align:left" v-for="msg in sr.message" v-bind:key="msg">{{msg}}</p>
        </div>
    </div>
</template>

<script>
    import WeatherResult from './Weather/WeatherResult.vue'
    import SetCourse from './SetCourse.vue'
    export default {
        name: "Sailing",
        components: {
            WeatherResult,
            SetCourse
        },
        data: function () {
            return {
                sailResult: {
                    results: {},
                    state: {}
                },
                voyage: {},
                shipModifiers: {},
                isNewVoyageVisable: false
            }
        },
        computed: {
            voyageName() {
                return this.$route.params.id;
            },
            voyageParameters() {
                var sm = this.shipModifiers.map(x => {
                    return {
                        loadoutName: x.name,
                        disciplineModifier: x.disciplineModifier,
                        commandModifier: x.commandModifier,
                        numberOfCrewUnfitForDuty: x.numberOfCrewUnfitForDuty,
                        numberOfCrewDiseased: x.diseasedCrew,
                        swabbies: x.swabbies,
                        disciplineStandards: x.disciplineStandards,
                        moraleModifier: x.moraleModifier
                    };
                });

                return {
                    voyageName: this.voyageName,
                    narrowPassage: this.voyage.narrowPassage,
                    shallowWater: this.voyage.shallowWater,
                    openOcean: this.voyage.openOcean,
                    nightStatus: this.voyage.nightStatus,
                    shipModifiers: sm
                };
            }
        },
        mounted: function () {
            this.loadState();
        },
        methods: {
            loadState() {
                var self = this;
                fetch('/SailingEngine/State?name=' + this.voyageName).then(r => r.json()).then(d => {
                    self.voyage = JSON.parse(JSON.stringify(d.voyage));
                    self.sailResult.state = d.state;
                    self.updateShipModifiers(d.voyage);
                    self.updateShipModifiersFromProgress(self.sailResult.state.shipsProgress);
                });
            },
            updateShipModifiers(voyage) {
                this.shipModifiers = Array.from(voyage.shipLoadouts).map(x => {
                    return {
                        name: x,
                        swabbies: voyage.swabbies.find(x => x.loadoutName = x).swabbies,
                        moralModifer: 0,
                        disciplineModifier: 0,
                        commandModifier: 0,
                        numberOfCrewUnfitForDuty: 0,
                        diseasedCrew: 0,
                        disciplineStandards: voyage.disciplineStandards,
                        moraleModifier: 0
                    };
                });
            },
            sail() {
                var self = this;
                fetch('/SailingEngine/Sail', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(this.voyageParameters)
                }).then(r => r.json()).then(d => {
                    self.sailResult = d
                    self.updateShipModifiersFromProgress(self.sailResult.state.shipsProgress);
                });
            },
            updateShipModifiersFromProgress(progress) {
                progress.forEach(p => {
                    var sm = this.shipModifiers.filter(mod => mod.name == p.shipName)[0];
                    sm.diseasedCrew = p.diseasedCrew;
                });
            },
            alterVoyage() {
                this.isNewVoyageVisable = true;
            },
            onAlterVoyageOk() {
                var self = this;
                fetch('/Voyage/Update', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(this.voyage)
                }).then(() => {
                    fetch('/SailingEngine/CourseChange?name=' + self.voyageName, {
                        method: 'POST',
                        headers: { 'Content-Type': 'application/json' },
                        body: JSON.stringify({ startDate: self.voyage.startDate, daysPlanned: self.voyage.daysPlanned, shipLoadouts: self.voyage.shipLoadouts })
                    }).then(() => self.loadState());
                });
                this.updateShipModifiers(JSON.parse(JSON.stringify(this.voyage)));
                this.isNewVoyageVisable = false;
            },
            onAlterVoyageCancel() {
                this.isNewVoyageVisable = false;
            }
        }
    }
</script>