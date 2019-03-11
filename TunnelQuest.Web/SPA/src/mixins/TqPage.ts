
import Vue from "vue";

import LinesAndAuctions from "../interfaces/LinesAndAuctions";

import TQGlobals from "../classes/TQGlobals";


// TqPage is the base page from which every page inherits

export default Vue.extend({

    data: function () {
        return {
        };
    },

    beforeMount: function () {
        //stub
        //console.log("TqPage.beforeMount");
    },

    mounted: function () {
        //stub
        //console.log("TqPage.mounted");
    },

    activated: function () {
        //stub
        //console.log("TqPage.activated");
    },

    deactivated: function () {
        //stub
        //console.log("TqPage.deactivated");
    },

    beforeDestroy: function () {
        //stub
        //console.log("TqPage.beforeDestroy");
    },

    methods: {

        onNewContent: function (newContent: LinesAndAuctions, enforceMaxSize: boolean) {

            // fetch all price histories at once

            if (newContent.lines) {
                for (let chatLineId in newContent.lines) {
                    let chatLine = newContent.lines[chatLineId];

                    for (let token of chatLine.tokens) {
                        if (token.type == "ITEM") {
                            TQGlobals.priceHistories.get(token.properties["itemName"], false);
                        }
                    }
                }
            }
            if (newContent.auctions) {
                for (let auctionId in newContent.auctions) {
                    let auction = newContent.auctions[auctionId];
                    auction.chatLine = newContent.lines[auction.chatLineId];
                    TQGlobals.priceHistories.get(auction.itemName, false);
                }
            }
            TQGlobals.priceHistories.fetchPendingPriceHistories();


            // STUB - filtering logic goes here


            // pass along to child classes
            this.onFilteredContent(newContent, enforceMaxSize);
        },

        onScroll: function () {
            //console.log("stub onScroll");

            if (document == null || document.documentElement == null)
                return;

            // stub
            //console.log("scrollTop " + document.documentElement.scrollTop.toString() + " innerHeight " + window.innerHeight.toString() + " scrollheight " + document.documentElement.scrollHeight.toString());

            if (this.isScrolledToBottom())
                this.getEarlierContent();

            this.onScrolled();
        },

        isScrolledToTop: function () {
            return (document != null && document.documentElement != null && document.documentElement.scrollTop == 0);
        },

        isScrolledToBottom: function () {
            return (document != null && document.documentElement != null && Math.ceil(document.documentElement.scrollTop) + window.innerHeight >= document.documentElement.scrollHeight);
        },

        onScrolled: function () {
            // overridden by extending components
        },

        getLatestContent: function () {
            // overridden by extending components
        },

        getEarlierContent: function () {
            // overridden by extending components
        },

        onFilteredContent: function (newContent: LinesAndAuctions, enforceMaxSize: boolean) {
            // overridden by extending components
        },
    }
});
