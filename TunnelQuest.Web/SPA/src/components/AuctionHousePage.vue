
<style>
    .tqAuctionHousePageListDivider {
        background-color: #55a2c6;
        color: #ffffff;
        font-weight: bold;
        margin-bottom: 10px;
        text-align: center;
        font-size: 1.2em;
    }

    .tqAuctionHousePageList {
        
    }

</style>

<template>
    <div>
        <div class="tqAuctionHousePageListDivider">Recent auctions:</div>
        <div class="tqAuctionHousePageList">
            <transition-group :name="transitionName">
                <auction-house-auction-view v-for="auction in recentlyUpdatedAuctions" :key="auction.id" :auction="auction"></auction-house-auction-view>
            </transition-group>
        </div>
        <div class="tqAuctionHousePageListDivider">Older auctions:</div>
        <div class="tqAuctionHousePageList">
            <transition-group :name="transitionName">
                <auction-house-auction-view v-for="auction in notRecentlyUpdatedAuctions" :key="auction.id" :auction="auction"></auction-house-auction-view>
            </transition-group>
        </div>
    </div>
</template>

<script lang="ts">
    import mixins from 'vue-typed-mixins';

    import Auction from "../interfaces/Auction";

    import LiveAuctionsPage from "../mixins/LiveAuctionsPage";

    import AuctionHouseAuctionView from "./AuctionHouseAuctionView.vue";

    import TQGlobals from "../classes/TQGlobals";


    export default mixins(LiveAuctionsPage).extend({

        name: "AuctionHousePage",

        components: {
            AuctionHouseAuctionView
        },

        methods: {
            // inherited from LiveAuctionsPage
            onNewAuction: function (auction: Auction) {
                
            },
        }

    });
</script>
