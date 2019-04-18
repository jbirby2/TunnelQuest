
import Vue from "vue";

import ChatLinePayload from "../interfaces/ChatLinePayload";

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

        onNewContent: function (newContent: ChatLinePayload, enforceMaxSize: boolean) {

            if (newContent.lines) {
                for (let chatLineId in newContent.lines) {
                    let chatLine = newContent.lines[chatLineId];

                    for (let auctionId in chatLine.auctions) {
                        let auction = chatLine.auctions[auctionId];

                        // give each auction a reference back to its own chat line
                        auction.chatLine = chatLine;

                        // for convenience, so we can always just use aliasText throughout the SPA instead of constantly checking if it's null
                        if (auction.aliasText == null)
                            auction.aliasText = auction.itemName;

                        TQGlobals.priceHistories.get(auction.itemName, false);
                    }
                }
            }

            // fetch all price histories at once
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

        onFilteredContent: function (newContent: ChatLinePayload, enforceMaxSize: boolean) {
            // overridden by extending components
        },
    }
});
