

<style>
    .tqItemPage {
        background-color: red;
        width: 100%;
    }

        /* override some of the item css to make it look better on this page */
        .tqItemPage .tqItem {
            /* center the item block on the page */
            margin: auto auto;
        }
</style>


<template>
    <div class="tqItemPage">
        <item-view v-if="item != null && item.isFetched" :item="item"></item-view>
    </div>
</template>

<script lang="ts">
    import mixins from 'vue-typed-mixins';

    import LinesAndAuctions from "../interfaces/LinesAndAuctions";
    import Item from '../interfaces/Item';

    import LivePage from "../mixins/LivePage";
    
    import ItemView from "./ItemView.vue";

    import TQGlobals from "../classes/TQGlobals";


    export default mixins(LivePage).extend({

        name: "ItemPage",

        data: function () {
            return {
                item: null as Item | null
            };
        },

        methods: {

            // inherited from LivePage
            onInitialized: function () {
                this.item = TQGlobals.items.get(this.$route.params.itemName);
            },

            // inherited from LivePage
            getLatestContent: function () {
            },

            // inherited from LivePage
            getEarlierContent: function () {
            },

            // inherited from LivePage
            onNewContent: function (newContent: LinesAndAuctions, enforceMaxSize: boolean) {
            },

            // inherited from LivePage
            onDestroying: function () {
            },
        },
        components: {
            ItemView
        },
    });
</script>
