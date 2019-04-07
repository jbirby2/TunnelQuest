
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

    .tqKnownItemLink {
        color: #e049ff;
        text-decoration: none;
    }

    .tqUnknownItemLink {
        color: #f7d8ff;
        text-decoration: none;
    }

    .tqHighlightedItemLink {
        font-weight: bold;
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
            itemNameLinks: {
                type: Boolean,
                required: true
            },
            itemNameToHighlight: {
                type: String,
                required: false
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

                let indexedItemNames = new Array<string>(); // used when adding PriceDeviationComponents
                let unparsedText = this.chatLine.text;
                let textSoFar = ""
                let nextTokenIndex = 0;
                while (unparsedText.length > 0) {

                    if (unparsedText.substring(0, TQGlobals.settings.chatToken.length) === TQGlobals.settings.chatToken) {
                        // the next word is a special data token

                        // create a text span for the player-typed words we've found up to this point in the loop
                        let playerTextSpan = document.createElement("span") as HTMLSpanElement;
                        playerTextSpan.innerHTML = this.htmlEncode(textSoFar);
                        textSpan.appendChild(playerTextSpan);
                        textSoFar = ""; // reset textSoFar for the next iteration after the token

                        let token = this.chatLine.tokens[nextTokenIndex];
                        nextTokenIndex++;

                        if (token.type == "ITEM") {
                            // item name token

                            let isKnownItem = (token.properties["isKnown"] == "1");
                            let itemName = token.properties["itemName"];
                            let text = token.properties["text"];
                            let urlEncodedItemName = encodeURIComponent(itemName);

                            indexedItemNames.push(itemName);

                            if (this.itemNameLinks) {
                                // make the item name a clickable link
                                let linkElem = document.createElement("a") as HTMLAnchorElement;

                                linkElem.classList.add(isKnownItem ? "tqKnownItemLink" : "tqUnknownItemLink");
                                if (this.itemNameToHighlight == itemName)
                                    linkElem.classList.add("tqHighlightedItemLink");

                                linkElem.href = "/item/" + urlEncodedItemName;
                                let thisComponent = this;
                                linkElem.addEventListener("click", function (e) {
                                    e.preventDefault();
                                    thisComponent.$router.push("/item/" + urlEncodedItemName);
                                });
                                linkElem.text = text;
                                textSpan.appendChild(linkElem);
                            }
                            else if (this.itemNameToHighlight == itemName) {
                                // highlight the item name without making it a clickable link
                                let spanElem = document.createElement("span") as HTMLSpanElement;
                                spanElem.classList.add(isKnownItem ? "tqKnownItemLink" : "tqUnknownItemLink");
                                spanElem.innerHTML = this.htmlEncode(text);
                                textSpan.appendChild(spanElem);
                            }
                            else {
                                textSoFar += text;
                            }

                        }
                        else if (token.type == "PRICE") {
                            // price token

                            let isBuying = (token.properties["isBuying"] == "1");
                            let price = parseInt(token.properties["price"]);
                            let itemIndexes = token.properties["items"].split(',');
                            let playerTypedPriceText = token.properties["text"];

                            // render the text that the player actually typed in chat
                            let priceElem = document.createElement("span") as HTMLSpanElement;
                            priceElem.innerHTML = this.htmlEncode(playerTypedPriceText);
                            textSpan.appendChild(priceElem);

                            // now create a PriceDeviationView for each item associated with this price
                            for (let itemNameIndexString of itemIndexes) {
                                let itemName = indexedItemNames[parseInt(itemNameIndexString)];
                                let priceDeviationElem = document.createElement("span") as HTMLSpanElement;
                                textSpan.appendChild(priceDeviationElem);
                                let priceDeviationView = new PriceDeviationView({
                                    propsData: {
                                        itemName: itemName,
                                        price: price,
                                        isBuying: isBuying
                                    }
                                });
                                priceDeviationView.$mount(priceDeviationElem);
                            }
                        }
                        else {
                            // unrecognized token
                            throw new Error("Unrecognized chat token type: " + token.type);
                        }

                        // update unparsedText
                        let nextIndex = TQGlobals.settings.chatToken.length;
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
