﻿
import mixins from 'vue-typed-mixins';
import { HubConnectionState } from '@aspnet/signalr';

import LiveComponent from "./LiveComponent";

import ChatLine from "../interfaces/ChatLine";
import Auction from "../interfaces/Auction";
import LinesAndAuctions from "../interfaces/LinesAndAuctions";

import TQGlobals from "../classes/TQGlobals";
import SlidingList from "../classes/SlidingList";


// LiveView inherits from LiveComponent, and provides core functionality for components that need to display an overall
// snapshot of the recent live data feed in real-time (i.e. ChatView and AuctionHouseView)

export default mixins(LiveComponent).extend({

    data: function () {
        return {
            chatLines: new SlidingList<ChatLine>(),
            auctions: new SlidingList<Auction>(),

            // "private"
            isScrolledToTop_: true,
            isScrolledToBottom_: false
        };
    },

    deactivated: function () {
        window.removeEventListener("scroll", this.onScroll);
    },

    methods: {

        // inherited from LiveComponent
        onInitialized: function () {
            this.chatLines.maxSize = TQGlobals.settings.maxChatLines;
            this.auctions.maxSize = TQGlobals.settings.maxAuctions;

            window.addEventListener("scroll", this.onScroll);

            // If the window is already scrolled to the top, then call reconnectAndCatchUp() directly.  Else, force the window
            // to scroll to the top, which will trigger onScroll, which will call reconnectAndCatchUp().
            if (document != null && document.documentElement != null && document.documentElement.scrollTop == 0)
                this.reconnectAndCatchUp();
            else
                window.scrollTo(0, 0);
        },

        // inherited from LiveComponent
        onNewContent: function (newLines: LinesAndAuctions) {
            // stub
            console.log("LiveView.onNewContent():");
            console.log(newLines);

            this.auctions.add(newLines.auctions);
            this.chatLines.add(newLines.lines);

            // stub
            //this.chatLines.consoleDump("chatLines");
            //this.auctions.consoleDump("auctions");
        },

        onScroll: function () {
            if (document == null || document.documentElement == null)
                return;

            let wasAtTop = this.isScrolledToTop_;
            let wasAtBottom = this.isScrolledToBottom_;

            this.isScrolledToTop_ = (document.documentElement.scrollTop == 0);
            this.isScrolledToBottom_ = (document.documentElement.scrollTop + window.innerHeight === document.documentElement.offsetHeight);

            if (wasAtTop && !this.isScrolledToTop_) {
                // stub
                console.log("disconnecting");

                // disconnect from signalr and stop receiving new lines if we leave the top of the list
                TQGlobals.connection.stop();
            }
            else if (this.isScrolledToTop_) {
                // reconnect to signalr and also request the next set of lines after the last one we received
                this.reconnectAndCatchUp();
            }

            if (this.isScrolledToBottom_) {
                this.getEarlierContent();
            }
        },

        reconnectAndCatchUp: function () {
            console.log("stub: reconnectAndCatchUp()");
            console.log(TQGlobals.connection.state);

            if (TQGlobals.connection.state != HubConnectionState.Connected) {
                console.log("starting connection");
                TQGlobals.connection
                    .start()
                    .then(() => {
                        console.log("connected");
                        // Don't make the call to getLatestContent() until AFTER we've connected to signalr.
                        // This ensures that we don't miss any lines, but it also means there's a slight possibility
                        // that we'll get a few duplicate lines.  That's ok however, because SlidingList will handle duplicates.
                        this.getLatestContent();
                    })
                    .catch(err => {
                        // stub
                        console.log(err);
                    }); // end connection.start()
            }
            else {
                // We're already connected, so just make sure we've got the latest content.
                this.getLatestContent();
            }
            
        },

        getLatestContent: function () {
            // overridden by extending components
        },
        
        getEarlierContent: function () {
            // overridden by extending components
        },
    },

    beforeDestroy: function () {
        this.chatLines.clear();
        this.auctions.clear();
    }
});
