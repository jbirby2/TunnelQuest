import Vue from 'vue';
import App from './App.vue';

Vue.config.productionTip = false;

/*
new Vue({
    render: (h) => h(App),
    data: {
        serverCode: "BLUE",
        hubUrl: "/blue_hub"
    }
}).$mount('#app');
*/

const app = new App({
    el: "#app",
    data: {
        serverCode: "BLUE",
        hubUrl: "/blue_hub"
    }
});