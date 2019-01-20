
import Vue from "vue";

import TQGlobals from "../classes/TQGlobals";

import LinesAndAuctions from "../interfaces/LinesAndAuctions";

// LiveComponent provides core functionality for components that need to react to the live data feed in real-time

export default Vue.extend({

    activated: function () {
        TQGlobals.init(() => {
            TQGlobals.connection.on("NewChatLines", this.onNewContent);
            this.onInitialized();
        });
    },

    deactivated: function () {
        TQGlobals.connection.off("NewChatLines", this.onNewContent);
    },

    methods: {
        onInitialized: function () {
            // overridden by extending components
        },

        onNewContent: function (newLines: LinesAndAuctions) {
            // overridden by extending components
        }
    }
});
