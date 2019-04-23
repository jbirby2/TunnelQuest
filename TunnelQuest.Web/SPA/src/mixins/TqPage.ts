import axios from "axios";
import Vue from "vue";

import ChatLine from "../interfaces/ChatLine";
import ChatLinePayload from "../interfaces/ChatLinePayload";


import TQGlobals from "../classes/TQGlobals";


// TqPage is the base page from which every page inherits

export default Vue.extend({

    data: function () {
        return {
            isInitialized: false,
            chatLines: new Array<ChatLine>(),
            chatLinesDict: new Map<number, ChatLine>(),
        };
    },

    beforeMount: function () {
        //stub
        //console.log("TqPage.beforeMount");
    },

    mounted: function () {
        //stub
        //console.log("LivePage.mounted");

        window.addEventListener("scroll", this.onScroll);

        TQGlobals.init(() => {
            this.isInitialized = true;
            this.onInitialized();
        });
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
        console.log("TqPage.beforeDestroy()");

        window.removeEventListener("scroll", this.onScroll);
        this.chatLines = new Array<ChatLine>();
        this.chatLinesDict.clear();
    },

    methods: {

        // Called in this component to add the results of API queries (from scrolling), and also called by
        // LivePage to add the results of new lines from the live hub feed.
        addChatLines: function (newLines: ChatLinePayload, enforceMaxSize: boolean) {

            //stub
            console.log("TqPage.addChatLines()");
            console.log(newLines);

            let newLinesArray = new Array<ChatLine>();
            if (newLines.lines) {
                for (let chatLineId in newLines.lines) {
                    let chatLine = newLines.lines[chatLineId];

                    // avoid adding duplicate chat lines
                    if (!this.chatLinesDict.has(chatLine.id)) {
                        for (let auctionId in chatLine.auctions) {
                            let auction = chatLine.auctions[auctionId];

                            // give each auction a reference back to its own chat line
                            auction.chatLine = chatLine;

                            // for convenience, so we can always just use aliasText throughout the SPA instead of constantly checking if it's null
                            if (auction.aliasText == null)
                                auction.aliasText = auction.itemName;

                            TQGlobals.priceHistories.get(auction.itemName, false);
                        }

                        newLinesArray.push(chatLine);
                        this.chatLines.push(chatLine);
                        this.chatLinesDict.set(chatLine.id, chatLine);
                    }
                }
            }

            this.chatLines.sort(function (a: ChatLine, b: ChatLine) {
                // sort ascending
                if (a.id < b.id)
                    return -1;
                else if (a.id > b.id)
                    return 1;
                else
                    return 0;
            });

            // pass along to extending components
            if (newLinesArray.length > 0)
                this.onChatLinesLoaded(newLinesArray);

            // (no need to re-sort after enforcing max size because trimming from the start of the array won't change the sort order)
            if (enforceMaxSize) {
                let removedLines = new Array<ChatLine>();
                while (this.chatLines.length > TQGlobals.settings.maxChatLines) {
                    let removedLine = this.chatLines.shift();
                    if (removedLine) {
                        this.chatLinesDict.delete(removedLine.id);
                        removedLines.push(removedLine);
                    }
                }

                // pass along to extending components
                if (removedLines.length > 0)
                    this.onChatLinesUnloaded(removedLines);
            }

            // fetch all price histories at once
            TQGlobals.priceHistories.fetchPendingPriceHistories();
        },

        onScroll: function () {

            if (document == null || document.documentElement == null)
                return;

            // stub
            //console.log("scrollTop " + document.documentElement.scrollTop.toString() + " innerHeight " + window.innerHeight.toString() + " scrollheight " + document.documentElement.scrollHeight.toString());

            if (this.chatLines.length > 0 && this.isScrolledToBottom())
                this.loadEarlierFilteredChatLines();

            this.onScrolled();
        },

        isScrolledToTop: function () {
            return (document != null && document.documentElement != null && document.documentElement.scrollTop == 0);
        },

        isScrolledToBottom: function () {
            return (document != null && document.documentElement != null && Math.ceil(document.documentElement.scrollTop) + window.innerHeight >= document.documentElement.scrollHeight);
        },

        // called by extending components
        loadLatestFilteredChatLines: function () {
            let minId: number | null = null;
            if (this.chatLines.length > 0)
                minId = this.chatLines[this.chatLines.length - 1].id + 1;

            axios.post('/api/chat_query', {
                serverCode: TQGlobals.serverCode,
                minimumId: minId,
                filterSettings: this.getChatFilterSettings()
            })
            .then(response => {
                let result = response.data as ChatLinePayload;
                this.addChatLines(result, true);
            })
            .catch(err => {
                // stub
                console.log(err);
            });
        },

        // called only within this component
        loadEarlierFilteredChatLines: function () {
            let maxId: number | null = null;
            if (this.chatLines.length > 0)
                maxId = this.chatLines[0].id - 1;

            axios.post('/api/chat_query', {
                serverCode: TQGlobals.serverCode,
                maximumId: maxId,
                filterSettings: this.getChatFilterSettings()
            })
            .then(response => {
                let result = response.data as ChatLinePayload;
                this.addChatLines(result, false);
            })
            .catch(err => {
                // stub
                console.log(err);
            });
        },

        onInitialized: function () {
            // overridden by extending components
        },

        onScrolled: function () {
            // overridden by extending components
        },

        getChatFilterSettings: function () {
            // overridden by extending components
        },

        onChatLinesLoaded: function (newLines: Array<ChatLine>) {
            // overridden by extending components
        },

        onChatLinesUnloaded: function (removedLines: Array<ChatLine>) {
            // overridden by extending components
        },
    }
});
