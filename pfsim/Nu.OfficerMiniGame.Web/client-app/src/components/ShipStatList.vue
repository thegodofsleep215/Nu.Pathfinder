<template>
    <div class="grid-container">
        <div class="button-grid">
            <button class="add-button" v-on:click="showAdd()"></button>
        </div>
        <div class="left-grid">
            <div v-for="ship in shipStats" v-bind:key="ship" class="card">
                <div class="container" v-on:click="loadShip(ship)">
                    <h3>{{ship}}</h3>
                </div>
            </div>
        </div>
        <div id="selectedShip" class="right-grid" style="visibility:hidden">
            <ShipStatDetails v-bind:shipName="selectedShip"
                        @delete-shipstats="onDelete"
                             ></ShipStatDetails>
        </div>

        <AddModal v-model="isModalVisible"
                  @modal-ok="onAdd"
                  @modal-cancel="onCancel">
        </AddModal>

    </div>

</template>

<script>
    import AddModal from './AddModal.vue'
    import ShipStatDetails from './ShipStatDetails.vue'
    export default {
        name: "ShipStatList",
        components: {
            AddModal,
            ShipStatDetails
        },
        data: function () {
            return {
                isModalVisible: false,
                shipStats: {},
                selectedShip: {}
            }
        },
        mounted: function () {
            this.loadNames();
        },
        methods: {
            loadNames() {
                var self = this;
                fetch('/ShipStats/Names').then(r => r.json()).then(d => self.shipStats = d);
            },
            loadShip(name) {
                this.selectedShip = name;
                document.getElementById("selectedShip").style.visibility = "visible";
            },
            showAdd() {
                this.isModalVisible = !this.isModalVisible;
            },
            onAdd(name) {
                this.isModalVisible = !this.isModalVisible;
                fetch('/ShipStats/Create?name=' + name, { method: 'POST' }).then(r => {
                    if (r.status == 200) {
                        this.loadNames();
                    }
                });
            },
            onCancel() {
                this.isModalVisible = false;
            },
            onDelete(name) {
                var self = this;
                fetch('/ShipStats/Delete?name=' + name, { method: 'DELETE' }).then(r => {
                    if (r.status == 200) {
                        self.loadNames();
                        self.selectedShip = undefined;
                    }
                })
            }


        }
    }
</script>