<template>
    <div class="grid-container">

        <AddModal v-model="isAddCrewVisible" 
                      @add-crew="onAddCrew"
                      @add-cancel="onAddCrewCancel"></AddModal>
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
            <CrewDetail v-model="currentCrewMember"
                        @delete-crew="onDeleteCrew"
                        v-if="showDetail"></CrewDetail>
        </div>

    </div>

</template>

<script>
    import CrewDetail from './CrewDetail.vue'
    import AddModal from './AddModal.vue'
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
        computed: {
            currentCrewMember() {
                return this.crew;
            },
            showDetail() {
                if (this.currentCrewMember == undefined || Object.entries(this.currentCrewMember).length === 0) {
                    return false;
                }
                return true;
            }
        },
        methods: {
            loadNames() {
                var self = this;
                fetch('/CrewMemberStats/Names').then(r => r.json()).then(d => self.crewMembers = d);
            },
            loadCrewMember(name) {
                var self = this;
                fetch('/CrewMemberStats?name=' + name).then(r => r.json()).then(d => self.crew = d);
            },
            showAdd() {
                this.isAddCrewVisible = !this.isAddCrewVisible;
            },
            saveNewCrewMember() {
                this.isAddCrewVisible = !this.isAddCrewVisible;
            },
            onAddCrew(name) {
                this.isAddCrewVisible = !this.isAddCrewVisible;
                fetch('/CrewMemberStats/Create?name=' + name, { method: 'POST' }).then(r => {
                    if (r.status == 200) {
                        this.loadNames();
                    }
                });
            },
            onAddCrewCancel() {
                this.isAddCrewVisible = false;
            },
            onDeleteCrew(name) {
                var self = this;
                fetch('/CrewMemberStats/Delete?name=' + name, { method: 'DELETE' }).then(r => {
                    if (r.status == 200) {
                        self.loadNames();
                        self.crew = {};
                    }
                })
            }
        }
    }
</script>

<style></style>