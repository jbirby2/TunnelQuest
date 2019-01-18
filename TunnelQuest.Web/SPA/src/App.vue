<template id="main-page">
    <v-ons-page>
        <v-ons-toolbar>
            <div class="center" v-if="isScrolledToTop_">TunnelQuest</div>
            <div class="center" v-if="!isScrolledToTop_">TunnelQuest (scroll up for new auctions)</div>
        </v-ons-toolbar>

        <div class="content" @scroll="onScroll">
            <chat-view ref="chatView" :settings="settings" :chatLines="chatLines" :auctions="auctions"></chat-view>
            <auction-list-view :auctions="auctions"></auction-list-view>
        </div>

    </v-ons-page>
</template>

<script lang="ts">
    import Vue from "vue";
    //import * as onsenui from "onsenui";
    import axios from "axios";
    import * as signalR from "@aspnet/signalr";

    import Settings from "./interfaces/Settings";
    import ChatLine from "./interfaces/ChatLine";
    import Auction from "./interfaces/Auction";
    import LinesAndAuctions from "./interfaces/LinesAndAuctions";

    import SlidingList from "./classes/SlidingList";

    import ChatView from "./components/ChatView.vue";
    import AuctionListView from "./components/AuctionListView.vue";


    export default Vue.extend({
        data: function () {
            return {
                serverCode: "",
                hubUrl: "",
                settings: null as Settings | null,

                chatLines: new SlidingList<ChatLine>(),
                auctions: new SlidingList<Auction>(),

                // "private"
                connection_: null as signalR.HubConnection | null,
                isScrolledToTop_: true,
                isScrolledToBottom_: false,
                contentDiv_: null as HTMLElement | null
            };
        },
        created: function () {

        },
        mounted: function () {
            this.contentDiv_ = this.$el.querySelector(".content") as HTMLElement;

            axios.get('/api/settings')
                .then(response => {
                    this.settings = response.data as Settings;
                    this.chatLines.maxSize = this.settings.maxChatLines;
                    this.auctions.maxSize = this.settings.maxAuctions;

                    window.onscroll = this.onScroll;

                    this.connection_ = new signalR.HubConnectionBuilder()
                        .withUrl(this.hubUrl)
                        .build();

                    // wire up signalr events before connecting
                    this.connection_.on("HandleNewChatLines", (newLines: LinesAndAuctions) => {
                        this.handleNewChatLines(newLines);
                    });

                    // open the signalr connection
                    this.reconnectAndCatchUp();
                })
                .catch(err => {
                    // stub
                    console.log(err);
                }); // end axios.get(settings)
        }, // end mounted
        beforeDestroy: function () {
            if (this.connection_ != null) {
                this.connection_.stop();
                this.connection_ = null;
            }
        },
        methods: {

            onScroll: function () {
                if (this.contentDiv_ == null)
                    return;

                let wasAtTop = this.isScrolledToTop_;
                let wasAtBottom = this.isScrolledToBottom_;

                this.isScrolledToTop_ = (this.contentDiv_.scrollTop == 0);
                this.isScrolledToBottom_ = (this.contentDiv_.scrollTop == this.contentDiv_.scrollHeight - this.contentDiv_.offsetHeight);

                if (this.connection_ != null) {
                    if (wasAtTop) {
                        // disconnect from signalr and stop receiving new lines if we leave the top of the list
                        this.connection_.stop();
                        this.$emit("disconnected");
                    }
                    else if (this.isScrolledToTop_) {
                        // reconnect to signalr and also request the next set of lines after the last one we received
                        this.reconnectAndCatchUp();
                        this.$emit("connecting");
                    }
                }

                this.$emit("scroll");

                if (this.isScrolledToBottom_)
                    this.$emit("hit-bottom");
            },

            reconnectAndCatchUp: function () {
                (this.connection_ as signalR.HubConnection)
                    .start()
                    .then(() => {
                        // Don't make the call to get the recent chat lines until AFTER we've connected to signalr.
                        // This ensures that we don't miss any lines, but it also means there's a slight possibility
                        // that we'll get a few duplicate lines.  That's ok however, because SlidingList will handle duplicates.
                        let minId: number | null = null;
                        if (this.chatLines.array.length > 0)
                            minId = this.chatLines.array[this.chatLines.array.length - 1].id + 1;
                        this.getNewChatLines(minId, null);
                    })
                    .catch(err => {
                        // stub
                        console.log(err)
                    }); // end connection.start()
            },
            getNewChatLines: function (minId: number | null = null, maxId: number | null = null) {
                axios.get('/api/chat_lines?serverCode=' + this.serverCode + "&minId=" + (minId == null ? "" : minId.toString()) + "&maxId=" + (maxId == null ? "" : maxId.toString()))
                    .then(response => {
                        let result = response.data as LinesAndAuctions;
                        this.handleNewChatLines(result);
                    })
                    .catch(err => {
                        // stub
                        console.log(err);
                    }); // end axios.get(chat_lines)
            },

            handleNewChatLines: function (newLines: LinesAndAuctions) {
                // stub
                console.log("app.handleNewChatLines():");
                console.log(newLines);


                this.auctions.addToEnd(newLines.auctions);
                this.chatLines.addToEnd(newLines.lines);

                // stub
                //this.chatLines.consoleDump("chatLines");
                //this.auctions.consoleDump("auctions");
            }
        }, // end methods
        components: {
            ChatView,
            AuctionListView
        }
    });
</script>

<style>
</style>