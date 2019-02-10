
<style>
    .tqNewspaperPageListDivider {
        background-color: #55a2c6;
        color: #ffffff;
        font-weight: bold;
        margin-bottom: 10px;
        text-align: center;
        font-size: 1.2em;
    }

    /* the "> span" is because the component we want to transition (the AuctionHouseItemView) is nested inside a <transition-group> element */
    .tqNewspaperPageList > span {
        width: 100%;
        height: 100%;
        display: flex;
        flex-direction: row;
        flex-wrap: wrap;
        justify-content: center;
    }

</style>

<template>
    <div>
        <site-header>
            <span>
                <router-link to="/">&lt;&lt; to Main Menu</router-link>
            </span>
            <connection-status-view :connection="connection"></connection-status-view>
        </site-header>

        <div class="tqNewspaperPageListDivider">Recent auctions:</div>
        <div class="tqNewspaperPageList">
            <transition-group :name="transitionName">
                <newspaper-auction-view v-for="auction in recentlyUpdatedAuctions" :key="auction.id" :auction="auction"></newspaper-auction-view>
            </transition-group>
        </div>
        <div class="tqNewspaperPageListDivider">Older auctions:</div>
        <div class="tqNewspaperPageList">
            <transition-group :name="transitionName">
                <newspaper-auction-view v-for="auction in notRecentlyUpdatedAuctions" :key="auction.id" :auction="auction"></newspaper-auction-view>
            </transition-group>
        </div>
    </div>
</template>

<script lang="ts">
    import mixins from 'vue-typed-mixins';

    import Auction from "../interfaces/Auction";

    import LiveAuctionsPage from "../mixins/LiveAuctionsPage";

    import SiteHeader from "./SiteHeader.vue";
    import ConnectionStatusView from "./ConnectionStatusView.vue";
    import NewspaperAuctionView from "./NewspaperAuctionView.vue";

    import TQGlobals from "../classes/TQGlobals";


    export default mixins(LiveAuctionsPage).extend({

        name: "NewspaperPage",

        methods: {
            // inherited from LiveAuctionsPage
            onNewAuction: function (auction: Auction) {
                // false to prevent it from immediately making an ajax call to fetch this one item
                auction.item = TQGlobals.items.get(auction.itemName, false);

            },
        },

        components: {
            SiteHeader,
            ConnectionStatusView,
            NewspaperAuctionView
        }
    });
</script>
