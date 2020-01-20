﻿import Vue from 'vue'
import Router from 'vue-router'

import ShipLoadouts from '@/components/ShipLoadouts.vue'
import CrewList from '@/components/CrewList.vue'
import Voyages from '@/components/Voyages.vue'
import Sailing from '@/components/Sailing.vue'

Vue.use(Router)

export default new Router({
    routes: [
        {
            path: '/',
            component: ShipLoadouts
        },
        {
            path: '/ShipLoadouts',
            component: ShipLoadouts
        },
        {
            path: '/Crew',
            component: CrewList
        },
        {
            path: '/Voyages',
            component: Voyages,
        },
        {
            path: '/Sailing/:id',
            component: Sailing
        }
    ]
})