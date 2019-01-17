<template>
    <div>
        <div>Chat View:</div>
        <div>
            <chat-line-view v-for="chatLine in viewLines" :key="chatLine.id" :settings="settings" :auctions="auctions" :chatLine="chatLine"></chat-line-view>
        </div>
    </div>
</template>

<script lang="ts">
    import * as _ from "lodash";
    import Vue from "vue";

    import Settings from "../interfaces/Settings";
    import ChatLine from "../interfaces/ChatLine";
    import Auction from "../interfaces/Auction";

    import SlidingList from "../classes/SlidingList";

    import ChatLineView from "./ChatLineView.vue";

    export default Vue.extend({
        props: {
            settings: {
                type: Object as () => Settings,
                required: true
            },
            auctions: {
                type: Object as () => SlidingList<Auction>,
                required: true
            },
            chatLines: {
                type: Object as () => SlidingList<ChatLine>,
                required: true
            }
        },
        data: function () {
            return {
            };
        },
        computed: {
            viewLines: function () {
                return _.clone(this.chatLines.array).reverse();
            }
        },
        created: function () {
        },
        mounted: function () {
            this.$parent.$on("hit-bottom", this.onHitBottom);
        },
        methods: {
            onHitBottom: function () {
                console.log("stub ChatView.onHitBottom()");
            }
        },
        components: {
            ChatLineView
        }
    });
</script>

<style>
</style>