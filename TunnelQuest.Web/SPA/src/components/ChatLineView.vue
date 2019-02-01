
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

    .tqItemLink {
        color: #e049ff;
        text-decoration: none;
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
    <span class="tqChatLineView">
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
            auctionIdToHighlight: {
                type: Number,
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
                            linkElem.classList.add("tqItemLink");

                            linkElem.href = "/item/" + auctionInfo.itemName;
                            linkElem.setAttribute("tqItemLink", auctionInfo.itemName);
                            let thisComponent = this;
                            linkElem.addEventListener("click", function (e) {
                                e.preventDefault();
                                thisComponent.$router.push("/item/" + auctionInfo.itemName);
                            });

                            linkElem.text = auctionInfo.itemName;
                            textSpan.appendChild(linkElem);
                        }
                        else if (this.auctionIdToHighlight == auctionId) {
                            let spanElem = document.createElement("span") as HTMLSpanElement;
                            spanElem.classList.add("tqItemLink");
                            spanElem.innerText = auctionInfo.itemName;
                            textSpan.appendChild(spanElem);
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

                textSpan.appendChild(document.createTextNode("'"));
            }
        },
        components: {
            IfDebug,
            TimeStamp
        }
    });
</script>
