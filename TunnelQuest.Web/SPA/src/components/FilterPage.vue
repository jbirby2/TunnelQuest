

<style>
    .tqFilterPage {
        margin: auto auto;
        max-width: 600px;
        background-color: rgba(0,0,0,0.7);
    }

    .tqFilterPageHeader {
        background-color: #55a2c6;
        color: #ffffff;
        font-weight: bold;
        font-size: 1.2em;
        padding-left: 3px;
    }

    

</style>


<template>
    <div class="tqFilterPage">
        <site-header></site-header>

        <div v-if="filter != null">
            <div class="tqFilterPageHeader">Filter Name</div>
            <div>
                <input type="text" v-model="filter.name" @change="onFilterNameChanged" />
            </div>

            <div class="tqFilterPageHeader">Specific Items</div>
            <filter-items-view :filter="filter"></filter-items-view>
        </div>
    </div>
</template>

<script lang="ts">
    import mixins from 'vue-typed-mixins';

    import Filter from "../interfaces/Filter";

    import TQGlobals from "../classes/TQGlobals";

    import TqPage from "../mixins/TqPage";

    import SiteHeader from "./SiteHeader.vue";
    import FilterItemsView from "./filterSettings/FilterItemsView.vue";


    export default mixins(TqPage).extend({

        name: "FilterPage",

        data: function () {
            return {
                filter: null as Filter | null
            }
        },

        mounted: function () {
            TQGlobals.init(() => {
                this.filter = TQGlobals.filterManager.get(this.$route.params.id);
            });
        },

        methods: {
            onFilterNameChanged: function () {
                TQGlobals.filterManager.saveUserFilters();
            }
        },

        components: {
            SiteHeader,
            FilterItemsView
        }
    });
</script>
