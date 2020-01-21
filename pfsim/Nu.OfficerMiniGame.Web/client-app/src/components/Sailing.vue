<template>
    <div>
        <div>
            <h3>Sailing on the Voyage - {{voyageName}}</h3>
            <button v-on:click="sail">Sail!</button>
        </div>

        <div class="grid-container">
            <div class="left-grid">
                <table align="center">
                    <tr>
                        <td style="text-align:right">
                            <label class="stat-label">Travel Progress: </label>
                        </td>
                        <td>
                            <label type="text" style="float:left">XXX / {{voyage.daysPlanned}}</label>
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

        <div v-for="sr in sailResult" v-bind:key="sr.loadout">
            <h3>{{sr.loadout}}</h3>
            <p style="text-align:left" v-for="msg in sr.results.messages" v-bind:key="msg">{{msg}}</p>
        </div>
    </div>
</template>

<script>
    export default {
        name: "Sailing",
        data: function () {
            return {
                sailResult: {},
                voyage: {}
            }
        },
        computed: {
            voyageName() {
                return this.$route.params.id;
            },
            voyageParameters() {
                return {
                    voyageName: this.voyageName,
                    narrowPassage: this.voyage.narrowPassage,
                    shallowWater: this.voyage.shallowWater,
                    openOcean: this.voyage.openOcean,
                    nightStatus: this.voyage.nightStatus,
                };
            },
        },
        mounted: function () {
            var self = this;
            fetch('/Voyage?name=' + this.voyageName).then(r => r.json()).then(d => self.voyage = d);
        },
        methods: {
            sail() {
                var self = this;
                fetch('/SailingEngine/Sail', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(this.voyageParameters)
                }).then(r => r.json()).then(d => self.sailResult = d);
            }
        }
    }
</script>