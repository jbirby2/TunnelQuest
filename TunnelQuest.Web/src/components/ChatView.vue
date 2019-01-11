<template>
<div>
    <div v-for="chatLine in chatLines" :key="chatLine.Id">
        <chat-line-view :chatLine="chatLine"></chat-line-view>
    </div>
</div>
</template>

<script lang="ts">
    import Vue from "vue";
    import ChatLineView from "./ChatLineView.vue";

    export default Vue.extend({
        props: {
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