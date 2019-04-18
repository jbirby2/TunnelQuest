
<style>
    .tqNewspaperAuctionView {
        margin: 5px 5px 5px 5px;
        display: inline-block;
        vertical-align: top;
    }

    .tqNewspaperAuctionView > span {
        display: block;
    }

    .tqNewspaperAuctionViewWtb {
        background-color: rgba(7,0,99,0.7);
    }

    .tqNewspaperAuctionViewWts {
        background-color: rgba(0,0,0,0.7);
    }

    /* override some of the item css to make it look better on this page */
    .tqNewspaperAuctionView .tqItem {
        /* unset this in .tqItem since we're setting it above (trust me it looks a lot better) */
        background-color: unset;
    }

    .tqNewspaperAuctionId {
        white-space: nowrap;
        color: #ffdc87;
    }

    .tqNewspaperAuctionHeader {
        display: table-row;
        padding: 0 5px 0 5px;
        display: flex;
        justify-content: space-between;
        font-style: italic;
    }

    .tqNewspaperAuctionHeader > span {
        display: table-cell;
    }

    .tqNewspaperAuctionPriceHeader {
        white-space: nowrap;
    }

    .tqNewspaperAuctionTimeStamp {
        font-style: italic;
        white-space: nowrap;
        width: 100%;
        text-align: right;
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
    <span :class="cssClasses">
        <span class="tqNewspaperAuctionHeader">
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
                    <span>for {{ formattedPrice }}</span>
                    <price-deviation-view :itemName="auction.itemName" :price="auction.price" :isBuying="auction.isBuying"></price-deviation-view>
                </span>
            </span>

            <time-stamp :timeString="auction.createdAtString" cssClass="tqNewspaperAuctionTimeStamp"></time-stamp>
        </span>

        <span>
            <item-view :item="auction.item" :showAliases="false">
                <template slot="footer">
                    <span class="tqNewspaperAuctionChatLine">
                        <chat-line-view :chatLine="auction.chatLine" :itemNameToHighlight="auction.itemName" :showTimestamp="false" :itemNameLinks="false"></chat-line-view>
                    </span>
                </template>
            </item-view>
        </span>        
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
    import PriceDeviationView from "./PriceDeviationView.vue";


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
            cssClasses: function () {
                return "tqNewspaperAuctionView " + (this.auction.isBuying ? "tqNewspaperAuctionViewWtb" : "tqNewspaperAuctionViewWts");
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
            ChatLineView,
            PriceDeviationView
        }
    });
</script>

