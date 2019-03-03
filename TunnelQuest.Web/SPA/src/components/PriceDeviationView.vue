<style>
    .tqPriceDeviation {
        color: red; /* stub */
    }
</style>

<template>
    <span class="tqPriceDeviation">
        PriceDeviationView({{itemName}},{{price}},{{isBuying}})
    </span>
</template>

<script lang="ts">
    import Vue from "vue";

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
                //isDebugMode: false // stub
            };
        },


    });
</script>
