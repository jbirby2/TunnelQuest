<style>
    .tqPriceHistory {
        display: block;
        background-color: rgba(0,0,0,0.7);
        color: #e0e0e0;
        position: relative;
        margin: auto auto;
        margin-top: 20px;
    }

    .tqPriceHistory > table {
        width: 100%;
    }

    .tqPriceHistory > table > thead > tr {
        background-color: #55a2c6;
        color: #ffffff;
        font-weight: bold;
        text-align: center;
        font-size: 1.2em;
    }

    .tqPriceHistory > table > tbody > tr:nth-child(even) {
        background-color: rgba(96, 96, 96, 0.7);
    }

    .tqPriceHistory > table > tbody > tr > td:nth-child(2) {
        text-align: center;
        font-weight: bold;
    }

    .tqPriceHistoryTimestamp {
        margin-left: 5px;
        font-size: 1.0em;
        font-style: italic;
    }

    @media screen and (min-width: 952px) {
        /* start of desktop styles */
        .tqPriceHistory {
            width: 400px;
        }
    }

    @media screen and (max-width: 951px) {
        /* start of large tablet styles */
        .tqPriceHistory {
            width: 350px;
        }
    }

    @media screen and (max-width: 767px) {
        /* start of medium tablet styles */
        .tqPriceHistory {
            width: 280px;
        }
    }

    @media screen and (max-width: 609px) {
        .tqPriceHistory {
            width: 225px;
        }
    }

    @media screen and (max-width: 479px) {
        /* start of phone styles */
        .tqPriceHistory {
            width: 100%;
        }
    }
</style>

<template>
    <div v-if="priceHistory" class="tqPriceHistory">
        <table>
            <thead>
                <tr>
                    <td colspan="2">
                        <span>Price History</span>
                        <span class="tqPriceHistoryTimestamp">
                            <span>updated </span>
                            <time-stamp :timeString="priceHistory.updatedAtString"></time-stamp>
                        </span>
                    </td>
                </tr>
            </thead>
            <tbody>
                <tr v-if="priceHistory.oneMonthMedian">
                    <td>1 month median price</td>
                    <td>{{formatPrice(priceHistory.oneMonthMedian)}}</td>
                </tr>
                <tr v-if="priceHistory.threeMonthMedian">
                    <td>3 month median price</td>
                    <td>{{formatPrice(priceHistory.threeMonthMedian)}}</td>
                </tr>
                <tr v-if="priceHistory.sixMonthMedian">
                    <td>6 month median price</td>
                    <td>{{formatPrice(priceHistory.sixMonthMedian)}}</td>
                </tr>
                <tr v-if="priceHistory.twelveMonthMedian">
                    <td>12 month median price</td>
                    <td>{{formatPrice(priceHistory.twelveMonthMedian)}}</td>
                </tr>
                <tr>
                    <td>Lifetime median price</td>
                    <td>{{formatPrice(priceHistory.lifetimeMedian)}}</td>
                </tr>
            </tbody>
        </table>
    </div>
</template>

<script lang="ts">
    import Vue from "vue";

    import TQGlobals from '../classes/TQGlobals';
    import PriceHistory from "../interfaces/PriceHistory";

    import TimeStamp from "./TimeStamp.vue";


    export default Vue.extend({

        props: {
            itemName: {
                type: String,
                required: true
            }
        },

        data: function () {
            return {
                priceHistory: null as PriceHistory | null
            };
        },

        mounted: function () {
            TQGlobals.init(this.onInit);
        },

        methods: {
            onInit: function () {
                this.priceHistory = TQGlobals.priceHistories.get(this.itemName, true);
            },

            formatPrice: function (price:number) {
                return TQGlobals.formatNumber(price, 0, ".", ",");
            }
        },

        components: {
            TimeStamp
        }

    });
</script>
