<template>
    <div class="card">
        <div class="container">
            <h3>{{crew.crewName}}</h3>
            <div>
                <p class="stat">Ship Type: {{crew.shipType}}</p>
                <p class="stat">Ship Size: {{crew.shipSize}}</p>
                <p class="stat">Hull Hit Points: {{crew.hullHitPoints}}</p>
                <p class="stat">Propulsion Types:</p>
                <div class="stat" v-for="prop in crew.propulsionTypes" v-bind:key="prop.propulsionType">
                    <p class="sub-stat">Type: {{prop.propulsionType}}</p>
                    <p class="sub-stat">Ship Speed: {{prop.shipSpeed}}</p>
                    <p class="sub-stat">Hit Points: {{prop.propulsionHitPoints}}</p>
                </div>
                <p class="stat">Crew Size: {{crew.crewSize}}</p>
                <p class="stat">Ship DC: {{crew.shipDc}}</p>
                <p class="stat">Ship Piloting Bonus: {{crew.shipPilotingBonus}}</p>
                <p class="stat">Ship Quality: {{crew.shipQuality}}</p>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        name: 'Crew',
        props: {
            crew: {}
        },
    }
</script>

<style>
</style>