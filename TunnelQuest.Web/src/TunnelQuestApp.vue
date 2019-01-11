<template>
    <div>
        <chat-view :chatLines="chatLines"></chat-view>
    </div>
</template>

<script lang="ts">
    import Vue from "vue";
    import * as signalR from "@aspnet/signalr";
    import ChatLine from "./interfaces/ChatLine";
    import Auction from "./interfaces/Auction";
    import ChatView from "./components/ChatView.vue";

    export default Vue.extend({
        data: function () {
            return {
                hubUrl: "",
                chatLines: new Array<ChatLine>(),
                auctions: new Array<Auction>()
            };
        },
        created: function () {
            
        },
        mounted: function () {
            const connection = new signalR.HubConnectionBuilder()
                .withUrl(this.hubUrl)
                .build();

            connection.start().catch(err => console.log(err)); // stub

            connection.on("HandleNewLines", (lines: Array<ChatLine>) => {
                console.log("new lines: " + JSON.stringify(lines)); // stub

                for (let line of lines) {
                    this.chatLines.push(line);
                }
            });
        },
        components: {
            ChatView
        }
    });
</script>

<style>
    .greeting {
        font-size: 20px;
    }
</style>