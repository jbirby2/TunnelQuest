
<style>
    .tqAuctionId {
        color: #ffdc87;
    }

    .tqAuctionChatLine {
        font-size: 0.8em;
        font-style: italic;
        color: #808080;
    }


    .tqAuctionView {
        opacity: 0.65;
        background-color: #000000;
    }

    @media screen and (min-width: 992px) {
        /* start of desktop styles */
        .tqAuctionView {
            margin-bottom: 12px;
        }
    }

    @media screen and (max-width: 991px) {
        /* start of large tablet styles */
        .tqAuctionView {
            margin-bottom: 9px;
        }
    }

    @media screen and (max-width: 767px) {
        /* start of medium tablet styles */
        .tqAuctionView {
            margin-bottom: 6px;
        }
    }

    @media screen and (max-width: 479px) {
        /* start of phone styles */
        .tqAuctionView {
            margin-bottom: 3px;
        }
    }

</style>

<template>
    <div class="tqAuctionView">
        <span>
            <if-debug>
                <span class="tqAuctionId">
                    [A{{auction.id}}]
                </span>
            </if-debug>
            <time-stamp :timeString="auction.updatedAtString"></time-stamp>
        </span>
        <span>
            <div>
                {{ auction.isBuying ? "WTB" : "WTS" }} {{ auction.itemName }} - {{ auction.price }}
            </div>
            <div class="tqAuctionChatLine">
                <chat-line-view :chatLine="auction.chatLine" :showTimestamp="false" :itemNameLinks="false" :auctionIdToHighlight="auction.id"></chat-line-view>
            </div>
        </span>
        
    </div>
</template>

<script lang="ts">
    import Vue from "vue";

    import Auction from "../interfaces/Auction";
    import ChatLine from "../interfaces/ChatLine";

    import IfDebug from "./IfDebug.vue";
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
            IfDebug,
            TimeStamp,
            ChatLineView
        }
    });
</script>

