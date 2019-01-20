<template>
    <div>
        <div>Auction View:</div>
        <div>
            <auction-view v-for="auction in viewAuctions" :key="auction.id" :auction="auction"></auction-view>
        </div>
    </div>
</template>

<script lang="ts">
    import axios from "axios";
    import mixins from 'vue-typed-mixins';
    import * as _ from "lodash";

    import LinesAndAuctions from "../interfaces/LinesAndAuctions";

    import LiveView from "../mixins/LiveView";

    import TQGlobals from "../classes/TQGlobals";

    import AuctionView from "./AuctionView.vue";


    export default mixins(LiveView).extend({
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

            // inherited from LiveView
            getLatestContent: function () {
                let minId: number | null = null;
                if (this.auctions.array.length > 0)
                    minId = this.auctions.array[this.auctions.array.length - 1].id + 1;

                axios.get('/api/auctions?serverCode=' + TQGlobals.serverCode + "&minId=" + (minId == null ? "" : minId.toString()))
                    .then(response => {
                        let result = response.data as LinesAndAuctions;
                        this.onNewContent(result);
                    })
                    .catch(err => {
                        // stub
                        console.log(err);
                    }); // end axios.get(chat_lines)
            },

            // inherited from LiveView
            getEarlierContent: function () {
                // stub
            },

        },
        components: {
            AuctionView
        }
    });
</script>

<style>
</style>