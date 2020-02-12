<template>
    <div class="modal-backdrop" v-show="value">
        <div class="modal">
            <section class="modal-body">
                <slot name="body">
                    <div class="card">
                        <div class="container">
                            <div class="form-grid-container">
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
                                                <div><input type="text" style="float:left" v-model="course.port" /></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align:right">
                                                <label class="stat-label">Destination Port: </label>
                                            </td>
                                            <td>
                                                <div><input type="text" style="float:left" v-model="course.destinationPort" /></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align:right">
                                                <label class="stat-label">Days Planned: </label>
                                            </td>
                                            <td>
                                                <div><input type="text" style="float:left" v-model="course.daysPlanned" /></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align:right">
                                                <label class="stat-label">Open Ocean: </label>
                                            </td>
                                            <td>
                                                <div><input type="checkbox" style="float:left" v-model="course.openOcean" /></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align:right">
                                                <label class="stat-label">Shallow Water: </label>
                                            </td>
                                            <td>
                                                <div><input type="checkbox" style="float:left" v-model="course.shallowWater" /></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align:right">
                                                <label class="stat-label">Narrow Passage: </label>
                                            </td>
                                            <td>
                                                <div><input type="checkbox" style="float:left" v-model="course.narrowPassage" /></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span class="stat-label">Ship Loadouts</span>
                                            </td>
                                            <td>
                                                <select id="msShipLoadouts" multiple v-model="course.shipLoadouts" v-on:change="shipLoadoutsChanged">
                                                    <option v-for="l in loadouts" :value="l" v-bind:key="l">{{l}}</option>
                                                </select>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div style="grid-column-start:1">
                                    <div v-for="ship in course.shipStates" v-bind:key="ship.loadoutName" class="card">
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
                                                <tr>
                                                    <td>
                                                        <label class="stat-label">Diseased Crew: </label>
                                                    </td>
                                                    <td>
                                                        <input type="number" v-model="ship.diseasedCrew" />
                                                    </td>

                                                </tr>
                                                <tr>
                                                    <td style="text-align:right">
                                                        <label class="stat-label">Night Status: </label>
                                                    </td>
                                                    <td>
                                                        <select v-model="ship.nightStatus" style="float:left">
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
                                                        <select v-model="ship.disciplineStandards" style="float:left">
                                                            <option value="Normal" selected>Normal</option>
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
                        </div>
                    </div>
                </slot>
            </section>
            <footer class="modal-footer">
                <slot name="footer">
                    <button type="button" @click="oneOk">Ok</button>
                    <button type="button" @click="onCancel">Cancel</button>
                </slot>
            </footer>
        </div>
    </div>

</template>

<script>
    export default {
        name: "SetCourse",
        props: {
            value: {required:false}
        },
        data: function () {
            return {
                course: {},
                loadouts: [],
                date: { year: 2012, month: 1, day: 1 }
            }
        },
        computed: {
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
            this.parseDate();
        },
        watch: {
            value: function () {
                this.shipLoadoutsChanged();
                this.parseDate();
            },
            serializedDate: function () {
                this.course.startDate = this.serializedDate;
            }
        },
        methods: {
            parseDate() {
                if (this.course.startDate != null) {
                    var date = /(\d+)\/(\d+)\/(\d+)/;
                    var match = this.course.startDate.match(date);
                    this.date.year = match[1];
                    this.date.month = match[2];
                    this.date.day = match[3];
                }
                else {
                    this.date = { year: 0, month: 0, day: 0 };
                }

            },
            shipLoadoutsChanged() {
                if (this.course.shipStates == null) {
                    this.course.shipStates = [];
                }
                // add new ones
                this.course.shipLoadouts.filter(x => !this.course.shipStates.map(x => x.loadoutName).includes(x))
                    .forEach(x => this.course.shipStates.push({
                        loadoutName: x,
                        swabbies: 0,
                        diseasedCrew: 0,
                        nightStatus: "Underweigh",
                        disciplineStandards: "Normal",
                    }));

                // remove old ones
                this.course.shipStates = this.course.shipStates.filter(x => this.course.shipLoadouts.includes(x.loadoutName));
            },
            onOk() {
                this.$emit("setcourse-ok", this.course);
            },
            onCancel() {
                this.$emit("setcourse-cancel");
            }
        }
    }
</script>