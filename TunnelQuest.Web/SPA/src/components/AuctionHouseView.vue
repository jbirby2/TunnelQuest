
<style>
</style>

<template>
    <div>
        <div>
            <transition-group :name="transitionName">
                <auction-view v-for="auction in viewAuctions" :key="auction.id" :auction="auction"></auction-view>
            </transition-group>
        </div>
    </div>
</template>

<script lang="ts">
    import axios from "axios";
    import mixins from 'vue-typed-mixins';
    import * as _ from "lodash";

    import Auction from "../interfaces/Auction";
    import LinesAndAuctions from "../interfaces/LinesAndAuctions";

    import LiveView from "../mixins/LiveView";

    import TQGlobals from "../classes/TQGlobals";
    import SlidingList from "../classes/SlidingList";

    import AuctionView from "./AuctionView.vue";


    export default mixins(LiveView).extend({

        data: function () {
            return {
                auctions: new SlidingList<Auction>(function (a: Auction, b: Auction) {
                    if (a.updatedAtString < b.updatedAtString)
                        return -1;
                    else if (a.updatedAtString > b.updatedAtString)
                        return 1;
                    else {
                        if (a.id < b.id)
                            return -1;
                        else if (a.id > b.id)
                            return 1;
                        else
                            return 0;
                    }
                })
            };
        },

        computed: {
            viewAuctions: function () {
                return _.clone(this.auctions.array).reverse();
            }
        },
        methods: {

            // inherited from LiveView
            onInitialized: function () {
                console.log("stub AuctionHouseView.onInitialized");

                this.auctions.maxSize = TQGlobals.settings.maxAuctions;
            },

            // inherited from LiveView
            getLatestContent: function () {
                let minUpdatedAt: Date | null = null;
                if (this.auctions.array.length > 0) {
                    minUpdatedAt = new Date(this.auctions.array[this.auctions.array.length - 1].updatedAtString);

                    // add 1 ms so we don't always get one auction that we already know about in the results
                    minUpdatedAt = new Date(minUpdatedAt.getTime() + 1);
                }

                axios.get('/api/auctions?serverCode=' + TQGlobals.serverCode + "&minUpdatedAt=" + (minUpdatedAt == null ? "" : minUpdatedAt.toISOString()))
                    .then(response => {
                        let result = response.data as LinesAndAuctions;
                        this.onNewContent(result, true);
                    })
                    .catch(err => {
                        // stub
                        console.log(err);
                    }); // end axios.get(chat_lines)
            },

            // inherited from LiveView
            getEarlierContent: function () {

                let maxUpdatedAt: Date | null = null;
                if (this.auctions.array.length > 0) {
                    maxUpdatedAt = new Date(this.auctions.array[0].updatedAtString);

                    // subtract 1 ms so we don't always get one auction that we already know about in the results
                    maxUpdatedAt = new Date(maxUpdatedAt.getTime() - 1);
                }

                axios.get('/api/auctions?serverCode=' + TQGlobals.serverCode + "&maxUpdatedAt=" + (maxUpdatedAt == null ? "" : maxUpdatedAt.toISOString()) + "&maxResults=" + TQGlobals.settings.maxAuctions.toString())
                    .then(response => {
                        let result = response.data as LinesAndAuctions;
                        this.onNewContent(result, false);
                    })
                    .catch(err => {
                        // stub
                        console.log(err);
                    }); // end axios.get(chat_lines)
            },

            // inherited from LiveView
            onNewContent: function (newContent: LinesAndAuctions, enforceMaxSize: boolean) {
                // stub
                console.log("AuctionHouseView.onNewContent():");
                console.log(newContent);

                // manually wire up auction.chatLine references
                for (let auctionId in newContent.auctions) {
                    let auction = newContent.auctions[auctionId];
                    auction.chatLine = newContent.lines[auction.chatLineId];
                }

                this.auctions.add(newContent.auctions, enforceMaxSize);
            },

            // inherited from LiveView
            onDestroying: function () {
                this.auctions.clear();
            }
        },
        components: {
            AuctionView
        },
    });
</script>
