<template>
    <div id="app">
        <Nagivation></Nagivation>
        <router-view/>
    </div>
</template>

<script>
    import Nagivation from './components/Navigation.vue'

    export default {
        name: 'app',
        components: {
            Nagivation
        }
    }
</script>

<style>
@import './assets/minigame.css';
    #app {
        font-family: 'Avenir', Helvetica, Arial, sans-serif;
        -webkit-font-smoothing: antialiased;
        -moz-osx-font-smoothing: grayscale;
        text-align: center;
        color: #2c3e50;
    }
</style>
