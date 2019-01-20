<template>
    <div class="auctionView">
        [{{ auction.id }}] <time-stamp :time="auction.updatedAtString"></time-stamp>
        {{ auction.isBuying ? "WTB" : "WTS" }} {{ auction.itemName }} - {{ auction.price }}
    </div>
</template>

<script lang="ts">
    import Vue from "vue";

    import Auction from "../interfaces/Auction";

    import TimeStamp from "./TimeStamp.vue";

    export default Vue.extend({
        props: {
            auction: {
                type: Object as () => Auction,
                required: true
            }
        },
        data: function () {
            return {
            };
        },
        created: function () {
        },
        mounted: function () {
        },
        components: {
            TimeStamp
        }
    });
</script>

<style>
</style>