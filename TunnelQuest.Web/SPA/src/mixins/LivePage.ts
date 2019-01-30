
import Vue from "vue";

import Auction from "../interfaces/Auction";
import LinesAndAuctions from "../interfaces/LinesAndAuctions";

import TQGlobals from "../classes/TQGlobals";


// LivePage provides functionality for page components that need to display an overall snapshot of the recent live data feed in real-time (i.e. ChatView and AuctionHouseView)

export default Vue.extend({

    data: function () {
        return {
            transitionName: "slidedown"
        };
    },

    mounted: function () {
        TQGlobals.onInit(this.onInit);
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
        TQGlobals.connection.off("NewChatLines", this.onNewChatLines);
        TQGlobals.connection.offConnected(this.onConnected);
        TQGlobals.connection.offDisconnected(this.onDisconnected);

        this.onDestroying();
    },

    methods: {

        onInit: function () {
            // wire event handlers
            TQGlobals.connection.on("NewChatLines", this.onNewChatLines);
            TQGlobals.connection.onConnected(this.onConnected);
            TQGlobals.connection.onDisconnected(this.onDisconnected);

            // pull the initial data
            if (TQGlobals.connection.isConnected())
                this.getLatestContent();
            else
                TQGlobals.connection.connect();

            this.onInitialized();
        },

        onNewChatLines: function (newContent: LinesAndAuctions) {
            this.onNewContent(newContent, true);
        },

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
        
        onInitialized: function () {
            // overridden by extending components
        },

        getLatestContent: function () {
            // overridden by extending components
        },

        getEarlierContent: function () {
            // overridden by extending components
        },

        onNewContent: function (newContent: LinesAndAuctions, enforceMaxSize: boolean) {
            // overridden by extending components
        },

        onDestroying: function () {
            // overridden by extending components
        },
    }
});
