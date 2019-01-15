<template>
    <div class="chatLineView">
        <time-stamp :time="chatLine.sentAt"></time-stamp> {{chatLine.playerName}} auctions, '<span class="playerText"></span>'
    </div>
</template>

<script lang="ts">
    import Vue from "vue";

    import ChatLine from "../interfaces/ChatLine";
    import Auction from "../interfaces/Auction";
    import Settings from "../interfaces/Settings";

    import SlidingList from "../classes/SlidingList";

    import TimeStamp from "./TimeStamp.vue";

    export default Vue.extend({
        props: {
            settings: {
                type: Object as () => Settings,
                required: true
            },
            auctions: {
                type: Object as () => SlidingList<Auction>,
                required: true
            },
            chatLine: {
                type: Object as () => ChatLine,
                required: true
            }
        },
        data: function () {
            return {
            };
        },
        created: function () {
        },
        mounted: function () {
            let textSpan = this.$el.querySelector(".playerText") as HTMLSpanElement;

            let wordsSoFar : string | null = null;
            let textWords = this.chatLine.text.split(" ");
            for (let word of textWords) {
                if (wordsSoFar == null)
                    wordsSoFar = "";
                else
                    wordsSoFar += " ";

                if (word.substring(0, this.settings.auctionToken.length) === this.settings.auctionToken) {
                    // createTextNode() html encodes the player-typed text to protect against html injection attacks
                    textSpan.appendChild(document.createTextNode(wordsSoFar));
                    wordsSoFar = "";

                    let auctionId = parseInt(word.substring(this.settings.auctionToken.length));
                    let linkElem = document.createElement("a") as HTMLAnchorElement;
                    linkElem.href = "#auction_id=" + auctionId.toString();
                    linkElem.text = this.auctions.dict[auctionId].itemName;
                    textSpan.appendChild(linkElem);
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
</style>