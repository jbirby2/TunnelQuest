<template>
    <div>
        {{chatLine.playerName}} auctions, '{{chatLine.text}}'
    </div>
</template>

<script lang="ts">
    import Vue from "vue";
    import ChatLine from "../interfaces/ChatLine";

    export default Vue.extend({
        props: {
            chatLine: {
                type: Object as () => ChatLine,
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
        }
    });
</script>

<style>
</style>