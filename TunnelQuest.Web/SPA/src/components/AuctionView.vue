
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
        
    }

    @media screen and (min-width: 992px) {
        /* start of desktop styles */
        .tqAuctionView {
        }
    }

    @media screen and (max-width: 991px) {
        /* start of large tablet styles */
        .tqAuctionView {
        }
    }

    @media screen and (max-width: 767px) {
        /* start of medium tablet styles */
        .tqAuctionView {
        }
    }

    @media screen and (max-width: 479px) {
        /* start of phone styles */
        .tqAuctionView {
        }
    }

</style>

<template>
    <tr class="tqAuctionView">
        <td>
            <if-debug>
                <span class="tqAuctionId">
                    [A{{auction.id}}]
                </span>
            </if-debug>
            <time-stamp :timeString="auction.updatedAtString"></time-stamp>
        </td>
        <td>
            {{ auction.isBuying ? "WTB" : "WTS" }}
        </td>
        <td>
            <auction-item-view :item="auction.item"></auction-item-view>
            <div class="tqAuctionChatLine">
                <chat-line-view :chatLine="auction.chatLine" :showTimestamp="false" :itemNameLinks="false" :auctionIdToHighlight="auction.id"></chat-line-view>
            </div>
        </td>        
    </tr>
</template>

<script lang="ts">
    import Vue from "vue";

    import Auction from "../interfaces/Auction";
    import ChatLine from "../interfaces/ChatLine";

    import IfDebug from "./IfDebug.vue";
    import TimeStamp from "./TimeStamp.vue";

    import AuctionItemView from "./AuctionItemView.vue";
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
            AuctionItemView,
            ChatLineView
        }
    });
</script>

