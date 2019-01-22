
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


    /* fade transition */

    .fade-enter-active, .fade-leave-active {
        transition: opacity .5s
    }

    .fade-enter, .fade-leave-to /* .fade-leave-active in <2.1.8 */ {
        opacity: 0
    }

    /* slide transition */

    .slide-enter {
        transform: translateX(100%)
    }

    .slide-enter-to {
        transform: translateX(0)
    }

    .slide-enter-active {
        /*position: absolute*/
    }

    .slide-leave {
        transform: translateX(0)
    }

    .slide-leave-to {
        transform: translateX(-100%)
    }

    .slide-enter-active,
    .slide-leave-active {
        transition: all 750ms ease-in-out
    }

    /* slideleft transition */

    .slideleft-enter {
        transform: translateX(100%)
    }
    .slideleft-enter-to {
        transform: translateX(0)
    }
    .slideleft-enter-active {
        /*position: absolute*/
    }
    .slideleft-leave {
        transform: translateX(0)
    }
    .slideleft-leave-to {
        transform: translateX(-100%)
    }
    .slideleft-enter-active,
    .slideleft-leave-active {
        transition: all 750ms ease-in-out
    }

    /* slidedown transition */
    /*
    .slidedown-enter, .slidedown-leave-to {
        transform: translateY(-100%)
    }

    .slidedown-enter-to, .slidedown-leave {
        transform: translateY(0)
    }

    .slidedown-enter-active, .slidedown-leave-active {
        transition: all 750ms ease-in-out
    }
    */

    .slidedown-enter {
        transform: translateY(-100%)
    }
    .slidedown-enter-to {
        transform: translateY(0)
    }

    /*
    .slidedown-leave-to {
        transform: translateY(-100%)
    }
    .slidedown-leave-active {
        transition: all 1750ms ease-in-out
    }
    */
    .slidedown-enter-active {
        transition: all 250ms ease-in-out
    }

</style>

<template>
    <div id="app">

        <div class="tqNavBar">
            <router-link to="/">Auction House View</router-link> |
            <router-link to="/chat">Chat View</router-link>
        </div>

        <transition name="none">
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

        beforeDestroy: function () {
            TQGlobals.connection.disconnect();
        },
        components: {
        }
    });
</script>

