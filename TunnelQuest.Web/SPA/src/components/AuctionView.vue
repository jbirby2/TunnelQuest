
<style>
    .tqAuctionView {
        opacity: 0.7;
        background-color: #000000;
        margin-top: 1px;
    }
</style>

<template>
    <div class="tqAuctionView">
        <div>
            <time-stamp :timeString="auction.updatedAtString"></time-stamp>
            {{ auction.isBuying ? "WTB" : "WTS" }} {{ auction.itemName }} - {{ auction.price }}
        </div>
        <chat-line-view :chatLine="auction.chatLine" :showTimestamp="false" :itemNameLinks="false"></chat-line-view>
    </div>
</template>

<script lang="ts">
    import Vue from "vue";

    import Auction from "../interfaces/Auction";
    import ChatLine from "../interfaces/ChatLine";

    import TimeStamp from "./TimeStamp.vue";
    import ChatLineView from "./ChatLineView.vue";


    export default Vue.extend({
        props: {
            auction: {
                type: Object as () => Auction,
                required: true
            },
            chatLine: {
                type: Object as () => ChatLine,
                required: true
            }
        },
        components: {
            TimeStamp,
            ChatLineView
        }
    });
</script>

