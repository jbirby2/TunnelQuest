
import axios from "axios";
import mixins from 'vue-typed-mixins';
import * as moment from "moment";

import Auction from "../interfaces/Auction";
import ChatLinePayload from "../interfaces/ChatLinePayload";

import LivePage from "../mixins/LivePage";

import TQGlobals from "../classes/TQGlobals";
import SlidingList from "../classes/SlidingList";


export default mixins(LivePage).extend({

    data: function () {
        return {
            auctions: new SlidingList<Auction>(function (a: Auction, b: Auction) {
                // sort ascending createdAtString
                if (a.createdAtString < b.createdAtString)
                    return -1;
                else if (a.createdAtString > b.createdAtString)
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
                    return !auction.isReplaced;
                })
                .filter(function (auction: Auction, index: number) {
                    return moment.duration(moment.default().diff(auction.createdAtMoment)).asMinutes() <= 15;
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
                    return moment.duration(moment.default().diff(auction.createdAtMoment)).asMinutes() > 15;
                })
                .sort(function (a: Auction, b: Auction) {
                    // sort descending createdAtString
                    if (a.createdAtString < b.createdAtString)
                        return 1;
                    else if (a.createdAtString > b.createdAtString)
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
            return "/blue_chat_hub";
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
                minId = this.auctions.array[this.auctions.array.length - 1].chatLine.id + 1;

            axios.post('/api/chat_query', {
                serverCode: TQGlobals.serverCode,
                minimumId: minId
            })
            .then(response => {
                let result = response.data as ChatLinePayload;
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
                maxId = this.auctions.array[0].chatLine.id - 1;

            axios.post('/api/chat_query', {
                serverCode: TQGlobals.serverCode,
                maximumId: maxId
            })
            .then(response => {
                let result = response.data as ChatLinePayload;
                this.onNewContent(result, false);
            })
            .catch(err => {
                // stub
                console.log(err);
            });
        },

        // inherited from TqPage
        onFilteredContent: function (chatLines: ChatLinePayload, enforceMaxSize: boolean) {
            // stub
            console.log("LiveAuctionsPage.onFilteredContent():");
            console.log(chatLines);

            // manually set some properties on the auction objects

            for (let chatLineId in chatLines.lines) {
                let chatLine = chatLines.lines[chatLineId];

                for (let auctionId in chatLine.auctions) {
                    let auction = chatLine.auctions[auctionId];

                    auction.createdAtMoment = moment.utc(auction.createdAtString).local();

                    // if necessary, update the previous auction object
                    if (auction.replacesAuctionId != null) {
                        let prevAuction = this.auctions.dict.get(auction.replacesAuctionId);
                        if (prevAuction) {
                            prevAuction.isReplaced = true;
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