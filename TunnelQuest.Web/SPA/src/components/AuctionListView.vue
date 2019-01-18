<template>
    <div>
        <div>Auction View:</div>
        <div>
            <auction-view v-for="auction in viewAuctions" :key="auction.id" :auction="auction"></auction-view>
        </div>
    </div>
</template>

<script lang="ts">
    import * as _ from "lodash";
    import Vue from "vue";

    import Auction from "../interfaces/Auction";

    import SlidingList from "../classes/SlidingList";

    import AuctionView from "./AuctionView.vue";

    export default Vue.extend({
        props: {
            auctions: {
                type: Object as () => SlidingList<Auction>,
                required: true
            }
        },
        data: function () {
            return {
            };
        },
        computed: {
            viewAuctions: function () {
                return _.clone(this.auctions.array).reverse();
            }
        },
        created: function () {
        },
        mounted: function () {
        },
        methods: {
        },
        components: {
            AuctionView
        }
    });
</script>

<style>
</style>