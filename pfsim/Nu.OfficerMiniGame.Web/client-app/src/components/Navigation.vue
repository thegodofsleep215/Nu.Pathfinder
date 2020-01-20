<template>
    <div class="header-grid">
        <nav >
            <router-link class="nav-item" v-for="routes in links"
                         v-bind:key="routes.id"
                         :to="`${routes.page}`">
                {{routes.text}}
            </router-link>
        </nav>
    </div>
</template>

<script>
    export default {
        name: "Navigation",
        data() {
            return {
                links: [
                    {
                        id: 0,
                        text: "Ship Loadouts",
                        page: "/ShipLoadouts"
                    },
                    {
                        id: 1,
                        text: "Crew Members",
                        page: "/Crew"
                    },
                    {
                        id: 2,
                        text: "Voyages",
                        page: "/Voyages"
                    }
                ]
            }
        }
    }
</script>

<style>
    nav{
        list-style-type:none;
        margin: 0;
        padding: 0;
        overflow: hidden;
        background-color: #333;
    }

    .nav-item {
        float: left;
        display: block;
        color: white;
        text-align: center;
        padding: 14px 16px;
        text-decoration: none;
    }

    .nav-item:hover {
        background-color:#111;
    }
</style>