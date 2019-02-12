<style>
</style>

<template>
    <span>
        <span v-if="$route.fullPath == '/'">
            Welcome to TunnelQuest
        </span>
        <span v-else-if="lastLivePageInfo == null || lastLivePageInfo.fullPath == $route.fullPath">
            <router-link to="/">&lt;&lt; to Main Menu</router-link>
        </span>
        <span v-else>
            <router-link :to="lastLivePageInfo.fullPath">&lt;&lt; to {{lastLivePageInfo.name}}</router-link>
        </span>
    </span>
</template>

<script lang="ts">
    import Vue from "vue";

    export default Vue.extend({
        data: function () {
            return {
                lastLivePageInfo: null as Object | null
            };
        },

        mounted: function () {
            let lastLivePageString = localStorage.getItem("LastLivePage");
            if (lastLivePageString != null)
                this.lastLivePageInfo = JSON.parse(lastLivePageString);
        }
    });
</script>
