<template>
    <div class="card">
        <SetCourse :value="isSetCourseVisable" :input="newCourseParams" :inputShipStates="inputShipStates" :inputShipLoadouts="inputShipLoadouts"
                   @setcourse-ok="onSetCourseOk"
                   @setcourse-cancel="onSetCourseCancel">
        </SetCourse>
        <div>
            <button v-on:click="showAdd()">Set Course</button>
        </div>
        <VoyageStatus v-bind:state="state"></VoyageStatus>
        <VoyageParameters v-bind:voyageParams="voyageParams" @voyage-params-updates="voyageParamsUpdated"></VoyageParameters>
        <div>
            <button v-on:click="sailClicked">Sail</button>
        </div>
        <div>
            <VoyageResults v-bind:results="results"></VoyageResults>
        </div>
    </div>
</template>

<script>
    import SetCourse from './SetCourse.vue'
    import VoyageStatus from './VoyageStatus.vue'
    import VoyageParameters from './VoyageParameters.vue'
    import VoyageResults from './VoyageResults.vue'
    export default {
        name: "VoyageDetail",
        props: {
            value: { type: String }
        },
        components: {
            SetCourse,
            VoyageStatus,
            VoyageParameters,
            VoyageResults
        },
        data: function () {
            return {
                voyageParams: {},
                state: {},
                results: {},
                isSetCourseVisable: false,
            }
        },
        mounted: function () {
            this.load(this.value);
        },
        watch: {
            value: function () {
                this.load(this.value);
            },
        },
        computed: {
            inputShipStates() {
                return Array.from(this.voyageParams.shipInputs).map(si => {
                    return {
                        loadoutName: si.loadoutName,
                        diseasedCrew: si.diseasedCrew,
                        disciplineStandards: si.disciplineStandards,
                        swabbies: si.swabbies
                    }
                });

            },
            inputShipLoadouts() {
                    return Array.from(this.voyageParams.shipInputs).map(si => si.loadoutName)
            },
            newCourseParams() {
                var ret = {
                    startDate: this.state.currentDate,
                    nightStatus: this.voyageParams.nightStatus,
                };
                return ret;
            }
        },
        methods: {
            load(name) {
                var self = this;
                if (this.value.length > 0) {
                    fetch('/Voyage/GetCurrentProgress?name=' + name).then(r => r.json())
                        .then(d => {
                            self.state = d.state;
                            self.voyageParams = d.voyageParams;
                            self.results = d.results
                        });
                }
            },
            async setCourse(course) {
                return fetch('/Voyage/SetCourse?name=' + this.value, {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(course)
                }).then((r) => { return r });
            },
            async sail() {
                return fetch('/Voyage/Sail', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(this.voyageParams)
                }).then(r => { return r; });
            },
            async sailClicked() {
                await this.sail();
                this.load(this.value);
            },
            voyageParamsUpdated(params) {
                this.voyageParams = params;
            },
            deleteVoyage() {
                if (confirm("Do the thing?")) {
                    this.$emit("delete-voyage", this.value.name);
                    self.value = undefined;
                }
            },
            showAdd() {
                this.isSetCourseVisable = true;
            },
            async onSetCourseOk(course) {
                this.isSetCourseVisable = false;
                await this.setCourse(course);
                this.load(this.value);
            },
            onSetCourseCancel() {
                this.isSetCourseVisable = false;
            }
        }
    }
</script>