
<style>
    .tqNewspaperPageListDivider {
        background-color: #55a2c6;
        color: #ffffff;
        font-weight: bold;
        margin-bottom: 10px;
        text-align: center;
        font-size: 1.2em;
    }

    .tqNewspaperPageList > span {
        width: 100%;
        height: 100%;
        display: flex;
        flex-direction: row;
        flex-wrap: wrap;
        justify-content: center;
    }

</style>

<template>
    <div>
        <div class="tqNewspaperPageListDivider">Recent auctions:</div>
        <div class="tqNewspaperPageList">
            <transition-group :name="transitionName">
                <newspaper-auction-view v-for="auction in recentlyUpdatedAuctions" :key="auction.id" :auction="auction"></newspaper-auction-view>
            </transition-group>
        </div>
        <div class="tqNewspaperPageListDivider">Older auctions:</div>
        <div class="tqNewspaperPageList">
            <transition-group :name="transitionName">
                <newspaper-auction-view v-for="auction in notRecentlyUpdatedAuctions" :key="auction.id" :auction="auction"></newspaper-auction-view>
            </transition-group>
        </div>
    </div>
</template>

<script lang="ts">
    import axios from "axios";
    import mixins from 'vue-typed-mixins';
    import * as _ from "lodash";
    import * as moment from "moment";

    import Auction from "../interfaces/Auction";
    import LinesAndAuctions from "../interfaces/LinesAndAuctions";

    import LivePage from "../mixins/LivePage";

    import TQGlobals from "../classes/TQGlobals";
    import SlidingList from "../classes/SlidingList";

    import NewspaperAuctionView from "./NewspaperAuctionView.vue";


    export default mixins(LivePage).extend({

        name: "NewspaperPage",

        data: function () {
            return {
                auctions: new SlidingList<Auction>(function (a: Auction, b: Auction) {
                    // sort ascending updatedAtString
                    if (a.updatedAtString < b.updatedAtString)
                        return -1;
                    else if (a.updatedAtString > b.updatedAtString)
                        return 1;
                    else {
                        // sort ascending id
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
            recentlyUpdatedAuctions: function () {
                return this.auctions.array
                    .filter(function (auction: Auction, index: number) {
                        return moment.duration(moment.default().diff(auction.updatedAtMoment)).asMinutes() <= 15;
                    })
                    .sort(function (a: Auction, b: Auction) {
                        // sort descending firstSeenDate
                        if (a.firstSeenDate < b.firstSeenDate)
                            return 1;
                        else if (a.firstSeenDate > b.firstSeenDate)
                            return -1;
                        else {
                            // sort descending id
                            if (a.id < b.id)
                                return 1;
                            else if (a.id > b.id)
                                return -1;
                            else
                                return 0;
                        }
                    });
            },

            notRecentlyUpdatedAuctions: function () {
                return this.auctions.array
                    .filter(function (auction: Auction, index: number) {
                        return moment.duration(moment.default().diff(auction.updatedAtMoment)).asMinutes() > 15;
                    })
                    .sort(function (a: Auction, b: Auction) {
                        // sort descending updatedAtString
                        if (a.updatedAtString < b.updatedAtString)
                            return 1;
                        else if (a.updatedAtString > b.updatedAtString)
                            return -1;
                        else {
                            // sort descending id
                            if (a.id < b.id)
                                return 1;
                            else if (a.id > b.id)
                                return -1;
                            else
                                return 0;
                        }
                    });
            }
        },
        methods: {

            // inherited from LivePage
            onInitialized: function () {
                console.log("stub NewspaperPage.onInitialized");

                this.auctions.maxSize = TQGlobals.settings.maxAuctions;
            },

            // inherited from LivePage
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

            // inherited from LivePage
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

            // inherited from LivePage
            onNewContent: function (newContent: LinesAndAuctions, enforceMaxSize: boolean) {
                // stub
                console.log("NewspaperPage.onNewContent():");
                console.log(newContent);

                // manually set some properties on the auction objects

                for (let auctionId in newContent.auctions) {
                    let auction = newContent.auctions[auctionId];
                    auction.chatLine = newContent.lines[auction.chatLineId];
                    auction.updatedAtMoment = moment.utc(auction.updatedAtString).local();
                    auction.item = TQGlobals.items.get(auction.itemName, false); // false to prevent it from immediately making an ajax call to fetch this one item

                    // transfer the firstSeenMoment from the previously existing entry, if it exists
                    let existingEntry = this.auctions.dict[auction.id];
                    if (existingEntry)
                        auction.firstSeenDate = existingEntry.firstSeenDate;
                    else
                        auction.firstSeenDate = new Date();
                }

                // call this after the loop to fetch all items and simultaneously (because it's far more efficient than making an ajax call for every individual item)
                TQGlobals.items.fetchPendingItems();

                this.auctions.add(newContent.auctions, enforceMaxSize);
            },

            // inherited from LivePage
            onDestroying: function () {
                this.auctions.clear();
            }
        },
        components: {
            NewspaperAuctionView
        },
    });
</script>
