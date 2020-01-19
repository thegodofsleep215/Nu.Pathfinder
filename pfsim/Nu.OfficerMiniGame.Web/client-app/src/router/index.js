import Vue from 'vue'
import Router from 'vue-router'

import ShipLoadouts from '@/components/ShipLoadouts.vue'
import CrewList from '@/components/CrewList.vue'

Vue.use(Router)

export default new Router({
    routes: [
        {
            path: '/',
            name: 'ShipLoadouts',
            component: ShipLoadouts
        },
        {
            path: '/ShipLoadouts',
            name: 'ShipLoadouts',
            component: ShipLoadouts
        },
        {
            path: '/Crew',
            name: 'Crew',
            component: CrewList
        },
    ]
})