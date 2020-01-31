<template>
    <div class="left-grid">
        <div v-for="item in items" :key="item" class="card">
            <div class="container" v-on:click="loadItem(item)">
                <h3>{{item}}</h3>
            </div>
        </div>
    </div>
</template>

<script>
    import EventBus from '../../scripts/EventBus.js'
    export default {
        name: "LeftSideList",
        props: {
            url: String
        },
        data: function () {
            return {
                items: {}
            }
        },
        mounted: function () {
            EventBus.$on("left-list-changed", this.loadList);
            this.loadList();
        },
        methods: {
            loadList() {
                var self = this;
                fetch("/" + this.url +'/Names').then(r => r.json()).then(d => self.items = d);
            },
            loadItem(i) {
                this.$emit("item-selected", i);
            }
        }
    }
</script>