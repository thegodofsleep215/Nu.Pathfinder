<template>
    <div class="grid-container">

        <AddModal v-model="isAddVoyageVisible" @add-crew="onAddVoyage"></AddModal>
        <div class="header-grid">
            <h3>Crew Members</h3>
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
        <div id="selectedCrew" class="right-grid">
            <!--CrewDetail v-model="currentCrewMember"
                        @delete-crew="onDeleteCrew"
                        v-if="showDetail"></!--CrewDetail -->
        </div>
    </div>
</template>

<script>
    import AddModal from '.\AddModal.vue'
    export default {
        name: "Voyages",
        components: {
            AddModal
        },
        data: function () {
            return {
                isAddVoyageVisible: false,
                voyages: {},
                voyage: {},
            }
        },
        methods: {
            loadNames() {
                var self = this;
                fetch('/Voyage/Names').then(r => r.json()).then(d => self.crewMembers = d);
            },
            loadVoyage(name) {
                var self = this;
                fetch('/Voygae?name=' + name).then(r => r.json()).then(d => self.crew = d);
            },
            onAddVoyage(name) {
                this.isAddVoyageVisible = !this.isAddVoyageVisible;
                fetch('/Voyage/Create?name=' + name, { method: 'POST' }).then(r => {
                    if (r.status == 200) {
                        this.loadNames();
                    }
                });
            },
            onAddCrewCancel() {
                this.isAddVoyageVisible = false;
            }
        }
    }
</script>

<style>
</style>