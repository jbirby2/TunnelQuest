
<style>
    document {
        padding: 0;
        margin: 0;
    }

    body {
        padding: 0;
        margin: auto auto;
        background-image: url("/assets/background.png");
        background-size: cover;
        background-attachment: fixed;
        color: #ffffff;
    }

    .tqMainPanel {
    }

    input[type=text] {
        width: 100%;
        border-style: none;
        padding-left: 3px;
    }

    input[type=text], input[type=tel], select {
        background-color: #f2f2f2;
        border: solid 1px #000;
        color: #000;
    }

    input[type=text]:focus, input[type=tel]:focus, select:focus {
        background-color: #edfeff;
        color: #000000;
    }

    input[type=radio]:checked + label {
        color: #f2a800;
    }

    input[type=button], button {
        background-color: #415d8c;
        border: solid 1px #000;
        color: #efefef;
        margin-left: 4px;
    }

    input[type=button]:active, button:active {
        background-color: #edfeff;
        color: #000000;
    }

    label {
        white-space: nowrap;
    }

    @media screen and (min-width: 992px) {
        /* start of desktop styles */
        body {
            font-size: 10pt;
        }
    }

    @media screen and (max-width: 991px) {
        /* start of large tablet styles */
        body {
            font-size: 9pt;
        }
    }

    @media screen and (max-width: 767px) {
        /* start of medium tablet styles */
        body {
            font-size: 8pt;
        }
    }

    @media screen and (max-width: 479px) {
        /* start of phone styles */
        body {
            font-size: 7.5pt;
        }
    }


    /* fade transition */

    .fade-enter-active {
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
        <div class="tqMainPanel">
            <keep-alive exclude="ItemPage,FilterPage">
                <router-view />
            </keep-alive>
        </div>
    </div>
</template>

<script lang="ts">
    import Vue from "vue";
    
    export default Vue.extend({

        mounted: function () {
        },

        beforeDestroy: function () {
        },

        components: {
        }
    });
</script>

