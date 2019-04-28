

<style>
    .tqItemPage {
        width: 100%;
    }

    /* override some of the item css to make it look better on this page */
    .tqItemPage .tqItem {
        /* center the item block on the page */
        margin: auto auto;
    }

    .tqItemPageSection {
        margin-top: 20px !important;
    }

    .tqItemPageAuctionSection {
        margin: auto auto;
        max-width: 600px;
    }

    .tqItemSectionHeader {
        background-color: #55a2c6;
        color: #ffffff;
        font-weight: bold;
        text-align: center;
        font-size: 1.2em;
    }

    .tqItemAuctionWtb {
        background-color: rgba(7,0,99,0.7);
    }

    .tqItemAuctionWts {
        background-color: rgba(0,0,0,0.7);
    }
    
</style>


<template>
    <div class="tqItemPage">
        <site-header></site-header>
        <item-view v-if="item != null && item.isFetched" :item="item" :showAliases="true"></item-view>

        <div class="tqItemPageSection">
            <price-history-view :itemName="$route.params.itemName"></price-history-view>
        </div>
        
        <div class="tqItemPageSection tqItemPageAuctionSection">
            <div class="tqItemSectionHeader">
                Auction History
            </div>
            <div>
                <item-auction-view v-for="auction in auctions" :key="auction.id" :auction="auction" :cssClass="auction.isBuying ? 'tqItemAuctionWtb' : 'tqItemAuctionWts'"></item-auction-view>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
    import mixins from 'vue-typed-mixins';

    import Item from '../interfaces/Item';
    import Auction from '../interfaces/Auction';
    import ChatLine from "../interfaces/ChatLine";

    import TQGlobals from "../classes/TQGlobals";

    import TqPage from "../mixins/TqPage";

    import SiteHeader from "./SiteHeader.vue";
    import ItemView from "./ItemView.vue";
    import PriceHistoryView from "./PriceHistoryView.vue";
    import ItemAuctionView from "./ItemAuctionView.vue";


    export default mixins(TqPage).extend({

        name: "ItemPage",

        data: function () {
            return {
                item: null as Item | null,
                auctions: new Array<Auction>(),
            };
        },

        beforeDestroy: function () {
            this.auctions = new Array<Auction>();
        },

        watch: {
            // so that we can navigate from one item page to another, and everything will update
            $route (to, from) {
                this.item = TQGlobals.items.queue(this.$route.params.itemName);
                TQGlobals.items.fetchQueuedItems();
                this.loadLatestFilteredChatLines();
            }
        },

        methods: {
            // inherited from TqPage
            onInitialized: function () {
                let aliasedItemName = TQGlobals.resolveItemAlias(this.$route.params.itemName);
                if (aliasedItemName != this.$route.params.itemName) {
                    this.$router.replace("/item/" + encodeURIComponent(aliasedItemName));
                }
                else {
                    this.item = TQGlobals.items.queue(this.$route.params.itemName);
                    TQGlobals.items.fetchQueuedItems();
                    this.loadLatestFilteredChatLines();
                }
            },

            // inherited from TqPage
            getChatFilterSettings: function () {
                return {
                    isPermanent: true,
                    itemNames: [ (this.item as Item).itemName ]
                };
            },

            // inherited from TqPage
            beforeChatLinesLoaded: function (newLines: Array<ChatLine>) {
            },

            // inherited from TqPage
            onChatLinesLoaded: function (newChatLines: Array<ChatLine>) {
                for (let chatLine of newChatLines) {
                    for (let auctionId in chatLine.auctions) {
                        let auction = chatLine.auctions[auctionId];

                        if (auction.passesFilter)
                            this.auctions.push(auction);
                    }
                }

                this.auctions.sort(function (a: Auction, b: Auction) {
                    // sort descending createdAtString
                    if (a.createdAtString < b.createdAtString)
                        return 1;
                    else if (a.createdAtString > b.createdAtString)
                        return -1;
                    else {
                        // sort descending id
                        if (a.id < b.id)
                            return 1;
                        else if (a.id > b.id)
                            return -1;
                        else
                            return 0;
                    }
                });
            }
        },

        components: {
            SiteHeader,
            ItemView,
            PriceHistoryView,
            ItemAuctionView
        },
    });
</script>
