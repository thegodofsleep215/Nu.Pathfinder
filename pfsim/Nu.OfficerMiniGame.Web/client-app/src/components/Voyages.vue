<template>
    <div>
        <div class="modal-backdrop" v-show="isNewVoyageVisable">
            <div class="modal">
                <section class="modal-body">
                    <slot name="body">
                        <VoyageContent v-model="setCourse"></VoyageContent>
                    </slot>

                </section>
                <footer class="modal-footer">
                    <slot name="footer">
                        <button type="button" @click="onAlterVoyageOk">Ok</button>
                        <button type="button" @click="onAlterVoyageCancel">Cancel</button>
                    </slot>
                </footer>
            </div>
        </div>

        <div class="header-grid">
            <h3>Voyages</h3>
        </div>
        <div class="button-grid">
            <button class="add-button" v-on:click="showAdd()"></button>
        </div>
        <LeftSideList v-bind:url="url" @item-selected="loadItem"></LeftSideList>
    </div>


</template>

<script>
    import LeftSideList from './Legos/LeftSideList.vue'
    import VoyageContent from './VoyageContent.vue'
    export default {
        name: "Voyages",
        components: {
            LeftSideList,
            VoyageContent
        },
        data: function () {
            return {
                url: "Voyage",
                isNewVoyageVisable: false,
                setCourse: {}
            };
        },
        methods: {
            showAdd() {
                this.setCourse = {};
                this.isNewVoyageVisable = true;
            },
            onAlterVoyageOk() {
                var self = this;
                fetch('/Voyage/SetCourse?name=' + self.setCourse.name, {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(self.setCourse)
                });
                this.isNewVoyageVisable = false;
            },
            onAlterVoyageCancel() {
                this.isNewVoyageVisable = false;
            }
        }
    }
</script>

<style>
</style>