<template>
    <div>
        <div>
            <h3>Sailing on the Voyage - {{voyageName}}</h3>
            <button v-on:click="sail">Sail!</button>
        </div>
        <div v-for="sr in sailResult" v-bind:key="sr.loadout">
            <h3>{{sr.loadout}}</h3>
            <p style="text-align:left" v-for="msg in sr.results.messages" v-bind:key="msg">{{msg}}</p>
        </div>
    </div>
</template>

<script>
    export default {
        name: "Sailing",
        data: function () {
            return { sailResult: {} }
        },
        computed: {
            voyageName() {
                return this.$route.params.id;
            },
            voyageParameters() {
                return { voyageName: this.voyageName };
            },
        },
        methods: {
            sail() {
                var self = this;
                fetch('/SailingEngine/Sail', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(this.voyageParameters)
                }).then(r => r.json()).then(d => self.sailResult = d);
            }
        }
    }
</script>