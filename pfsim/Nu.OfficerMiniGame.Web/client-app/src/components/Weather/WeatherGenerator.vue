<template>
    <div class="grid-container">
        <div class="header-grid">
            <h3>Weather Generator</h3>
        </div>
        <div class="left-grid">
            <table>
                <tr>
                    <td align="center" colspan="2">
                        <button v-on:click="generateWeather">Generate</button>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label class="stat-label" style="float:right">Year:</label>
                    </td>
                    <td>
                        <input type="number" v-model="date.year" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label class="stat-label" style="float:right">Month:</label>
                    </td>
                    <td>
                        <input type="number" v-model="date.month" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label class="stat-label" style="float:right">Day:</label>
                    </td>
                    <td>
                        <input type="number" v-model="date.day" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label class="stat-label" style="float:right">Elevation:</label>
                    </td>
                    <td>
                        <input type="number" v-model="elevation" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label class="stat-label" style="float:right">Region:</label>
                    </td>
                    <td>
                        <select v-model="region" style="float:left">
                            <option valye="Tropical" selected>Tropical</option>
                            <option value="Cold">Cold</option>
                            <option value="Temperate">Temperate</option>
                        </select>
                    </td>
                </tr>
            </table>
        </div>
        <div class="right-grid">
            <WeatherResult v-model="weather"></WeatherResult>
        </div>
    </div>
</template>

<script>
    import WeatherResult from './WeatherResult.vue'
    export default {
        name: "WeatherGenerator",
        components: {
            WeatherResult
        },
        data: function () {
            return {
                date: {
                    year: 2000,
                    month: 1,
                    day: 1,
                },
                region: "",
                elevation: 0,
                weather: {}
            }
        },
        computed: {
            parameters() {
                var dateAsString = this.date.year + "/" + this.date.month + "/" + this.date.day + " 0:0:0";

                return {
                    date: dateAsString,
                    region: this.region,
                    elevationFt: this.elevation
                };
            }
        },
        methods: {
            generateWeather() {
                fetch('/Weather/Generate', {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify(this.parameters)
                }).then(r => r.json()).then(d => { this.weather = d; });
            }
        }
    }
</script>