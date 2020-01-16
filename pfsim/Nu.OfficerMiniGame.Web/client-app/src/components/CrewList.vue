<template>
    <div class="grid-container">

        <AddCrewModal v-model="isAddCrewVisible" @add-crew="onAddCrew"></AddCrewModal>
        <div class="header-grid">
            <h3>Crew Members</h3>
        </div>
        <div class="button-grid">
            <button class="add-button" v-on:click="showAdd()"></button>
        </div>
        <div class="left-grid">
            <div v-for="cm in crewMembers" :key="cm" class="card">
                <div class="container" v-on:click="loadCrewMember(cm)">
                    <h3>{{cm}}</h3>
                </div>
            </div>
        </div>
        <div id="selectedCrew" class="right-grid">
            <CrewDetail v-bind:data="crew"></CrewDetail>
        </div>

    </div>

</template>

<script>
    import CrewDetail from './CrewDetail.vue'
    import AddCrewModal from './AddCrewModal.vue'
    export default {
        name: "CrewList",
        components: {
            CrewDetail,
            AddCrewModal
        },
        props: {
            crewMembers: {},
            crew: {},
        },
        data: function () {
            return {
                isAddCrewVisible: false
            };
        },
        mounted: function () {
            this.loadNames();
        },
        methods: {
            loadNames() {
                var self = this;
                fetch('/Crew/Names').then(r => r.json()).then(d => self.crewMembers = d);
            },
            loadCrewMember(name) {
                //document.getElementById("selectedCrew").style.visibility = "visible";
                var self = this;
                fetch('/Crew?name=' + name).then(r => r.json()).then(d => self.crew = d);
            },
            showAdd() {
                this.isAddCrewVisible = !this.isAddCrewVisible;
            },
            saveNewCrewMember() {
                this.isAddCrewVisible = !this.isAddCrewVisible;
            },
            onAddCrew(e) {
                alert(e);
                this.isAddCrewVisible = !this.isAddCrewVisible;
            }
        }
    }
</script>

<style></style>