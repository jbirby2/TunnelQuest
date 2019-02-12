

<style>
    .tqItemPage {
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
        <site-header></site-header>
        <item-view v-if="item != null && item.isFetched" :item="item"></item-view>
    </div>
</template>

<script lang="ts">
    import mixins from 'vue-typed-mixins';

    import Item from '../interfaces/Item';

    import SiteHeader from "./SiteHeader.vue";
    import ItemView from "./ItemView.vue";

    import TQGlobals from "../classes/TQGlobals";

    import TqPage from "../mixins/TqPage";


    export default mixins(TqPage).extend({

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
            SiteHeader,
            ItemView
        },
    });
</script>
