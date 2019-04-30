

<style>
    .tqFilterPageSettings {
        margin: auto auto;
        max-width: 600px;
        
    }

    .tqFilterPageSettings fieldset {
        background-color: rgba(0,0,0,0.7);
        color: #ffffff;
        font-size: 1.2em;
        margin-bottom: 20px;
    }

</style>

<template>
    <div>
        <site-header></site-header>

        <div v-if="filter != null" class="tqFilterPageSettings">
            <fieldset>
                <legend>Filter Name</legend>
                <input type="text" v-model="filter.name" @change="onFilterNameChanged" />
            </fieldset>

            <fieldset>
                <legend>Items</legend>
                <filter-items-view :filter="filter"></filter-items-view>
            </fieldset>
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

                if (this.filter != null && TQGlobals.filterManager.selectedFilter != this.filter)
                    TQGlobals.filterManager.setSelectedFilter(this.filter);
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
