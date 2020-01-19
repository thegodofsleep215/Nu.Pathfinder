<template>
    <div class="card">
        <div class="container">
            <div class="content-grid-container">
                <div class="header-grid">
                    <h3>{{ship.crewName}}</h3>
                </div>
                <div class="left-grid">
                    <h4>Ship's Stats</h4>
                    <p class="stat"><span class="stat-label">Ship Type:</span> {{ship.shipType}}</p>
                    <p class="stat"><span class="stat-label">Ship Size:</span> {{ship.shipSize}}</p>
                    <p class="stat"><span class="stat-label">Hull Hit Points:</span> {{ship.hullHitPoints}}</p>
                    <p class="stat"><span class="stat-label">Propulsion Types:</span> </p>
                    <div class="stat" v-for="prop in ship.propulsionTypes" v-bind:key="prop.propulsionType">
                        <p class="sub-stat"><span class="stat-label">Type:</span> {{prop.propulsionType}}</p>
                        <p class="sub-stat"><span class="stat-label">Ship Speed:</span> {{prop.shipSpeed}}</p>
                        <p class="sub-stat"><span class="stat-label">Hit Points:</span> {{prop.propulsionHitPoints}}</p>
                    </div>
                    <p class="stat"><span class="stat-label">Crew Size:</span> {{ship.crewSize}}</p>
                    <p class="stat"><span class="stat-label">Ship DC:</span> {{ship.shipDc}}</p>
                    <p class="stat"><span class="stat-label">Ship Piloting Bonus:</span> {{ship.shipPilotingBonus}}</p>
                    <p class="stat"><span class="stat-label">Ship Quality:</span> {{ship.shipQuality}}</p>
                </div>
                <div calass="right-grid">
                    <h4>Ship's Crew</h4>
                    <CrewMember v-for="cm in ship.shipsCrew"
                                v-bind:key="cm.name"
                                v-bind:crewMember="cm"></CrewMember>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    import CrewMember from './CrewMember.vue'
    export default {
        name: 'ShipLoadoutDetails',
        components: {
            CrewMember
        },
        props: {
            ship: {}
        },
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