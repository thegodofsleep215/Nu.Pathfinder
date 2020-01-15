<template>
    <div class="grid-container">
        <div class="ships-grid">
            <div v-for="ship in ships" :key="ship" class="card">
                <div class="container" v-on:click="loadShip(ship)">
                    <h3>{{ship}}</h3>
                </div>
            </div>
        </div>
        <div id="selectedCrew" class="right-grid" style="visibility:hidden">
            <Crew v-bind:crew="crew"></Crew>
        </div>
    </div>
</template>

<script>
    import Crew from './Crew.vue'
    export default {
        name: 'Ships',
        components: {
            Crew
        },
        data: function () {
            return {
                ships: [],
                crew: {}
            }
        },
        mounted: function () {
            var self = this;
            fetch('/Ship/Names').then(r => r.json()).then(d => self.ships = d);
        },
        methods: {
            loadShip(name) {
                document.getElementById("selectedCrew").style.visibility = "visible";
                var self = this;
                fetch('/Ship?name=' + name).then(r => r.json()).then(d => self.crew = d);
            }
        }
    }
</script>

<style>
   
</style>