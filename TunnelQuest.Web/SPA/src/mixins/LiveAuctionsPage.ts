
import axios from "axios";
import mixins from 'vue-typed-mixins';
import * as _ from "lodash";
import * as moment from "moment";

import Auction from "../interfaces/Auction";
import LinesAndAuctions from "../interfaces/LinesAndAuctions";

import LivePage from "../mixins/LivePage";

import TQGlobals from "../classes/TQGlobals";
import SlidingList from "../classes/SlidingList";


export default mixins(LivePage).extend({

    data: function () {
        return {
            // STUB hard-coded
            serverCode: "BLUE",

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
                    // avoid temporarily showing "duplicate" auctions when a new auction is created to replace an older auction
                    // by immediately hiding the older auction in the recentlyUpdatedAuctions panel
                    return !auction.isPreviousAuction;
                })
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
        getHubUrl: function () {
            // STUB hard-coded
            return "/blue_auction_hub";
        },

        // inherited from LivePage
        onInitialized: function () {
            console.log("stub LiveAuctionsPage.onInitialized");

            this.auctions.maxSize = TQGlobals.settings.maxAuctions;
        },

        // inherited from TqPage
        getLatestContent: function () {
            let minId: number | null = null;
            if (this.auctions.array.length > 0)
                minId = this.auctions.array[this.auctions.array.length - 1].id + 1;

            axios.post('/api/auction_query', {
                serverCode: this.serverCode,
                minimumId: minId,
                includeChatLine: true
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
            if (this.auctions.array.length > 0)
                maxId = this.auctions.array[0].id - 1;

            axios.post('/api/auction_query', {
                serverCode: this.serverCode,
                maximumId: maxId,
                includeChatLine: true
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
            console.log("LiveAuctionsPage.onNewContent():");
            console.log(newContent);

            // manually set some properties on the auction objects

            for (let auctionId in newContent.auctions) {
                let auction = newContent.auctions[auctionId];
                auction.updatedAtMoment = moment.utc(auction.updatedAtString).local();

                // if necessary, update the previous auction object
                if (auction.previousAuctionId != null) {
                    let prevAuction = this.auctions.dict.get(auction.previousAuctionId);
                    if (prevAuction) {
                        prevAuction.isPreviousAuction = true;
                        // also copy over the firstSeenDate from the previous auction so that the new auction will 
                        // take the previous auction's place in the sort order, instead of appearing at the top
                        auction.firstSeenDate = prevAuction.firstSeenDate;
                    }
                }
                
                // transfer the firstSeenMoment from the previously existing entry, if it exists
                let existingEntry = this.auctions.dict.get(auction.id);
                if (existingEntry)
                    auction.firstSeenDate = existingEntry.firstSeenDate;
                else
                    auction.firstSeenDate = new Date();

                this.onNewAuction(auction); // this has to happen FIRST so that auction.item is set BEFORE the auction is added to the SlidingList, or else Vue won't detect when auction.item's properties are filled in by ajax later
                this.auctions.add(auction);
            }

            if (enforceMaxSize)
                this.auctions.enforceMaxSize();
            this.auctions.sort();

            // call this last to fetch all items and simultaneously (because it's far more efficient than making an ajax call for every individual item)
            TQGlobals.items.fetchPendingItems();
        },

        // inherited from LivePage
        onDestroying: function () {
            this.auctions.clear();
        },


        onNewAuction: function (auction: Auction) {
            // overridden by extending components
        },
    }
});