import Vue from 'vue';
import Router from 'vue-router';

import MainMenu from './components/MainMenu.vue';
import ChatView from './components/ChatView.vue';
import AuctionHouseView from './components/AuctionHouseView.vue';

Vue.use(Router);

export default new Router({
    routes: [
        {
            path: '/',
            name: 'Main Menu',
            component: MainMenu,
        },
        {
            path: '/chat',
            name: 'Chat View',
            component: ChatView,
        },
        {
            path: '/auctionhouse',
            name: 'Auction House View',
            component: AuctionHouseView,
        },
    ],
});
