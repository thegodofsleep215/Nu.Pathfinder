<template>
    <div class="card">
        <div class="container">
            <h4>Weather Conditions</h4>
            <div>Date: {{value.date}}</div>
            <div>Temperature: {{value.temperatureInF}} </div>
            <div>Precipitation: {{value.precipitationType}}</div>
            <div>Precipitation Start: {{value.precipitationStartTime}}:00</div>
            <div>Precipitation Duration: {{value.precipitationHours}}</div>
            <div>Cloud Cover: {{value.cloudCover}}</div>
            <div>Wind Speed: {{value.windSpeed}}</div>
        </div>
    </div>
</template>

<script>
    export default {
        name: "WeatherResult",
        props: {
            value: {}
        }
    }
</script>