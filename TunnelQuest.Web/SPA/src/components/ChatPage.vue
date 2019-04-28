<style>
    .tqChatPage {
        background-color: rgba(0,0,0,0.7);
    }

    /* override .tqChatLineView */
    .tqChatLineView {
        display: block;
    }

</style>

<template>
    <div>
        <site-header>
            <connection-status-view :connection="connection"></connection-status-view>
        </site-header>

        <filter-manager-view></filter-manager-view>

        <div class="tqChatPage">
            <transition-group :name="transitionName">
                <chat-line-view v-for="chatLine in viewLines" :key="chatLine.id" :chatLine="chatLine" :showTimestamp="true"></chat-line-view>
            </transition-group>
        </div>
    </div>
</template>

<script lang="ts">
    import mixins from 'vue-typed-mixins';
    import * as _ from "lodash";

    import ChatLine from "../interfaces/ChatLine";
    
    import LivePage from "../mixins/LivePage";

    import SiteHeader from "./SiteHeader.vue";
    import ConnectionStatusView from "./ConnectionStatusView.vue";
    import FilterManagerView from "./FilterManagerView.vue";
    import ChatLineView from "./ChatLineView.vue";


    export default mixins(LivePage).extend({

        name: "ChatPage",

        data: function () {
            return {
                viewLines: new Array<ChatLine>()
            };
        },

        methods: {
            // inherited from TqPage
            beforeChatLinesLoaded: function (newLines: Array<ChatLine>) {
            },

            // inherited from TqPage
            onChatLinesLoaded: function (newLines: Array<ChatLine>) {
                this.refreshViewLines();
            },

            // inherited from TqPage
            onChatLinesUnloaded: function (newLines: Array<ChatLine>) {
                this.refreshViewLines();
            },

            refreshViewLines: function () {
                this.viewLines = _.clone(this.chatLines).reverse();
            }
        },

        components: {
            SiteHeader,
            FilterManagerView,
            ConnectionStatusView,
            ChatLineView
        },
    });
</script>
