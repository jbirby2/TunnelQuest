import Vue from 'vue';
import VueRouter from 'vue-router';

import ChatPage from './components/ChatPage.vue';
import AuctionHousePage from './components/AuctionHousePage.vue';
import NewspaperPage from './components/NewspaperPage.vue';
import ItemPage from './components/ItemPage.vue';

Vue.use(VueRouter);

export default new VueRouter({

    mode: "history",

    routes: [
        {
            path: '/',
            name: 'AuctionHousePage',
            component: AuctionHousePage,
        },
        {
            path: '/newspaper',
            name: 'NewspaperPage',
            component: NewspaperPage,
        },
        {
            path: '/chat',
            name: 'ChatPage',
            component: ChatPage,
        },
        {
            path: '/item/:itemName',
            name: 'ItemPage',
            component: ItemPage,
        },
    ],

    scrollBehavior(to, from, savedPosition) {
        if (savedPosition)
            return savedPosition
        else
            return { x: 0, y: 0 }
    }
});
