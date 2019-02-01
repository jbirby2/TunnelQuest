
<style>
    .tqAuctionHouseAuctionView {
        opacity: 0.65;
        background-color: #000000;
        margin: auto auto;
        display: block;
        max-width: 1000px;
    }

    .tqAuctionHouseAuctionId {
        white-space: nowrap;
        color: #ffdc87;
    }

    .tqAuctionHouseAuctionTimeStamp {
        font-family: Courier New, Courier, monospace;
        color: #c9c9c9;
        font-size: 0.8em;
    }

    .tqAuctionHouseAuctionWtb {
        background-color: #007a18;
    }

    .tqAuctionHouseAuctionWts {
        background-color: #001977;
    }

    .tqAuctionHouseAuctionItemName {
        color: #e049ff;
        font-weight: bold;
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
    <span :class="'tqAuctionHouseAuctionView ' + backgroundCssClass">
        <if-debug>
            <span class="tqAuctionHouseAuctionId">
                [A{{auction.id}}]
            </span>
        </if-debug>
        <time-stamp :timeString="auction.updatedAtString" cssClass="tqAuctionHouseAuctionTimeStamp"></time-stamp>
        <span>{{ auction.chatLine.playerName }}</span>
        <span v-if="auction.isBuying" class="tqAuctionHouseAuctionWtb">WTB</span>
        <span v-if="!auction.isBuying" class="tqAuctionHouseAuctionWts">WTS</span>
        <span class="tqAuctionHouseAuctionItemName">{{auction.chatLine.auctions[auction.id].itemName}}</span>
        <span>{{ formattedPrice }}</span>
    </span>
</template>

<script lang="ts">
    import Vue from "vue";

    import Auction from "../interfaces/Auction";
    import ChatLine from "../interfaces/ChatLine";

    import TQGlobals from '../classes/TQGlobals';

    import IfDebug from "./IfDebug.vue";
    import TimeStamp from "./TimeStamp.vue";


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
        },

        components: {
            IfDebug,
            TimeStamp
        }
    });
</script>

