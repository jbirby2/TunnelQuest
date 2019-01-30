<style>
    .tqChatPage {
        opacity: 0.7;
        background-color: #000000;
    }

</style>

<template>
    <div>
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
    import Auction from "../interfaces/Auction";
    import LinesAndAuctions from "../interfaces/LinesAndAuctions";

    import LivePage from "../mixins/LivePage";

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
            onInitialized: function () {
                console.log("stub ChatPage.onInitialized");

                this.chatLines.maxSize = TQGlobals.settings.maxChatLines;
            },

            // inherited from LivePage
            getLatestContent: function () {
                let minId: number | null = null;
                if (this.chatLines.array.length > 0)
                    minId = this.chatLines.array[this.chatLines.array.length - 1].id + 1;

                axios.get('/api/chat_lines?serverCode=' + TQGlobals.serverCode + "&minId=" + (minId == null ? "" : minId.toString()))
                    .then(response => {
                        let result = response.data as LinesAndAuctions;
                        this.onNewContent(result, true);
                    })
                    .catch(err => {
                        // stub
                        console.log(err);
                    }); // end axios.get(chat_lines)
            },

            // inherited from LivePage
            getEarlierContent: function () {
                let maxId: number | null = null;
                if (this.chatLines.array.length > 0)
                    maxId = this.chatLines.array[0].id - 1;

                console.log("stub ChatPage.getEarlierContent(maxId=" + maxId + ")");

                axios.get('/api/chat_lines?serverCode=' + TQGlobals.serverCode + "&maxId=" + (maxId == null ? "" : maxId.toString()) + "&maxResults=" + TQGlobals.settings.maxChatLines.toString())
                    .then(response => {
                        let result = response.data as LinesAndAuctions;
                        this.onNewContent(result, false);
                    })
                    .catch(err => {
                        // stub
                        console.log(err);
                    }); // end axios.get(chat_lines)
            },

            // inherited from LivePage
            onNewContent: function (newContent: LinesAndAuctions, enforceMaxSize: boolean) {
                // stub
                console.log("ChatPage.onNewContent():");
                console.log(newContent);

                this.chatLines.add(newContent.lines, enforceMaxSize);
            },

            // inherited from LivePage
            onDestroying: function () {
                this.chatLines.clear();
            },
        },
        components: {
            ChatLineView
        },
    });
</script>
