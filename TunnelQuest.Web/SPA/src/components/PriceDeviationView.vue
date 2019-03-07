<style>
    .tqPriceDeviationGood {
        color: #00a819;
    }
    .tqPriceDeviationEqual {
        color: #cec400;
    }
    .tqPriceDeviationBad {
        color: #ff0000;
    }
</style>

<template>
    <span v-if="itemName && price && medianPrice" class="tqPriceDeviation">
        <span v-if="price == medianPrice" class="tqPriceDeviationEqual">
            =
        </span>
        <span v-else-if="price < medianPrice" :class="isBuying ? 'tqPriceDeviationBad' : 'tqPriceDeviationGood'">
            &#9660;{{medianPriceDifference}}
        </span>
        <span v-else :class="isBuying ? 'tqPriceDeviationGood' : 'tqPriceDeviationBad'">
            &#9650;{{medianPriceDifference}}
        </span>
    </span>
</template>

<script lang="ts">
    import Vue from "vue";

    import TQGlobals from '../classes/TQGlobals';
    import PriceHistory from "../interfaces/PriceHistory";


    export default Vue.extend({

        props: {
            itemName: {
                type: String,
                required: true
            },
            price: {
                type: Number,
                required: true
            },
            isBuying: {
                type: Boolean,
                required: true
            }
        },

        data: function () {
            return {
                priceHistory: null as PriceHistory | null
            };
        },

        mounted: function () {
            this.priceHistory = TQGlobals.priceHistories.get(this.itemName, false);
        },

        computed: {
            medianPrice: function () {
                if (this.priceHistory) {
                    if (this.priceHistory.threeMonthMedian)
                        return this.priceHistory.threeMonthMedian;
                    else
                        return this.priceHistory.lifetimeMedian;
                }
                else
                    return null;
            },

            medianPriceDifference: function () {
                if (this.medianPrice)
                    return Math.abs(this.price - this.medianPrice);
                else
                    return null;
            }
        },


    });
</script>
