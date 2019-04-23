
import mixins from 'vue-typed-mixins';

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
        //stub
        console.log("LivePage.beforeDestroy()");

        this.connection.off("NewContent", this.onNewLiveContent);
        this.connection.offConnected(this.onConnected);
        //this.connection.offDisconnected(this.onDisconnected);
        if (this.connection.isConnected())
            this.connection.disconnect();
    },
    
    methods: {

        getHubUrl: function () {
            // STUB hard-coded
            return "/blue_chat_hub";
        },

        // inherited from TqPage
        onInitialized: function () {
            // create connection
            this.connection.setHubUrl(this.getHubUrl());
            this.connection.on("NewContent", this.onNewLiveContent);
            this.connection.onConnected(this.onConnected);
            //this.connection.onDisconnected(this.onDisconnected);
            this.connection.connect();
        },

        onNewLiveContent: function (newContent: ChatLinePayload) {
            this.addChatLines(newContent, true);
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

    }
});
