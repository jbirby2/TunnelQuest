
<style>

    .tqChatLineId {
        color: #08ff68;
    }

    .tqChatLineView_PlayerName {
        font-style: italic;
    }

    .tqChatLineView_auctions {
        font-style: italic;
    }

    .tqChatLineView_PlayerText {
    }

    .tqChatLineTimeStamp {
        font-family: Courier New, Courier, monospace;
        color: #c9c9c9;
        font-size: 0.8em;
    }

    @media screen and (min-width: 992px) {
        /* start of desktop styles */
        .tqChatLineTimeStamp {
            margin-right: 7px;
        }
    }

    @media screen and (max-width: 991px) {
        /* start of large tablet styles */
        .tqChatLineTimeStamp {
            margin-right: 6px;
        }
    }

    @media screen and (max-width: 767px) {
        /* start of medium tablet styles */
        .tqChatLineTimeStamp {
            margin-right: 5px;
        }
    }

    @media screen and (max-width: 479px) {
        /* start of phone styles */
        .tqChatLineTimeStamp {
            margin-right: 4px;
        }
    }
</style>

<template>
    <span :class="'tqChatLineView ' + cssClass">
        <if-debug>
            <span class="tqChatLineId">[C{{chatLine.id}}]</span>
        </if-debug>
        <time-stamp v-if="showTimestamp" :timeString="chatLine.sentAtString" cssClass="tqChatLineTimeStamp"></time-stamp>
        <span class="tqChatLineView_PlayerName">{{chatLine.playerName}}</span>
        <span class="tqChatLineView_auctions"> auctions, </span>
        <span class="tqChatLineView_PlayerText"></span>
    </span>
</template>

<script lang="ts">
    import Vue from "vue";

    import ChatLine from "../interfaces/ChatLine";

    import TQGlobals from "../classes/TQGlobals";

    import IfDebug from "./IfDebug.vue";
    import TimeStamp from "./TimeStamp.vue";
    import ItemLink from "./ItemLink.vue";
    import PriceDeviationView from "./PriceDeviationView.vue";

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
            cssClass: {
                type: String,
                required: false
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

                textSpan.appendChild(document.createTextNode("'"));

                let unparsedText = this.chatLine.text;
                let textSoFar = ""
                while (unparsedText.length > 0) {

                    if (unparsedText.substring(0, TQGlobals.settings.chatToken.length) === TQGlobals.settings.chatToken) {
                        // the next word is a special data token

                        // create a text span for the player-typed words we've found up to this point in the loop
                        let playerTextSpan = document.createElement("span") as HTMLSpanElement;
                        playerTextSpan.innerHTML = this.htmlEncode(textSoFar);
                        textSpan.appendChild(playerTextSpan);
                        textSoFar = ""; // reset textSoFar for the next iteration after the token

                        let closingTokenIndex = unparsedText.indexOf(TQGlobals.settings.chatToken, TQGlobals.settings.chatToken.length);
                        let auctionId = parseInt(unparsedText.substring(TQGlobals.settings.chatToken.length, closingTokenIndex));
                        let auction = this.chatLine.auctions[auctionId];

                        // create an ItemLink for this auction
                        let itemLinkElem = document.createElement("span") as HTMLSpanElement;
                        textSpan.appendChild(itemLinkElem);
                        let itemLink = new ItemLink({
                            propsData: {
                                itemName: auction.itemName,
                                aliasText: auction.aliasText,
                                isKnown: auction.isKnownItem,
                                router: this.$router
                            }
                        });
                        itemLink.$mount(itemLinkElem);
                        
                        // create a PriceDeviationView for this auction
                        if (auction.price != null) {
                            let priceDeviationElem = document.createElement("span") as HTMLSpanElement;
                            textSpan.appendChild(priceDeviationElem);
                            let priceDeviationView = new PriceDeviationView({
                                propsData: {
                                    itemName: auction.itemName,
                                    price: auction.price,
                                    isBuying: auction.isBuying
                                }
                            });
                            priceDeviationView.$mount(priceDeviationElem);
                        }
                        
                        // update unparsedText
                        let nextIndex = closingTokenIndex + TQGlobals.settings.chatToken.length;
                        if (nextIndex < unparsedText.length)
                            unparsedText = unparsedText.substring(nextIndex);
                        else
                            unparsedText = "";
                    }
                    else {
                        textSoFar += unparsedText[0];
                        // updated unparsedText
                        unparsedText = unparsedText.substring(1);
                    }

                }

                if (textSoFar != "") {
                    let playerTextSpan = document.createElement("span") as HTMLSpanElement;
                    playerTextSpan.innerHTML = this.htmlEncode(textSoFar);
                    textSpan.appendChild(playerTextSpan);
                }

                textSpan.appendChild(document.createTextNode("'"));
            },

            htmlEncode: function (str: string) {
                return String(str).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/ /g, '&nbsp;');
            }
        },
        components: {
            IfDebug,
            TimeStamp
        }
    });
</script>
