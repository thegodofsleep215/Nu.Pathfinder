<template>
    <div class="header-grid">
        <div class="navbar">
            <div v-for="routes in links" v-bind:key="routes.id" class="dropdown">
                <div v-if="routes.children">
                    <button class="dropbtn">
                        {{ routes.text }}
                    </button>
                    <div v-if="routes.children" class="dropdown-content">
                        <router-link v-for="child in routes.children" v-bind:key="child.id" class="dropdown-item" :to="`${child.page}`">{{child.text}}</router-link>
                    </div>
                </div>
                <router-link class="nav-item" :to="`${routes.page}`" v-else>
                    {{routes.text}}
                </router-link>
            </div>
        </div>
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
                        text: "Ship Stats",
                        page: "/ShipStats"

                    },
                    {
                        id: 1,
                        text: "Ship Loadouts",
                        page: "/ShipLoadouts"
                    },
                    {
                        id: 2,
                        text: "Crew Members",
                        page: "/Crew"
                    },
                    {
                        id: 3,
                        text: "Voyages",
                        page: "/Voyages"
                    },
                    {
                        text: "Tools",
                        children: [
                            {
                                id: 1,
                                text: "Weather",
                                page: "/Weather"
                            }
                        ]
                    }

                ]
            }
        },
    }
</script>

<style>
    .navbar {
        list-style-type: none;
        margin: 0;
        padding: 0;
        overflow: hidden;
        background-color: #333;
    }

    .nav-item {
        display: block;
        float: left;
        font-size: 16px;
        color: white;
        text-align: center;
        padding: 14px 16px;
        text-decoration: none;
    }

    .dropdown {
        float: left;
        overflow: hidden;
    }

        .dropdown .dropbtn {
            font-size: 16px;
            border: none;
            outline: none;
            color: white;
            padding: 14px 16px;
            background-color: inherit;
            font-family: inherit;
            margin: 0;
        }

        .navbar .nav-item:hover, .dropdown:hover .dropbtn {
            background-color: #111;
        }

    .dropdown-content {
        display: none;
        position: absolute;
        background-color: #f9f9f9;
        min-width: 160px;
        box-shadow: 0px 8px 16px 0px rgba(0,0,0,0.2);
        z-index: 1;
    }

        .dropdown-content a {
            float: none;
            color: black;
            padding: 12px 16px;
            text-decoration: none;
            display: block;
            text-align: left;
        }

            .dropdown-content a:hover {
                background-color: #ddd;
            }

    .dropdown:hover .dropdown-content {
        display: block;
    }
</style>