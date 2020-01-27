<template>
    <div class="grid-container">
        <div class="left-grid">
            <div v-for="ship in shipStats" v-bind:key="ship" class="card">
                <div class="container" v-on:click="loadShip(ship)">
                    <h3>{{ship}}</h3>
                </div>
            </div>
        </div>
        <div id="selectedShip" class="right-grid" style="visibility:hidden">
            <ShipStatDetails v-bind:shipName="selectedShip"></ShipStatDetails>
        </div>
    </div>

</template>

<script>
    import ShipStatDetails from './ShipStatDetails.vue'
    export default {
        name: "ShipStatList",
        components: {
            ShipStatDetails
        },
        data: function () {
            return {
                shipStats: {},
                selectedShip: {}
            }
        },
        mounted: function () {
            var self = this;
            fetch('/ShipStats/Names').then(r => r.json()).then(d => self.shipStats = d);
        },
        methods: {
            loadShip(name) {
                this.selectedShip = name;
                document.getElementById("selectedShip").style.visibility = "visible";
            }
        }
    }
</script>