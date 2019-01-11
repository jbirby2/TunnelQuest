<template>
    <div>
        <chat-view :settings="settings" :chatLines="chatLines"></chat-view>
    </div>
</template>

<script lang="ts">
    import Vue from "vue";
    import * as signalR from "@aspnet/signalr";

    import Settings from "./interfaces/Settings";
    import ChatLine from "./interfaces/ChatLine";
    import Auction from "./interfaces/Auction";

    import ChatView from "./components/ChatView.vue";


    export default Vue.extend({
        data: function () {
            return {
                hubUrl: null as string | null,
                settings: null as Settings | null,
                chatLines: new Array<ChatLine>(),
                auctions: new Array<Auction>()
            };
        },
        created: function () {
            
        },
        mounted: function () {

            if (this.hubUrl != null) {
                const connection = new signalR.HubConnectionBuilder()
                    .withUrl(this.hubUrl)
                    .build();

                connection.on("HandleNewLines", (lines: Array<ChatLine>) => {
                    // only process new lines after we've gotten the settings from the server
                    if (this.settings != null) {
                        // stub
                        console.log("new lines: " + JSON.stringify(lines));

                        for (let line of lines) {
                            //if (!this.chatLines[line.id]) {
                                //this.chatLines[line.id] = line;
                                this.chatLines.push(line);
                            //}
                                

                            for (let auctionIdString in line.auctions) {
                                let auctionId = parseInt(auctionIdString);

                                if (!this.auctions[auctionId])
                                    this.auctions[auctionId] = line.auctions[auctionIdString];

                                // STUB create items collection and update that too
                            }
                        }
                    }
                });

                connection
                    .start()
                    .then(() => {
                        connection.invoke("getSettings")
                            .then((serverSettings: Settings) => {
                                this.settings = serverSettings;
                            })
                            .catch((err: any) => {
                                // stub
                                console.log(err)
                            });
                    })
                    .catch(err => {
                        // stub
                        console.log(err)
                    });
            }
            
        },
        components: {
            ChatView
        }
    });
</script>

<style>
</style>