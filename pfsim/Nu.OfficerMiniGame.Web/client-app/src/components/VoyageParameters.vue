<template>
    <div class="grid-container">
        <div class="left-grid">
            <div class="card">
                <div class="container">
                    <h4>{{voyageParams.loadoutName}}</h4>
                    <table align="center">
                        <tr>
                            <td style="text-align:right">
                                <label class="stat-label">Night Status: </label>
                            </td>
                            <td>
                                <select v-model="voyageParams.nightStatus" style="float:left">
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
                                <div><input type="checkbox" style="float:left" v-model="voyageParams.openOcean" /></div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align:right">
                                <label class="stat-label">Shallow Water: </label>
                            </td>
                            <td>
                                <div><input type="checkbox" style="float:left" v-model="voyageParams.shallowWater" /></div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align:right">
                                <label class="stat-label">Narrow Passage: </label>
                            </td>
                            <td>
                                <div><input type="checkbox" style="float:left" v-model="voyageParams.narrowPassage" /></div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <div class="right-grid">
            <div class="card" v-for="ship in voyageParams.shipInputs" v-bind:key="ship.name">
                <div class="container">
                    <h4>{{ship.loadoutName}}</h4>
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
                                <input type="number" v-model="ship.crewUnfitForDuty" />
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
</template>

<script>
    export default {
        name: "voyageParams Parameters",
        props: {
            voyageParams: {}
        },
        watch: {
            voyageParams: function () {
                this.emit("voyage-params-updates", this.voyageParams);
            }
        }
    }
</script>