

<style>
    .tqFiltersPage {
    }

    .FilterActive {
        background-color: red; /* stub */
    }

</style>


<template>
    <div class="tqFiltersPage">
        <site-header></site-header>

        <div v-if="filters != null">
            <match-any-view :filters="filters" :saveFunction="saveFilterFunction"></match-any-view>
        </div>
    </div>
</template>

<script lang="ts">
    import mixins from 'vue-typed-mixins';

    import Filters from "../interfaces/Filters";

    import TQGlobals from "../classes/TQGlobals";

    import TqPage from "../mixins/TqPage";

    import SiteHeader from "./SiteHeader.vue";
    import MatchAnyView from "./filters/MatchAnyView.vue";


    export default mixins(TqPage).extend({

        name: "Filters",

        data: function () {
            return {
                filters: null as Filters | null
            }
        },

        mounted: function () {
            TQGlobals.init(() => {
                this.filters = TQGlobals.filters;
            });
        },

        methods: {
            saveFilterFunction: function () {
                TQGlobals.saveFilter();
            }
        },

        components: {
            SiteHeader,
            MatchAnyView
        }
    });
</script>
