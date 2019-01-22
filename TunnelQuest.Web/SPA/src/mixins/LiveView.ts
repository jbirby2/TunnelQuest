
import Vue from "vue";

import Auction from "../interfaces/Auction";
import LinesAndAuctions from "../interfaces/LinesAndAuctions";

import TQGlobals from "../classes/TQGlobals";


// LiveView provides functionality for components that need to display an overall snapshot of the recent live data feed in real-time (i.e. ChatView and AuctionHouseView)

export default Vue.extend({

    data: function () {
        return {
            transitionName: "slidedown"
        };
    },
    mounted: function () {
        TQGlobals.init(() => {
            // wire event handlers
            TQGlobals.connection.on("NewChatLines", this.onNewContent);
            TQGlobals.connection.onConnected(this.onConnected);
            TQGlobals.connection.onDisconnected(this.onDisconnected);

            // pull the initial data
            if (TQGlobals.connection.isConnected())
                this.getLatestContent();
            else
                TQGlobals.connection.connect();

            this.onInitialized();
        });
    },

    activated: function () {
        console.log("stub activated");
        window.addEventListener("scroll", this.onScroll);
        
        // force scroll to top, if not already scrolled to top
        if (document != null && document.documentElement != null && document.documentElement.scrollTop != 0)
            window.scrollTo(0, 0);
    },

    deactivated: function () {
        console.log("stub deactivated");
        window.removeEventListener("scroll", this.onScroll);
    },

    beforeDestroy: function () {
        // unwire event handlers
        TQGlobals.connection.off("NewChatLines", this.onNewContent);
        TQGlobals.connection.offConnected(this.onConnected);
        TQGlobals.connection.offDisconnected(this.onDisconnected);

        this.onDestroying();
    },

    methods: {

        onConnected: function () {
            this.getLatestContent();
        },

        onDisconnected: function () {
        },

        onScroll: function () {
            //console.log("stub onScroll");

            if (document == null || document.documentElement == null)
                return;

            let isScrolledToTop = (document.documentElement.scrollTop == 0);
            let isScrolledToBottom = (Math.ceil(document.documentElement.scrollTop) + window.innerHeight >= document.documentElement.scrollHeight);

            //console.log("scrollTop " + document.documentElement.scrollTop.toString() + " innerHeight " + window.innerHeight.toString() + " scrollheight " + document.documentElement.scrollHeight.toString());

            if (isScrolledToTop) {
                this.transitionName = "slidedown";

                if (!TQGlobals.connection.isConnected())
                    TQGlobals.connection.connect();
            }
            else {
                this.transitionName = "none";

                if (TQGlobals.connection.isConnected())
                    TQGlobals.connection.disconnect();

                if (isScrolledToBottom)
                    this.getEarlierContent();
            }
        },
        
        wireUpRelationships: function (newContent: LinesAndAuctions) {

            // populate all of the auction.chatLine properties
            for (let auctionId in newContent.auctions) {
                let auction = newContent.auctions[auctionId];
                auction.chatLine = newContent.lines[auction.chatLineId];
            }

            // populate all of the chatLine.auction properties
            for (let chatLineId in newContent.lines) {
                let chatLine = newContent.lines[chatLineId];
                chatLine.auctions = new Array<Auction>();
                for (let auctionId of chatLine.auctionIds) {
                    chatLine.auctions[auctionId] = newContent.auctions[auctionId];
                }
            }
        },

        onInitialized: function () {
            // overridden by extending components
        },

        getLatestContent: function () {
            // overridden by extending components
        },

        getEarlierContent: function () {
            // overridden by extending components
        },

        onNewContent: function (newContent: LinesAndAuctions) {
            // overridden by extending components
        },

        onDestroying: function () {
            // overridden by extending components
        },
    }
});
