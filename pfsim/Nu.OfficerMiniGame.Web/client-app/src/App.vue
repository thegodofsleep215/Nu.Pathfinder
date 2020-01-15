<template>
    <div id="app">
        <Ships></Ships>
    </div>
</template>

<script>
    import Ships from './components/Ships.vue'

    export default {
        name: 'app',
        components: {
            Ships
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
        margin-top: 60px;
    }
</style>
