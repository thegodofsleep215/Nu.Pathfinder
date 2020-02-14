<template>
    <div class="card">
        <table align="center">
            <tr>
                <td style="text-align:right">
                    <label class="stat-label">Current Date: </label>
                </td>
                <td>
                    <label type="text" style="float:left">{{state.currentDate}}</label>
                </td>
            </tr>
            <tr>
                <td style="text-align:right">
                    <label class="stat-label">Travel Progress: </label>
                </td>
                <td>
                    <label type="text" style="float:left">{{state.progressMade}}/{{state.daysPlanned}}</label>
                </td>
            </tr>
        </table>
        <WeatherResult v-model="state.weatherConditions"></WeatherResult>
    </div>
</template>

<script>
    import WeatherResult from './Weather/WeatherResult.vue'
    export default {
        name: "VoyageStatus",
        components: {
            WeatherResult
        },
        props: {
            state: {}
        }
    }
</script>