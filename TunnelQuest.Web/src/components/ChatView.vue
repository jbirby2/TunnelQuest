<template>
    <div>
        <div>Chat View:</div>
        <div><input v-model="filterText" type="text" /></div>
        <div>{{filterText}}</div>

        <sliding-list-view :slidingList="chatLines">
            <chat-line-view v-for="chatLine in viewLines" :key="chatLine.id" :settings="settings" :auctions="auctions" :chatLine="chatLine"></chat-line-view>
        </sliding-list-view>
    </div>
</template>

<script lang="ts">
    import * as _ from "lodash";
    import Vue from "vue";

    import Settings from "../interfaces/Settings";
    import ChatLine from "../interfaces/ChatLine";
    import Auction from "../interfaces/Auction";

    import SlidingList from "../classes/SlidingList";

    import SlidingListView from "./SlidingListView.vue";
    import ChatLineView from "./ChatLineView.vue";

    export default Vue.extend({
        props: {
            settings: {
                type: Object as () => Settings,
                required: true
            },
            auctions: {
                type: Object as () => SlidingList<Auction>,
                required: true
            },
            chatLines: {
                type: Object as () => SlidingList<ChatLine>,
                required: true
            }
        },
        data: function () {
            return {
                filterText: ""
            };
        },
        computed: {
            viewLines: function () {
                return _.clone(this.chatLines.array).reverse();
            }
        },
        created: function () {
        },
        mounted: function () {
        },
        methods: {
            /*
            doesLineMatchFilter: function (line: ChatLine) {
                if (this.filterText == "")
                    return true;
                else {
                    let cleanText = this.filterText.trim();

                    if (line.playerName.toLowerCase().indexOf(cleanText) >= 0)
                        return true;
                    else if (line.text.toLowerCase().indexOf(cleanText) >= 0)
                        return true;
                    else {
                        for (let aucId in line.auctions) {
                            if (line.auctions[aucId].itemName.toLowerCase().indexOf(cleanText) >= 0)
                                return true;
                        }
                    }
                }

                return false;
            }
            */
        },
        components: {
            SlidingListView,
            ChatLineView
        }
    });
</script>

<style>
</style>