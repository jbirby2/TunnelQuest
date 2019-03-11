

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
        <item-view v-if="item != null && item.isFetched" :item="item"></item-view>

        <div class="tqItemPageSection">
            <price-history-view :itemName="$route.params.itemName"></price-history-view>
        </div>
        
        <div class="tqItemPageSection tqItemPageAuctionSection">
            <div class="tqItemSectionHeader">
                Auction History
            </div>
            <div>
                <chat-line-view v-for="auction in auctions.array" :key="auction.chatLine.id" :chatLine="auction.chatLine" :showTimestamp="true" :itemNameLinks="true" :itemNameToHighlight="auction.itemName" :cssClass="auction.isBuying ? 'tqAuctionHouseAuctionWtb' : 'tqAuctionHouseAuctionWts'"></chat-line-view>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
    import axios from "axios";
    import mixins from 'vue-typed-mixins';

    import Item from '../interfaces/Item';
    import Auction from '../interfaces/Auction';
    import LinesAndAuctions from "../interfaces/LinesAndAuctions";

    import TQGlobals from "../classes/TQGlobals";
    import SlidingList from "../classes/SlidingList";

    import TqPage from "../mixins/TqPage";

    import SiteHeader from "./SiteHeader.vue";
    import ItemView from "./ItemView.vue";
    import PriceHistoryView from "./PriceHistoryView.vue";
    import ChatLineView from "./ChatLineView.vue";


    export default mixins(TqPage).extend({

        name: "ItemPage",

        data: function () {
            return {
                // STUB hard-coded
                serverCode: "BLUE",

                item: null as Item | null,

                auctions: new SlidingList<Auction>(function (a: Auction, b: Auction) {
                    // sort descending updatedAtString
                    if (a.updatedAtString < b.updatedAtString)
                        return 1;
                    else if (a.updatedAtString > b.updatedAtString)
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
                })
            };
        },

        mounted: function () {
            window.addEventListener("scroll", this.onScroll);

            TQGlobals.init(() => {
                this.item = TQGlobals.items.get(this.$route.params.itemName);
                this.getLatestContent();
            });
        },

        watch: {
            $route (to, from) {
                this.item = TQGlobals.items.get(this.$route.params.itemName);
                this.getLatestContent();
            }
        },

        beforeDestroy: function () {
            window.removeEventListener("scroll", this.onScroll);
        },

        methods: {
            // inherited from TqPage
            getLatestContent: function () {
                this.auctions.clear();

                axios.post('/api/auction_query', {
                    serverCode: this.serverCode,
                    itemName: this.$route.params.itemName,
                    includeChatLine: true
                })
                .then(response => {
                    let itemAuctions = response.data as LinesAndAuctions;
                    this.onNewContent(itemAuctions, true);
                })
                .catch(err => {
                    // stub
                    console.log(err);
                });
            },

            // inherited from TqPage
            getEarlierContent: function () {
                let maxId: number | null = null;
                if (this.auctions.array.length > 0)
                    maxId = this.auctions.array[this.auctions.array.length - 1].id - 1;

                axios.post('/api/auction_query', {
                    serverCode: this.serverCode,
                    itemName: this.$route.params.itemName,
                    maximumId: maxId,
                    includeChatLine: true
                })
                .then(response => {
                    let itemAuctions = response.data as LinesAndAuctions;
                    this.onNewContent(itemAuctions, false);
                })
                .catch(err => {
                    // stub
                    console.log(err);
                });
            },


            // inherited from TqPage
            onFilteredContent: function (itemAuctions: LinesAndAuctions, enforceMaxSize: boolean) {
                for (let auctionId in itemAuctions.auctions) {
                    let auction = itemAuctions.auctions[auctionId];
                    this.auctions.add(auction);
                }

                if (enforceMaxSize)
                    this.auctions.enforceMaxSize();

                this.auctions.sort();
            }
        },

        components: {
            SiteHeader,
            ItemView,
            PriceHistoryView,
            ChatLineView
        },
    });
</script>
