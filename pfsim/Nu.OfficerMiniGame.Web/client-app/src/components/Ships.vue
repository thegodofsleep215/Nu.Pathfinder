<template>
    <div class="grid-container">
        <div class="left-grid">
            <div v-for="ship in ships" :key="ship" class="card">
                <div class="container" v-on:click="loadShip(ship)">
                    <h3>{{ship}}</h3>
                </div>
            </div>
        </div>
        <div id="selectedShip" class="right-grid" style="visibility:hidden">
            <ShipDetails v-bind:ship="ship"></ShipDetails>
        </div>
    </div>
</template>

<script>
    import ShipDetails from './ShipDetails.vue'
    export default {
        name: 'Ships',
        components: {
            ShipDetails
        },
        data: function () {
            return {
                ships: [],
                ship: {}
            }
        },
        mounted: function () {
            var self = this;
            fetch('/Ship/Names').then(r => r.json()).then(d => self.ships = d);
        },
        methods: {
            loadShip(name) {
                document.getElementById("selectedShip").style.visibility = "visible";
                var self = this;
                fetch('/Ship?name=' + name).then(r => r.json()).then(d => self.ship = d);
            }
        }
    }
</script>

<style>
   
</style>