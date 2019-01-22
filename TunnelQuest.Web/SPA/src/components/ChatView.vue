<style>
</style>

<template>
    <div>
        <div>Chat View:</div>
        <div>
            <transition-group name="none">
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

    import LiveView from "../mixins/LiveView";

    import ChatLineView from "./ChatLineView.vue";

    import TQGlobals from "../classes/TQGlobals";
    import SlidingList from "../classes/SlidingList";

    export default mixins(LiveView).extend({

        data: function () {
            return {
                chatLines: new SlidingList<ChatLine>()
            };
        },

        computed: {
            viewLines: function () {
                return _.clone(this.chatLines.array).reverse();
            }
        },
        methods: {

            // inherited from LiveView
            onInitialized: function () {
                console.log("stub ChatView.onInitialized");

                this.chatLines.maxSize = TQGlobals.settings.maxChatLines;
            },

            // inherited from LiveView
            getLatestContent: function () {
                let minId: number | null = null;
                if (this.chatLines.array.length > 0)
                    minId = this.chatLines.array[this.chatLines.array.length - 1].id + 1;

                axios.get('/api/chat_lines?serverCode=' + TQGlobals.serverCode + "&minId=" + (minId == null ? "" : minId.toString()))
                    .then(response => {
                        let result = response.data as LinesAndAuctions;
                        this.onNewContent(result);
                    })
                    .catch(err => {
                        // stub
                        console.log(err);
                    }); // end axios.get(chat_lines)
            },

            // inherited from LiveView
            getEarlierContent: function () {
                console.log("stub ChatView.getEarlierContent()");

                let maxId: number | null = null;
                if (this.chatLines.array.length > 0)
                    maxId = this.chatLines.array[0].id - 1;

                axios.get('/api/chat_lines?serverCode=' + TQGlobals.serverCode + "&maxId=" + (maxId == null ? "" : maxId.toString()) + "&maxResults=" + TQGlobals.settings.chatLineBackScrollFetchSize.toString())
                    .then(response => {
                        let result = response.data as LinesAndAuctions;
                        this.onNewContent(result);
                    })
                    .catch(err => {
                        // stub
                        console.log(err);
                    }); // end axios.get(chat_lines)
            },

            // inherited from LiveView
            onNewContent: function (newContent: LinesAndAuctions) {
                // stub
                console.log("ChatView.onNewContent():");
                console.log(newContent);

                this.wireUpRelationships(newContent);
                this.chatLines.add(newContent.lines);
            },

            // inherited from LiveView
            onDestroying: function () {
                this.chatLines.clear();
            },
        },
        components: {
            ChatLineView
        },
    });
</script>
