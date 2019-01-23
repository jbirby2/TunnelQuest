<template>
    <div class="tqChatLineView">
        [{{chatLine.id}}]
        <time-stamp v-if="showTimestamp" :timeString="chatLine.sentAtString"></time-stamp>
        <span class="tqChatLineView_PlayerName">{{chatLine.playerName}} auctions,</span> '<span class="tqChatLineView_PlayerText"></span>'
    </div>
</template>

<script lang="ts">
    import Vue from "vue";

    import ChatLine from "../interfaces/ChatLine";

    import TQGlobals from "../classes/TQGlobals";

    import TimeStamp from "./TimeStamp.vue";

    export default Vue.extend({
        props: {
            chatLine: {
                type: Object as () => ChatLine,
                required: true
            },
            showTimestamp: {
                type: Boolean,
                required: true
            },
            itemNameLinks: {
                type: Boolean,
                required: true
            }
        },
        watch: {
            chatLine: function (newValue, oldValue) {
                this.rebuildText();
            }
        },
        mounted: function () {
            this.rebuildText();
        },
        methods: {
            rebuildText: function () {
                let textSpan = this.$el.querySelector(".tqChatLineView_PlayerText") as HTMLSpanElement;
                
                // remove whatever text we built in there last time
                while (textSpan.lastChild) {
                    textSpan.removeChild(textSpan.lastChild);
                }

                let nextItemNameIndex = 0;
                let wordsSoFar: string | null = null;
                let textWords = this.chatLine.text.split(" ");
                for (let word of textWords) {
                    if (wordsSoFar == null)
                        wordsSoFar = "";
                    else
                        wordsSoFar += " ";

                    if (word.substring(0, TQGlobals.settings.auctionToken.length) === TQGlobals.settings.auctionToken) {
                        // createTextNode() html encodes the player-typed text to protect against html injection attacks
                        textSpan.appendChild(document.createTextNode(wordsSoFar));
                        wordsSoFar = "";

                        let auctionId = parseInt(word.substring(TQGlobals.settings.auctionToken.length));
                        let auctionInfo = this.chatLine.auctions[auctionId];

                        if (this.itemNameLinks && auctionInfo.isKnownItem) {
                            let linkElem = document.createElement("a") as HTMLAnchorElement;
                            linkElem.href = "#/item/" + auctionInfo.itemName;
                            linkElem.text = auctionInfo.itemName;
                            textSpan.appendChild(linkElem);
                        }
                        else {
                            wordsSoFar += auctionInfo.itemName;
                        }
                    }
                    else {
                        wordsSoFar += word;
                    }
                }
                if (wordsSoFar != null && wordsSoFar != "")
                    textSpan.appendChild(document.createTextNode(wordsSoFar));
            }
        },
        components: {
            TimeStamp
        }
    });
</script>

<style>
    .tqChatLineView {
    }

    .tqChatLineView_PlayerName {
        font-style: italic;
    }

    .tqChatLineView_PlayerText {
    }

</style>