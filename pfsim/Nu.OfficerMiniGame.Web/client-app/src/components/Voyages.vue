<template>
    <div class="grid-container">

        <AddModal v-model="isAddVoyageVisible"
                  @modal-ok="onAddVoyage"
                  @modal-cancel="onAddCancel"></AddModal>
        <div class="header-grid">
            <h3>Voyages</h3>
        </div>
        <div class="button-grid">
            <button class="add-button" v-on:click="showAdd()"></button>
        </div>
        <div class="left-grid">
            <div v-for="v in voyages" :key="v" class="card">
                <div class="container" v-on:click="loadVoyage(v)">
                    <h3>{{v}}</h3>
                </div>
            </div>
        </div>
        <div class="right-grid">
            <VoyageDetail v-model="selectedVoyage"
                          @delete-voyage="deleteVoyage"
                          v-if="showDetail"></VoyageDetail>
        </div>
    </div>
</template>

<script>
    import AddModal from './AddModal.vue'
    import VoyageDetail from './VoyageDetail.vue'
    export default {
        name: "Voyages",
        components: {
            AddModal,
            VoyageDetail
        },
        data: function () {
            return {
                isAddVoyageVisible: false,
                voyages: {},
                voyage: {},
                crew: { name: "Foo" }
            }
        },
        mounted: function () {
            this.loadNames();
        },
        computed: {
            selectedVoyage() {
                return this.voyage;
            },
            showDetail() {
                if (this.selectedVoyage == undefined || Object.entries(this.selectedVoyage).length === 0) {
                    return false;
                }
                return true;
            }
        },
        methods: {
            loadNames() {
                var self = this;
                fetch('/Voyage/Names').then(r => r.json()).then(d => self.voyages = d);
            },
            loadVoyage(name) {
                var self = this;
                fetch('/Voyage?name=' + name).then(r => r.json()).then(d => self.voyage = d);
            },
            showAdd() {
                this.isAddVoyageVisible = true;
            },
            onAddVoyage(name) {
                this.isAddVoyageVisible = false;
                fetch('/Voyage/Create?name=' + name, { method: 'POST' }).then(r => {
                    if (r.status == 200) {
                        this.loadNames();
                    }
                });
            },
            onAddCancel() {
                this.isAddVoyageVisible = false;
            },
            deleteVoyage(name) {
                var self = this;
                fetch('/Voyage/Delete?name=' + name, { method: 'DELETE' }).then(r => {
                    if (r.status == 200) {
                        self.loadNames();
                        self.voyage = {};
                    }
                })
            }
        }
    }
</script>

<style>
</style>