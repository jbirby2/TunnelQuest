import Vue from 'vue';
import Router from 'vue-router';

import ChatPage from './components/ChatPage.vue';
import NewspaperPage from './components/NewspaperPage.vue';
import ItemPage from './components/ItemPage.vue';

Vue.use(Router);

export default new Router({
    routes: [
        {
            path: '/',
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
});
