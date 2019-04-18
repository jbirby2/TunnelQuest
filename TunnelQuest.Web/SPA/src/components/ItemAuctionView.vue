
<style>
    .tqItemAuctionView {
        display: table-row;
    }

    .tqItemAuctionView > span {
        display: table-cell;
        text-align: left;
        padding-left: 5px;
    }

    .tqItemAuctionId {
        white-space: nowrap;
        color: #ffdc87;
    }

    .tqItemAuctionTimeStamp {
        display: table-cell;
        font-family: Courier New, Courier, monospace;
        color: #c9c9c9;
        font-size: 0.8em;
        white-space: nowrap;
    }

    .tqItemAuctionWtb {
        background-color: rgba(7,0,99,0.7);
    }

    .tqItemAuctionWts {
        background-color: rgba(0,0,0,0.7);
    }

    .tqItemAuctionItemName {
        width: 100%;
    }

    .tqItemAuctionKnownItem {
        color: #e049ff;
        text-decoration: none;
    }

    .tqItemAuctionUnknownItem {
        color: #f7d8ff;
        text-decoration: none;
    }

    .tqItemAuctionPrice {
        text-align: right !important;
    }

    .tqItemAuctionPriceDeviation {
        text-align: center !important;
        padding-right: 5px;
    }

    @media screen and (min-width: 952px) {
        /* start of desktop styles */
        .tqItemAuctionView {
        }
    }

    @media screen and (max-width: 951px) {
        /* start of large tablet styles */
        .tqItemAuctionView {
        }
    }

    @media screen and (max-width: 767px) {
        /* start of medium tablet styles */
        .tqItemAuctionView {
        }
    }

    @media screen and (max-width: 609px) {
        .tqItemAuctionView {
        }
    }

    @media screen and (max-width: 479px) {
        /* start of phone styles */
        .tqItemAuctionView {
        }
    }

</style>

<template>
    <div :class="'tqItemAuctionView ' + backgroundCssClass">
        <if-debug>
            <span class="tqItemAuctionId">
                [A{{auction.id}}]
            </span>
        </if-debug>

        <time-stamp :timeString="auction.createdAtString" cssClass="tqItemAuctionTimeStamp"></time-stamp>

        <span>{{ auction.chatLine.playerName }}</span>

        <span v-if="auction.isBuying">WTB</span>
        <span v-else>WTS</span>

        <span :class="'tqItemAuctionItemName ' + (auction.isKnownItem ? 'tqItemAuctionKnownItem' : 'tqItemAuctionUnknownItem')">
            {{ auction.aliasText }}
        </span>

        <span class="tqItemAuctionPrice">
            <span v-if="this.auction.price != null">{{ formattedPrice }}</span>
        </span>

        <span class="tqItemAuctionPriceDeviation">
            <price-deviation-view v-if="this.auction.price != null" :itemName="auction.itemName" :price="auction.price" :isBuying="auction.isBuying"></price-deviation-view>
        </span>
    </div>
</template>

<script lang="ts">
    import Vue from "vue";

    import Auction from "../interfaces/Auction";

    import TQGlobals from '../classes/TQGlobals';

    import IfDebug from "./IfDebug.vue";
    import TimeStamp from "./TimeStamp.vue";
    import PriceDeviationView from "./PriceDeviationView.vue";


    export default Vue.extend({

        props: {
            auction: {
                type: Object as () => Auction,
                required: true
            }
        },

        computed: {

            urlEncodedItemName: function () {
                return encodeURIComponent(this.auction.itemName);
            },

            formattedPrice: function () {
                if (this.auction.price == null)
                    return null;
                else
                    return TQGlobals.formatNumber(this.auction.price, 0, ".", ",");
            },

            backgroundCssClass: function () {
                return this.auction.isBuying ? "tqItemAuctionWtb" : "tqItemAuctionWts";
            },
        },

        methods: {
        },

        components: {
            IfDebug,
            TimeStamp,
            PriceDeviationView
        }
    });
</script>

