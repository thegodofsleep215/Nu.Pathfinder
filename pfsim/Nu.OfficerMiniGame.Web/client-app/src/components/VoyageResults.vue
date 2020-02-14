<template>

    <div>
        <div v-for="sr in results" v-bind:key="sr.loadout">
            <h3>{{sr.loadout}}</h3>
            <p style="text-align:left" v-for="msg in sr.messages" v-bind:key="msg">{{msg}}</p>
        </div>
    </div>

</template>

<script>
    export default {
        name: "VoyageResults",
        props: {
            results: {}
        }
    }
</script>