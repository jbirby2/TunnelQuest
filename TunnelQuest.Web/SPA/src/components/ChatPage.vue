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
                <chat-line-view v-for="chatLine in viewLines" :key="chatLine.id" :chatLine="chatLine" :showTimestamp="true" :itemNameLinks="true"></chat-line-view>
            </transition-group>
        </div>
    </div>
</template>

<script lang="ts">
    import axios from "axios";
    import mixins from 'vue-typed-mixins';
    import * as _ from "lodash";

    import ChatLine from "../interfaces/ChatLine";
    import LinesAndAuctions from "../interfaces/LinesAndAuctions";

    import LivePage from "../mixins/LivePage";

    import SiteHeader from "./SiteHeader.vue";
    import ConnectionStatusView from "./ConnectionStatusView.vue";
    import FilterManagerView from "./FilterManagerView.vue";
    import ChatLineView from "./ChatLineView.vue";

    import TQGlobals from "../classes/TQGlobals";
    import SlidingList from "../classes/SlidingList";

    export default mixins(LivePage).extend({

        name: "ChatPage",

        data: function () {
            return {
                chatLines: new SlidingList<ChatLine>(function (a: ChatLine, b: ChatLine) {
                    // sort ascending
                    if (a.id < b.id)
                        return -1;
                    else if (a.id > b.id)
                        return 1;
                    else
                        return 0;
                })
            };
        },

        computed: {
            viewLines: function () {
                return _.clone(this.chatLines.array).reverse();
            }
        },

        methods: {

            // inherited from LivePage
            getHubUrl: function () {
                // STUB hard-coded
                return "/blue_chat_hub";
            },

            // inherited from LivePage
            onInitialized: function () {
                console.log("stub ChatPage.onInitialized");

                this.chatLines.maxSize = TQGlobals.settings.maxChatLines;
            },

            // inherited from TqPage
            getLatestContent: function () {
                let minId: number | null = null;
                if (this.chatLines.array.length > 0)
                    minId = this.chatLines.array[this.chatLines.array.length - 1].id + 1;

                axios.post('/api/chat_query', {
                    serverCode: TQGlobals.serverCode,
                    minimumId: minId
                })
                .then(response => {
                    let result = response.data as LinesAndAuctions;
                    this.onNewContent(result, true);
                })
                .catch(err => {
                    // stub
                    console.log(err);
                });
            },

            // inherited from TqPage
            getEarlierContent: function () {
                let maxId: number | null = null;
                if (this.chatLines.array.length > 0)
                    maxId = this.chatLines.array[0].id - 1;

                axios.post('/api/chat_query', {
                    serverCode: TQGlobals.serverCode,
                    maximumId: maxId
                })
                .then(response => {
                    let result = response.data as LinesAndAuctions;
                    this.onNewContent(result, false);
                })
                .catch(err => {
                    // stub
                    console.log(err);
                });
            },

            // inherited from TqPage
            onFilteredContent: function (newContent: LinesAndAuctions, enforceMaxSize: boolean) {
                // stub
                console.log("ChatPage.onNewContent():");
                console.log(newContent);

                for (let chatLineId in newContent.lines) {
                    let chatLine = newContent.lines[chatLineId];
                    this.chatLines.add(chatLine);
                }

                if (enforceMaxSize)
                    this.chatLines.enforceMaxSize();
                this.chatLines.sort();
            },

            // inherited from LivePage
            onDestroying: function () {
                this.chatLines.clear();
            },
        },

        components: {
            SiteHeader,
            FilterManagerView,
            ConnectionStatusView,
            ChatLineView
        },
    });
</script>
