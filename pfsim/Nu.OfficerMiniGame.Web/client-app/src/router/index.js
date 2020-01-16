import Vue from 'vue'
import Router from 'vue-router'

import Ships from '@/components/Ships.vue'
import CrewList from '@/components/CrewList.vue'

Vue.use(Router)

export default new Router({
    routes: [
        {
            path: '/',
            name: 'Ships',
            component: Ships
        },
        {
            path: '/Ships',
            name: 'Ships',
            component: Ships
        },
        {
            path: '/Crew',
            name: 'Crew',
            component: CrewList
        },
    ]
})