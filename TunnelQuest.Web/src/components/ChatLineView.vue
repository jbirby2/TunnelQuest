<template>
    <div>
        {{chatLine.playerName}} auctions, '<span id="textSpan"></span>'
    </div>
</template>

<script lang="ts">
    import Vue from "vue";
    import ChatLine from "../interfaces/ChatLine";
    import Settings from "../interfaces/Settings";

    export default Vue.extend({
        props: {
            settings: {
                type: Object as () => Settings,
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
            let textSpan = document.querySelector("#textSpan") as HTMLSpanElement;

            // STUB
            //textSpan.innerHTML = this.chatLine.text;
            //console.log(this.chatLine.text);

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

                    let stub = [];
                    stub[1] = true;


                    let auctionId = parseInt(word.substring(this.settings.auctionToken.length));
                    let linkElem = document.createElement("a") as HTMLAnchorElement;
                    linkElem.href = "#auction_id=" + auctionId.toString();
                    linkElem.text = this.chatLine.auctions[auctionId].itemName;
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
        }
    });
</script>

<style>
</style>