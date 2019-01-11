<template>
    <div>
        <div>Chat View:</div>
        <chat-line-view v-for="chatLine in chatLines" :key="chatLine.Id" :settings="settings" :chatLine="chatLine"></chat-line-view>
    </div>
</template>

<script lang="ts">
    import Vue from "vue";
    import Settings from "../interfaces/Settings";
    //import ChatLine from "../interfaces/ChatLine";

    import ChatLineView from "./ChatLineView.vue";

    export default Vue.extend({
        props: {
            settings: {
                type: Object as () => Settings,
                required: true
            },
            chatLines: {
                type: Array,
                required: true
            }
        },
        data: function () {
            return {
            };
        },
        created: function () {
        },
        mounted: function () {
        },
        components: {
            ChatLineView
        }
    });
</script>

<style>
</style>