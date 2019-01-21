import Vue from 'vue';
import Router from 'vue-router';

import ChatView from './components/ChatView.vue';
import AuctionHouseView from './components/AuctionHouseView.vue';

Vue.use(Router);

export default new Router({
    routes: [
        {
            path: '/',
            name: 'Auction House View',
            component: AuctionHouseView,
        },
        {
            path: '/chat',
            name: 'Chat View',
            component: ChatView,
        },
    ],
});
