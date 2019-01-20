<template>
    <div id="app">

        <div id="nav">
            <router-link to="/">Main Menu</router-link> |
            <router-link to="/chat">Chat View</router-link> |
            <router-link to="/auctionhouse">Auction House View</router-link>
        </div>

        <transition>
            <keep-alive>
                <router-view />
            </keep-alive>
        </transition>

    </div>
</template>

<script lang="ts">
    import Vue from "vue";
    
    import TQGlobals from "./classes/TQGlobals";


    export default Vue.extend({

        created: function () {
        },
        mounted: function () {
            TQGlobals.init();
        }, // end mounted
        beforeDestroy: function () {
            TQGlobals.connection.stop();
        },
        methods: {
        }, // end methods
        components: {
        }
    });
</script>

<style>

    #app {
        font-family: 'Avenir', Helvetica, Arial, sans-serif;
        -webkit-font-smoothing: antialiased;
        -moz-osx-font-smoothing: grayscale;
        text-align: center;
        color: #2c3e50;
    }

    #nav {
        padding: 30px;
    }

    #nav a {
        font-weight: bold;
        color: #2c3e50;
    }

    #nav a.router-link-exact-active {
        color: #42b983;
    }
</style>
