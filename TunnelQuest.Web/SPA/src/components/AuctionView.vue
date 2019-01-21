<template>
    <div class="tqAuctionView">
        <div>
            [{{ auction.id }}] <time-stamp :timeString="auction.updatedAtString"></time-stamp>
            {{ auction.isBuying ? "WTB" : "WTS" }} {{ auction.itemName }} - {{ auction.price }}
        </div>
        <chat-line-view :auctions="auctions" :chatLine="chatLine" :showTimestamp="false" :itemNameLinks="false"></chat-line-view>
    </div>
</template>

<script lang="ts">
    import Vue from "vue";

    import Auction from "../interfaces/Auction";
    import ChatLine from "../interfaces/ChatLine";

    import TimeStamp from "./TimeStamp.vue";
    import ChatLineView from "./ChatLineView.vue";

    import SlidingList from "../classes/SlidingList";

    export default Vue.extend({
        props: {
            auctions: {
                type: Object as () => SlidingList<Auction>,
                required: true
            },
            auction: {
                type: Object as () => Auction,
                required: true
            },
            chatLine: {
                type: Object as () => ChatLine,
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
            TimeStamp,
            ChatLineView
        }
    });
</script>

<style>
    .tqAuctionView {
        opacity: 0.85;
        background-color: #595959;
        margin-top: 1px;
    }
</style>