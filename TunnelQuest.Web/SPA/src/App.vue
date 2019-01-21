<template>
    <div id="app">

        <div class="tqNavBar">
            <router-link to="/">Auction House View</router-link> |
            <router-link to="/chat">Chat View</router-link>
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
    document {
        padding: 0;
        margin: 0;
    }

    body {
        padding: 0;
        margin: 0;
        background-image: url("/assets/background.png");
        background-size: cover;
        background-attachment: fixed;
        color: #ffffff;
    }

    .tqNavBar {
        background-color: #d19200;
    }
</style>
