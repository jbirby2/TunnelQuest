import Vue from 'vue';
import VueRouter from 'vue-router';

import MainMenuPage from './components/MainMenuPage.vue';
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
            name: 'Main Menu',
            component: MainMenuPage,
        },

        {
            path: '/auctions',
            name: 'Auction House Mode',
            component: AuctionHousePage,
        },
        {
            path: '/auctionhouse',
            name: 'Auction House Mode',
            component: AuctionHousePage,
        },

        {
            path: '/newspaper',
            name: 'Newspaper Mode',
            component: NewspaperPage,
        },

        {
            path: '/chat',
            name: 'Chat Mode',
            component: ChatPage,
        },

        {
            path: '/item/:itemName',
            name: 'Item Information',
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
