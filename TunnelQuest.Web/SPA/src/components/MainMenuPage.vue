

<style>
    .tqMainMenuPage {
    }

    .tqMainMenuPage > a {
        display: block;
        background-color: rgba(0,0,0,0.7);
        color: #ffffff;
        font-weight: bold;
        font-size: 1.5em;
        padding: 5px;
        margin: 10px;
        text-decoration: none;
        
    }

    .tqMainMenuPage > a:hover {
        background-color: #a87500;
    }

</style>


<template>
    <div class="tqMainMenuPage">
        <site-header></site-header>

        <router-link to="/auctions">Auction House Mode</router-link>
        <router-link to="/newspaper">Newspaper Mode</router-link>
        <router-link to="/chat">Chat Mode</router-link>
    </div>
</template>

<script lang="ts">
    import mixins from 'vue-typed-mixins';

    import TqPage from "../mixins/TqPage";

    import SiteHeader from "./SiteHeader.vue";


    export default mixins(TqPage).extend({

        name: "MainMenuPage",

        beforeRouteEnter: function (to: any, from: any, next: any) {

            // If the user goes directly to the main menu page (not by clicking on a route-link),
            // then attempt to automatically send them back to the last LivePage they were looking at,
            // as a convenience.  They can still go back to the main menu by clicking the link in the header.
            if (from == null || from.name == null) {
                let lastLivePageString = localStorage.getItem("LastLivePage");
                if (lastLivePageString != null) {
                    let lastLivePageInfo = JSON.parse(lastLivePageString);
                    next({ path: lastLivePageInfo.fullPath });
                }
                else
                    next();
            }
            else
                next();
        },

        components: {
            SiteHeader
        }
    });
</script>
