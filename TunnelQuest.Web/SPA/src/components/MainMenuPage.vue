

<style>
    .tqMainMenuPage {
    }

    .tqMainMenuPage > a {
        display: block;
        background-color: orange;
        font-weight: bold;
        font-size: 1.5em;
        padding: 5px;
        margin: 10px;
    }
</style>


<template>
    <div class="tqMainMenuPage">
        <router-link to="/auctions">Auctions</router-link>
        <router-link to="/newspaper">Newspaper</router-link>
        <router-link to="/chat">Chat</router-link>
    </div>
</template>

<script lang="ts">
    import Vue from "vue";

    export default Vue.extend({

        name: "MainMenuPage",

        components: {
        },
    });
</script>
