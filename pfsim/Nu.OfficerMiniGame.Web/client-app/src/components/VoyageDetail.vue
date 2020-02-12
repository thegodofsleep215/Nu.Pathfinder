<template>
    <div class="card">
        <SetCourse v-model="isSetCourseVisable"
                   @setcourse-ok="onSetCourseOk"
                   @setcourse-cancel="onSetCourseCancel">
        </SetCourse>
        <div>
            <button v-on:click="showAdd()">Set Course</button>
        </div>
    </div>
</template>

<script>
    import SetCourse from './SetCourse.vue'
    export default {
        name: "VoyageDetail",
        props: {
            value: { type: String }
        },
        components: {
            SetCourse
        },
        data: function () {
            return {
                voyage: {},
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
        methods: {
            load(name) {
                var self = this;
                if (this.value.length > 0) {
                    fetch('/Voyage?name=' + name).then(r => r.json())
                        .then(d => {
                            self.voyage = d;
                            self.parseDate();
                        });
                }

            },
            updateVoyage(v) {
                fetch('/Voyage/Update', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(v)
                });
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
            onSetCourseOk() {
                this.isSetCourseVisable = false;
            },
            onSetCourseCancel() {
                this.isSetCourseVisable = false;
            }
        }
    }
</script>