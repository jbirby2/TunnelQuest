<template>
    <div class="tqChatLineView">
        [{{ chatLine.id }}] 
        <time-stamp v-if="showTimestamp" :timeString="chatLine.sentAtString"></time-stamp>
        <span class="tqChatLineView_PlayerName">{{chatLine.playerName}} auctions,</span> '<span class="tqChatLineView_PlayerText"></span>'
    </div>
</template>

<script lang="ts">
    import Vue from "vue";

    import ChatLine from "../interfaces/ChatLine";
    import Auction from "../interfaces/Auction";

    import TQGlobals from "../classes/TQGlobals";
    import SlidingList from "../classes/SlidingList";

    import TimeStamp from "./TimeStamp.vue";

    export default Vue.extend({
        props: {
            auctions: {
                type: Object as () => SlidingList<Auction>,
                required: true
            },
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
        data: function () {
            return {
            };
        },
        mounted: function () {
            let textSpan = this.$el.querySelector(".tqChatLineView_PlayerText") as HTMLSpanElement;

            let wordsSoFar : string | null = null;
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

                    if (this.itemNameLinks) {
                        let linkElem = document.createElement("a") as HTMLAnchorElement;
                        linkElem.href = "#auction_id=" + auctionId.toString();
                        linkElem.text = this.auctions.dict[auctionId].itemName;
                        textSpan.appendChild(linkElem);
                    }
                    else {
                        wordsSoFar += this.auctions.dict[auctionId].itemName;
                    }
                }
                else {
                    wordsSoFar += word;
                }
            }
            if (wordsSoFar != null && wordsSoFar != "")
                textSpan.appendChild(document.createTextNode(wordsSoFar));
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