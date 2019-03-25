
<style>
    .tqAuctionHouseAuctionView {
        display: table-row;
    }

    .tqAuctionHouseAuctionView > span {
        display: table-cell;
        text-align: left;
        padding-left: 5px;
    }

    .tqAuctionHouseAuctionId {
        white-space: nowrap;
        color: #ffdc87;
    }

    .tqAuctionHouseAuctionTimeStamp {
        display: table-cell;
        font-family: Courier New, Courier, monospace;
        color: #c9c9c9;
        font-size: 0.8em;
        white-space: nowrap;
    }

    .tqAuctionHouseAuctionWtb {
        background-color: rgba(7,0,99,0.7);
    }

    .tqAuctionHouseAuctionWts {
        background-color: rgba(0,0,0,0.7);
    }

    .tqAuctionHouseAuctionItemName {
        width: 100%;
    }

    .tqAuctionHouseAuctionKnownItemLink {
        color: #e049ff;
        text-decoration: none;
    }

    .tqAuctionHouseAuctionUnknownItemLink {
        color: #f7d8ff;
        text-decoration: none;
    }

    .tqAuctionHouseAuctionPrice {
        text-align: right !important;
    }

    .tqAuctionHouseAuctionPriceDeviation {
        text-align: center !important;
        padding-right: 5px;
    }

    @media screen and (min-width: 952px) {
        /* start of desktop styles */
        .tqAuctionHouseAuctionView {
        }
    }

    @media screen and (max-width: 951px) {
        /* start of large tablet styles */
        .tqAuctionHouseAuctionView {
        }
    }

    @media screen and (max-width: 767px) {
        /* start of medium tablet styles */
        .tqAuctionHouseAuctionView {
        }
    }

    @media screen and (max-width: 609px) {
        .tqAuctionHouseAuctionView {
        }
    }

    @media screen and (max-width: 479px) {
        /* start of phone styles */
        .tqAuctionHouseAuctionView {
        }
    }

</style>

<template>
    <div :class="'tqAuctionHouseAuctionView ' + backgroundCssClass">
        <if-debug>
            <span class="tqAuctionHouseAuctionId">
                [A{{auction.id}}]
            </span>
        </if-debug>

        <time-stamp :timeString="auction.updatedAtString" cssClass="tqAuctionHouseAuctionTimeStamp"></time-stamp>

        <span>{{ auction.chatLine.playerName }}</span>

        <span v-if="auction.isBuying">WTB</span>
        <span v-else>WTS</span>



        <span class="tqAuctionHouseAuctionItemName">
            <a :class="(auction.isKnownItem ? 'tqAuctionHouseAuctionKnownItemLink' : 'tqAuctionHouseAuctionUnknownItemLink')" :href="'/item/' + this.urlEncodedItemName" @click="onItemNameClick">{{auction.itemName}}</a>
        </span>



        <span class="tqAuctionHouseAuctionPrice">
            <span v-if="this.auction.price != null">{{ formattedPrice }}</span>
        </span>

        <span class="tqAuctionHouseAuctionPriceDeviation">
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
                return this.auction.isBuying ? "tqAuctionHouseAuctionWtb" : "tqAuctionHouseAuctionWts";
            },
        },

        methods: {
            onItemNameClick: function (e: any) {
                e.preventDefault();
                this.$router.push("/item/" + this.urlEncodedItemName);
            }
        },

        components: {
            IfDebug,
            TimeStamp,
            PriceDeviationView
        }
    });
</script>

