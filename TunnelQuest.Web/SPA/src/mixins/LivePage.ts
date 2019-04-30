import mixins from 'vue-typed-mixins';

import ChatLine from "../interfaces/ChatLine";
import Auction from "../interfaces/Auction";
import ChatLinePayload from "../interfaces/ChatLinePayload";

import TQGlobals from "../classes/TQGlobals";
import ConnectionWrapper from "../classes/ConnectionWrapper";

import TqPage from "../mixins/TqPage";


// LivePage provides functionality for page components that need to display an overall snapshot of the recent live data feed in real-time (i.e. ChatView and AuctionHouseView)

export default mixins(TqPage).extend({

    data: function () {
        return {
            isActive: true,
            transitionName: "slidedown",
            connection: new ConnectionWrapper(),
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

    activated: function () {
        this.isActive = true;

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
        this.isActive = false;

        if (this.isInitialized && this.connection.isConnected())
            this.connection.disconnect();
    },

    beforeDestroy: function () {
        // unwire from global events
        TQGlobals.filterManager.offSelectedFilterChanged(this.onSelectedFilterChangedOrEdited);
        TQGlobals.filterManager.offSelectedFilterEdited(this.onSelectedFilterChangedOrEdited);

        // shut down the connection
        this.connection.off("NewContent", this.onNewLiveChat);
        this.connection.offConnected(this.onConnected);
        //this.connection.offDisconnected(this.onDisconnected);
        if (this.connection.isConnected())
            this.connection.disconnect();
        this.connection.destroy();
    },
    
    methods: {

        getHubUrl: function () {
            // STUB hard-coded
            return "/blue_chat_hub";
        },

        // inherited from TqPage
        onInitialized: function () {
            // wire up to global events
            TQGlobals.filterManager.onSelectedFilterChanged(this.onSelectedFilterChangedOrEdited);
            TQGlobals.filterManager.onSelectedFilterEdited(this.onSelectedFilterChangedOrEdited);

            // create connection
            this.connection.setHubUrl(this.getHubUrl());
            this.connection.on("NewContent", this.onNewLiveChat);
            this.connection.onConnected(this.onConnected);
            //this.connection.onDisconnected(this.onDisconnected);
            this.connection.connect();
        },

        onSelectedFilterChangedOrEdited: function () {
            // stub
            console.log("LivePage.onSelectedFilterChangedOrEdited");

            // clear out all previous chat lines
            this.trimChatLines(0);

            if (this.isActive)
                this.loadLatestFilteredChatLines();
        },

        onNewLiveChat: function (liveChatLines: ChatLinePayload) {
            this.filterAndAddLinesAsync(liveChatLines);
        },

        onConnected: function () {
            //stub
            console.log("LivePage.onConnected()");

            this.loadLatestFilteredChatLines();
        },

        //onDisconnected: function () {
        //},


        // inherited from TqPage
        onScrolled: function () {
            //console.log("stub LivePage.onScrolled");

            if (this.isInitialized == false || this.isActive == false)
                return;

            if (this.isScrolledToTop()) {
                this.transitionName = "slidedown";

                if (!this.connection.isConnected())
                    this.connection.connect();
            }
            else {
                this.transitionName = "none";

                if (this.connection.isConnected())
                    this.connection.disconnect();
            }
        },

        // inherited from TqPage
        getChatFilterSettings: function () {
            return TQGlobals.filterManager.selectedFilter.settings;
        },

        filterAndAddLinesAsync: function (liveChatLines: ChatLinePayload) {
            // fetch any missing item data that will be necessary to apply the filter logic
            this.fetchNecessaryItemsAsync(liveChatLines.lines, () => {
                // fetch any missing price history data that will be necessary to apply the filter logic
                this.fetchNecessaryPriceHistoryAsync(liveChatLines.lines, () => {
                    // now we've got all the data necessary to evaluate the filter loaded into the repos,
                    // and we can do the filtering logic

                    let filteredChatLines = new Array<ChatLine>();

                    for (let chatLine of liveChatLines.lines) {
                        // the chatLine passes the filter if any of its auctions pass the filter
                        let anyAuctionPassedFilter = false;
                        for (let auctionId in chatLine.auctions) {
                            let auction = chatLine.auctions[auctionId];
                            // Auctions coming in from the live hub feed will NOT already have their
                            // "passesFilter" property set by the server like the API query results do,
                            // so we need to evaluate all the filtering logic here in the client script
                            // to set auction.passesFilter for these auctions.
                            auction.passesFilter = this.doesAuctionPassFilter(auction);

                            anyAuctionPassedFilter = anyAuctionPassedFilter || auction.passesFilter;
                        }
                        if (anyAuctionPassedFilter)
                            filteredChatLines.push(chatLine);
                    }

                    this.addChatLines(filteredChatLines, true);
                });
            });
        },

        doesAuctionPassFilter(auction: Auction) {
            // default to true, and then check each filter setting one-by-one to see if the auction
            // fails any of them
            let passesFilter = true;

            let fs = TQGlobals.filterManager.selectedFilter.settings; // shortcut

            if (fs.items.filterType == "name") {
                // filter by names
                if (fs.items.names.length > 0 && fs.items.names.indexOf(auction.itemName) < 0)
                    passesFilter = false;
            }
            else {
                // filter by stats

                if (fs.items.stats.minStrength != null && (auction.item.strength == null || fs.items.stats.minStrength > auction.item.strength))
                    passesFilter = false;

                // STUB TODO more filter conditions
            }
            
            return passesFilter;
        },

        fetchNecessaryItemsAsync: function (liveChatLines: Array<ChatLine>, callback: Function) {
            //STUB
            console.log("LivePage.fetchNecessaryItemsAsync()");
            console.log(liveChatLines);

            let fs = TQGlobals.filterManager.selectedFilter.settings; // shortcut

            if (fs.items.filterType == "stats") {
                for (let chatLine of liveChatLines) {
                    for (let auctionId in chatLine.auctions) {
                        let auction = chatLine.auctions[auctionId];
                        auction.item = TQGlobals.items.queue(auction.itemName);
                    }
                }

                TQGlobals.items.fetchQueuedItems(callback);
            }
            else {
                callback();
            }
        },

        fetchNecessaryPriceHistoryAsync: function (liveChatLines: Array<ChatLine>, callback: Function) {
            //STUB
            console.log("LivePage.fetchNecessaryPriceHistoryAsync()");
            console.log(liveChatLines);

            // STUB fetch logic goes here

            callback();
        }
    }
});
