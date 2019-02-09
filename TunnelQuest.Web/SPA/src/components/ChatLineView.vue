
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
            itemNameToHighlight: {
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

                    if (unparsedText.substring(0, TQGlobals.settings.itemNameToken.length) === TQGlobals.settings.itemNameToken) {
                        let playerTextSpan = document.createElement("span") as HTMLSpanElement;
                        playerTextSpan.innerHTML = this.htmlEncode(textSoFar);
                        textSpan.appendChild(playerTextSpan);
                        textSoFar = ""; // reset textSoFar

                        let endTokenIndex = unparsedText.indexOf(TQGlobals.settings.itemNameToken, TQGlobals.settings.itemNameToken.length);
                        let itemName = unparsedText.substring(TQGlobals.settings.itemNameToken.length, endTokenIndex).replace(/_/g, " ");

                        if (this.itemNameLinks) {
                            // make the item name a clickable link
                            let linkElem = document.createElement("a") as HTMLAnchorElement;
                            linkElem.classList.add("tqItemLink");

                            linkElem.href = "/item/" + itemName;
                            linkElem.setAttribute("tqItemLink", itemName);
                            let thisComponent = this;
                            linkElem.addEventListener("click", function (e) {
                                e.preventDefault();
                                thisComponent.$router.push("/item/" + itemName);
                            });

                            linkElem.text = itemName;
                            textSpan.appendChild(linkElem);
                        }
                        else if (this.itemNameToHighlight == itemName) {
                            // highlight the item name without making it a clickable link
                            let spanElem = document.createElement("span") as HTMLSpanElement;
                            spanElem.classList.add("tqItemLink");
                            spanElem.innerText = itemName;
                            textSpan.appendChild(spanElem);
                        }
                        else {
                            textSoFar += itemName;
                        }

                        // update unparsedText
                        let nextIndex = endTokenIndex + TQGlobals.settings.itemNameToken.length;
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
