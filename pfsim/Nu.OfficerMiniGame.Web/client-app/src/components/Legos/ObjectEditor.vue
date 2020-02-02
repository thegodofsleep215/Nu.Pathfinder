<template>
    <div class="grid-container">
        <AddModal v-model="isModalVisible"
                  @modal-ok="onAdd"
                  @modal-cancel="onCancel">
        </AddModal>
        <div class="header-grid">
            <h3>{{title}}</h3>
        </div>
        <div class="button-grid">
            <button class="add-button" v-on:click="showAdd()"></button>
        </div>
        <LeftSideList v-bind:url="url" @item-selected="loadItem"></LeftSideList>
        <div id="selectedDetail" class="right-grid">
            <component :is="loadedComponent"
                       :value="selectedItem"
                       @delete="onDelete"
                       v-if="showDetail"></component>
        </div>
    </div>
</template>

<script>
    import LeftSideList from './LeftSideList.vue'
    import EventBus from '../../scripts/EventBus.js'
    import AddModal from '../AddModal.vue'
    export default {
        name: "ObjectEditor",
        props: {
            url: String,
            title: String,
            detailComponent: String
        },
        components: {
            LeftSideList,
            AddModal
        },
        computed: {
            loadedComponent() {
                return () => import('../' + this.detailComponent);
            },
            showDetail() {
                if (typeof(this.selectedItem) === 'string') {
                    return true;
                }
                return false;
                //if (this.selectedItem == undefined || Object.entries(this.selectedItem).length === 0) {
                //    return false;
                //}
                //return true;
            }

        },
        data: function () {
            return {
                isModalVisible: false,
                selectedItem: { type: Object },
            }
        },
        mounted: function () {
        },
        methods: {
            showAdd() {
                this.isModalVisible = !this.isModalVisible;
            },
            onAdd(name) {
                this.isModalVisible = !this.isModalVisible;
                fetch('/' + this.url + '/Create?name=' + name, { method: 'POST' }).then(r => {
                    if (r.status == 200) {
                        EventBus.$emit("left-list-changed");
                    }
                });
            },
            onCancel() {
                this.isModalVisible = false;
            },
            onDelete(name) {
                var self = this;
                fetch('/' + this.url + '/Delete?name=' + name, { method: 'DELETE' }).then(r => {
                    if (r.status == 200) {
                        EventBus.$emit("left-list-changed");
                        self.selectedItem = undefined;
                    }
                })
            },
            loadItem(name) {
                this.selectedItem = name;
            },
        }
    }
</script>