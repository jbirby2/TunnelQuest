
import mixins from 'vue-typed-mixins';

import LinesAndAuctions from "../interfaces/LinesAndAuctions";

import TQGlobals from "../classes/TQGlobals";
import ConnectionWrapper from "../classes/ConnectionWrapper";

import TqPage from "../mixins/TqPage";


// LivePage provides functionality for page components that need to display an overall snapshot of the recent live data feed in real-time (i.e. ChatView and AuctionHouseView)

export default mixins(TqPage).extend({

    data: function () {
        return {
            isInitialized: false,
            transitionName: "slidedown",
            connection: new ConnectionWrapper()
        };
    },

    beforeRouteEnter: function (to: any, from: any, next: any) {
        //stub
        //console.log("LivePage.beforeRouteEnter");
        //console.log(to);
        //console.log(from);

        localStorage.setItem("LastLivePage", JSON.stringify({
            fullPath: to.fullPath,
            name: to.name
        }));

        next();
    },

    mounted: function () {
        //stub
        //console.log("LivePage.mounted");

        TQGlobals.init(() => {
            // create connection
            this.connection.setHubUrl(this.getHubUrl());
            this.connection.on("NewContent", this.onNewLiveContent);
            this.connection.onConnected(this.onConnected);
            this.connection.onDisconnected(this.onDisconnected);
            this.connection.connect();

            this.isInitialized = true;
            this.onInitialized();
        });
    },

    activated: function () {
        window.addEventListener("scroll", this.onScroll);

        // I don't think VueRouter provides an event hook for after it restores the saved scroll position,
        // so a hacky workaround seems to be adding a tiny delay after activated.  Otherwise, if we run
        // the below code directly inside activated, it will always reconnect because it will always be
        // scrolled to the top at this moment (before VueRouter restores the saved state)
        setTimeout(() => {
            if (this.isInitialized && this.isScrolledToTop() && !this.connection.isConnected())
                this.connection.connect();
        }, 10);
    },

    deactivated: function () {
        window.removeEventListener("scroll", this.onScroll);

        if (this.isInitialized && this.connection.isConnected())
            this.connection.disconnect();
    },

    beforeDestroy: function () {
        //stub
        console.log("LivePage.beforeDestroy()");

        // unwire event handlers
        if (this.isInitialized) {
            this.connection.off("NewContent", this.onNewLiveContent);
            this.connection.offConnected(this.onConnected);
            this.connection.offDisconnected(this.onDisconnected);
            if (this.connection.isConnected())
                this.connection.disconnect();

            this.onDestroying();
        }
    },

    methods: {

        onNewContent: function (newContent: LinesAndAuctions, enforceMaxSize: boolean) {
            
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
                    TQGlobals.priceHistories.get(auction.itemName, false);
                }
            }

            // fetch all price histories at once
            TQGlobals.priceHistories.fetchPendingPriceHistories();

            // STUB - filtering logic goes here

            // pass along to child classes
            this.onFilteredContent(newContent, enforceMaxSize);
        },

        onNewLiveContent: function (newContent: LinesAndAuctions) {
            this.onNewContent(newContent, true);
        },

        onConnected: function () {
            this.getLatestContent();
        },

        onDisconnected: function () {
        },

        onScroll: function () {
            //console.log("stub onScroll");

            if (this.isInitialized == false || document == null || document.documentElement == null)
                return;

            //console.log("scrollTop " + document.documentElement.scrollTop.toString() + " innerHeight " + window.innerHeight.toString() + " scrollheight " + document.documentElement.scrollHeight.toString());

            if (this.isScrolledToTop()) {
                this.transitionName = "slidedown";

                if (!this.connection.isConnected())
                    this.connection.connect();
            }
            else {
                this.transitionName = "none";

                if (this.connection.isConnected())
                    this.connection.disconnect();

                if (this.isScrolledToBottom())
                    this.getEarlierContent();
            }
        },

        isScrolledToTop: function () {
            return (document != null && document.documentElement != null && document.documentElement.scrollTop == 0);
        },

        isScrolledToBottom: function () {
            return (document != null && document.documentElement != null && Math.ceil(document.documentElement.scrollTop) + window.innerHeight >= document.documentElement.scrollHeight);
        },

        getHubUrl: function () {
            // overridden by extending components
            return "";
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

        onFilteredContent: function (newContent: LinesAndAuctions, enforceMaxSize: boolean) {
            // overridden by extending components
        },

        onDestroying: function () {
            // overridden by extending components
        },
    }
});
