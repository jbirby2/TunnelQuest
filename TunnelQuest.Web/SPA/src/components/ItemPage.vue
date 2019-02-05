

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
    import Vue from "vue";

    import Item from '../interfaces/Item';
        
    import ItemView from "./ItemView.vue";

    import TQGlobals from "../classes/TQGlobals";


    export default Vue.extend({

        name: "ItemPage",

        data: function () {
            return {
                item: null as Item | null
            };
        },

        mounted: function () {
            TQGlobals.init(() => {
                this.item = TQGlobals.items.get(this.$route.params.itemName);
            });
        },

        methods: {
        },

        components: {
            ItemView
        },
    });
</script>
