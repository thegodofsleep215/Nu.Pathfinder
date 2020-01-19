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
            <ShipLoadoutDetails v-bind:ship="ship"></ShipLoadoutDetails>
        </div>
    </div>
</template>

<script>
    import ShipLoadoutDetails from './ShipLoadoutDetails.vue'
    export default {
        name: 'ShipLoadouts',
        components: {
            ShipLoadoutDetails
        },
        data: function () {
            return {
                ships: [],
                ship: {}
            }
        },
        mounted: function () {
            var self = this;
            fetch('/ShipLoadout/Names').then(r => r.json()).then(d => self.ships = d);
        },
        methods: {
            loadShip(name) {
                document.getElementById("selectedShip").style.visibility = "visible";
                var self = this;
                fetch('/ShipLoadout?name=' + name).then(r => r.json()).then(d => self.ship = d);
            }
        }
    }
</script>

<style>
   
</style>