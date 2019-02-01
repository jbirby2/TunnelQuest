
<style>
    .tqNewspaperAuctionView {
        opacity: 0.65;
        background-color: #000000;
        margin: 5px 5px 5px 5px;
        display: inline-block;
        vertical-align: top;
    }

    /* override some of the item css to make it look better on this page */
    .tqNewspaperAuctionView .tqItem {
        /* unset these in .tqItem since we're setting them above (trust me it looks a lot better) */
        opacity: unset;
        background-color: unset;
    }

    .tqNewspaperAuctionView > span {
        display: block;
    }

    .tqNewspaperAuctionId {
        white-space: nowrap;
        color: #ffdc87;
    }

    .tqNewspaperAuctionWtb {
        background-color: #007a18;
        font-style: italic;
    }

    .tqNewspaperAuctionWts {
        background-color: #001977;
        font-style: italic;
    }

    .tqNewspaperAuctionPriceHeader {
        margin-left: 5px;
    }

    .tqNewspaperAuctionTimeStamp {
        float: right;
        font-style: italic;
        text-align: right;
        white-space: nowrap;
        margin-right: 5px;
    }

    .tqNewspaperAuctionChatLine {
        margin-top: 8px;
        font-size: 0.8em;
        font-style: italic;
        color: #808080;
    }


    @media screen and (min-width: 952px) {
        /* start of desktop styles */
        .tqNewspaperAuctionView {
        }
    }

    @media screen and (max-width: 951px) {
        /* start of large tablet styles */
        .tqNewspaperAuctionView {
        }
    }

    @media screen and (max-width: 767px) {
        /* start of medium tablet styles */
        .tqNewspaperAuctionView {
        }
    }

    @media screen and (max-width: 609px) {
        .tqNewspaperAuctionView {
        }
    }

    @media screen and (max-width: 479px) {
        /* start of phone styles */
        .tqNewspaperAuctionView {
            width: 100%;
        }
    }

</style>

<template>
    <span class="tqNewspaperAuctionView">
        <span :class="headerCssClass">
            <if-debug>
                <span class="tqNewspaperAuctionId">
                    [A{{auction.id}}]
                </span>
            </if-debug>

            <span class="tqNewspaperAuctionPriceHeader">
                <span>
                    {{ auction.chatLine.playerName }}   
                </span>
                <span>
                    {{ auction.isBuying ? "is buying" : "is selling" }}
                </span>
                <span v-if="auction.price != null">
                    for {{ formattedPrice }}
                </span>
            </span>

            <time-stamp :timeString="auction.updatedAtString" cssClass="tqNewspaperAuctionTimeStamp"></time-stamp>
        </span>

        <item-view :item="auction.item">
            <template slot="footer">
                <span class="tqNewspaperAuctionChatLine">
                    <chat-line-view :chatLine="auction.chatLine" :auctionIdToHighlight="auction.id" :showTimestamp="false" :itemNameLinks="false"></chat-line-view>
                </span>
            </template>
        </item-view>
        
    </span>
</template>

<script lang="ts">
    import Vue from "vue";

    import Auction from "../interfaces/Auction";
    import ChatLine from "../interfaces/ChatLine";

    import TQGlobals from '../classes/TQGlobals';

    import IfDebug from "./IfDebug.vue";
    import TimeStamp from "./TimeStamp.vue";
    import ItemView from "./ItemView.vue";
    import ChatLineView from "./ChatLineView.vue";


    export default Vue.extend({

        props: {
            auction: {
                type: Object as () => Auction,
                required: true
            },
            chatLine: {
                type: Object as () => ChatLine,
                required: true
            }
        },

        computed: {
            headerCssClass: function () {
                return this.auction.isBuying ? "tqNewspaperAuctionWtb" : "tqNewspaperAuctionWts";
            },

            formattedPrice: function () {
                if (this.auction.price == null)
                    return null;
                else
                    return TQGlobals.formatNumber(this.auction.price, 0, ".", ",");
            }
        },

        components: {
            IfDebug,
            TimeStamp,
            ItemView,
            ChatLineView
        }
    });
</script>

