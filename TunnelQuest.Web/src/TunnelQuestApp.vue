<template>
    <div>
        <chat-view :settings="settings" :chatLines="chatLines" :auctions="auctions"></chat-view>
    </div>
</template>

<script lang="ts">
    import Vue from "vue";
    import axios from "axios";
    import * as signalR from "@aspnet/signalr";

    import Settings from "./interfaces/Settings";
    import ChatLine from "./interfaces/ChatLine";
    import Auction from "./interfaces/Auction";
    import GetChatLinesResult from "./interfaces/GetChatLinesResult";

    import SlidingList from "./classes/SlidingList";

    import ChatView from "./components/ChatView.vue";


    export default Vue.extend({
        data: function () {
            return {
                serverCode: "",
                hubUrl: "",
                settings: null as Settings | null,

                chatLines: new SlidingList<ChatLine>(),
                auctions: new SlidingList<Auction>(),

                // "private"
                connection_: null as signalR.HubConnection | null
            };
        },
        created: function () {
            
        },
        mounted: function () {

            axios.get('/api/settings')
                .then(response => {
                    this.settings = response.data as Settings;
                    this.chatLines.maxSize = this.settings.maxChatLines;
                    this.auctions.maxSize = this.settings.maxAuctions;

                    this.connection_ = new signalR.HubConnectionBuilder()
                        .withUrl(this.hubUrl)
                        .build();

                    // wire up signalr events before connecting

                    this.connection_.on("ProcessNewLines", (result: GetChatLinesResult) => {
                        this.processServerResult(result);
                    });

                    // open the signalr connection
                    this.connection_
                        .start()
                        .then(() => {
                            // Don't make the call to get the recent chat lines until AFTER we've connected to signalr.
                            // This ensures that we don't miss any lines, but it also means there's a slight possibility
                            // that we'll get a few duplicate lines.  That's ok however, because chatLines.addToEnd() will
                            // ignore duplicates.
                            axios.get('/api/chat_lines?serverCode=' + this.serverCode)
                                .then(response => {
                                    let result = response.data as GetChatLinesResult;
                                    this.processServerResult(result);
                                })
                                .catch (err => {
                                    // stub
                                    console.log(err);
                                }); // end axios.get(chat_lines)
                        })
                        .catch(err => {
                            // stub
                            console.log(err)
                        }); // end connection.start()
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
            processServerResult: function (result: GetChatLinesResult) {
                console.log("from server:");
                console.log(result);

                this.auctions.addToEnd(result.auctions);
                this.chatLines.addToEnd(result.lines);

                // stub
                //this.chatLines.consoleDump("chatLines");
                //this.auctions.consoleDump("auctions");
            }
        }, // end methods
        components: {
            ChatView
        }
    });
</script>

<style>
</style>