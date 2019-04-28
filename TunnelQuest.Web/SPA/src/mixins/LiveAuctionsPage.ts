
import axios from "axios";
import mixins from 'vue-typed-mixins';
import * as moment from "moment";

import Auction from "../interfaces/Auction";
import ChatLine from "../interfaces/ChatLine";

import LivePage from "../mixins/LivePage";

import TQGlobals from "../classes/TQGlobals";


export default mixins(LivePage).extend({

    data: function () {
        return {
            auctions: new Array<Auction>(),
            auctionsDict: new Map<number, Auction>(),
            recentlyUpdatedAuctions: new Array<Auction>(),
            notRecentlyUpdatedAuctions: new Array<Auction>(),
        };
    },

    beforeDestroy: function () {
        this.auctionsDict.clear();
        this.recentlyUpdatedAuctions = new Array<Auction>();
        this.notRecentlyUpdatedAuctions = new Array<Auction>();
    },

    methods: {

        // inherited from TqPage
        beforeChatLinesLoaded: function (newChatLines: Array<ChatLine>) {
            let auctionsArray = new Array<Auction>();
            for (let chatLine of newChatLines) {
                for (let auctionId in chatLine.auctions) {
                    if (chatLine.auctions[auctionId].passesFilter)
                        auctionsArray.push(chatLine.auctions[auctionId]);
                }
            }

            this.beforeAuctionsLoaded(auctionsArray);
        },

        // inherited from TqPage
        onChatLinesLoaded: function (newChatLines: Array<ChatLine>) {
            // stub
            console.log("LiveAuctionsPage.onChatLinesLoaded():");
            console.log(newChatLines);

            // manually set some properties on the auction objects
            let auctionsArray = new Array<Auction>();
            for (let chatLine of newChatLines) {
                for (let auctionId in chatLine.auctions) {
                    let auction = chatLine.auctions[auctionId];

                    if (auction.passesFilter) {
                        auction.createdAtMoment = moment.utc(auction.createdAtString).local();

                        // if necessary, update the previous auction object
                        if (auction.replacesAuctionId != null) {
                            let prevAuction = this.auctionsDict.get(auction.replacesAuctionId);
                            if (prevAuction) {
                                prevAuction.isReplaced = true;
                                // also copy over the firstSeenDate from the previous auction so that the new auction will 
                                // take the previous auction's place in the sort order, instead of appearing at the top
                                auction.firstSeenDate = prevAuction.firstSeenDate;
                            }
                        }

                        // transfer the firstSeenMoment from the previously existing entry, if it exists
                        let existingEntry = this.auctionsDict.get(auction.id);
                        if (existingEntry)
                            auction.firstSeenDate = existingEntry.firstSeenDate;
                        else
                            auction.firstSeenDate = new Date();

                        auctionsArray.push(auction);
                        this.auctions.push(auction);
                        this.auctionsDict.set(auction.id, auction);
                    }
                }                
            }

            this.auctions.sort(function (a: Auction, b: Auction) {
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
            });

            this.onAuctionsLoaded(auctionsArray);

            this.rebuildAuctionArrays();
        },

        // inherited from TqPage
        onChatLinesUnloaded: function (removedLines: Array<ChatLine>) {
            let unloadedAuctions = new Array<Auction>();

            for (let chatLine of removedLines) {
                for (let auctionId in chatLine.auctions) {
                    if (this.auctionsDict.has(chatLine.auctions[auctionId].id)) {
                        let auction = chatLine.auctions[auctionId];

                        this.auctionsDict.delete(auction.id);
                        this.auctions.splice(this.auctions.indexOf(auction), 1);

                        unloadedAuctions.push(auction);
                    }
                }
            }

            // pass along to extending components
            this.onAuctionsUnloaded(unloadedAuctions);

            this.rebuildAuctionArrays();
        },

        rebuildAuctionArrays: function () {

            // build recentlyUpdatedAuctions
            this.recentlyUpdatedAuctions = this.auctions
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

            // build notRecentlyUpdatedAuctions
            this.notRecentlyUpdatedAuctions = this.auctions
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
        },

        beforeAuctionsLoaded: function (newAuctions: Array<Auction>) {
            // overridden by extending components
        },

        onAuctionsLoaded: function (newAuctions: Array<Auction>) {
            // overridden by extending components
        },

        onAuctionsUnloaded: function (removedAuctions: Array<Auction>) {
            // overridden by extending components
        },
    }
});