
<style>
    .tqAuctionHousePageListDivider {
        background-color: #4485a3;
        color: #ffffff;
        font-weight: bold;
        margin-bottom: 10px;
        text-align: center;
        font-size: 1.2em;
    }

    .tqAuctionHousePageList {
        margin: auto auto;
        max-width: 600px;
    }

    .tqAuctionHousePageTransition {
        display: table;
        empty-cells: show;
        width: 100%;
    }

</style>

<template>
    <div>
        <site-header>
            <connection-status-view :connection="connection"></connection-status-view>
        </site-header>

        <filter-manager-view></filter-manager-view>

        <div class="tqAuctionHousePageListDivider">Recent auctions:</div>
        <div class="tqAuctionHousePageList">
            <transition-group :name="transitionName" class="tqAuctionHousePageTransition">
                <auction-house-auction-view v-for="auction in recentlyUpdatedAuctions" :key="auction.id" :auction="auction"></auction-house-auction-view>
            </transition-group>
        </div>
        <div class="tqAuctionHousePageListDivider">Older auctions:</div>
        <div class="tqAuctionHousePageList">
            <transition-group :name="transitionName" class="tqAuctionHousePageTransition">
                <auction-house-auction-view v-for="auction in notRecentlyUpdatedAuctions" :key="auction.id" :auction="auction"></auction-house-auction-view>
            </transition-group>
        </div>
    </div>
</template>

<script lang="ts">
    import mixins from 'vue-typed-mixins';

    import Auction from "../interfaces/Auction";

    import LiveAuctionsPage from "../mixins/LiveAuctionsPage";

    import SiteHeader from "./SiteHeader.vue";
    import FilterManagerView from "./FilterManagerView.vue";
    import ConnectionStatusView from "./ConnectionStatusView.vue";
    import AuctionHouseAuctionView from "./AuctionHouseAuctionView.vue";


    export default mixins(LiveAuctionsPage).extend({

        name: "AuctionHousePage",

        methods: {
            // inherited from LiveAuctionsPage
            beforeAuctionsLoaded: function (newAuctions: Array<Auction>) {
            },

            // inherited from LiveAuctionsPage
            onAuctionsLoaded: function (newAuctions: Array<Auction>) {
            },

            // inherited from LiveAuctionsPage
            onAuctionsUnloaded: function (removedAuctions: Array<Auction>) {
            },
        },

        components: {
            SiteHeader,
            FilterManagerView,
            ConnectionStatusView,
            AuctionHouseAuctionView
        }
    });
</script>
