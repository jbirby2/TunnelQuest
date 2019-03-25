<style>
</style>

<template>
    <span v-if="filters != null">
        <router-link to="/filters">{{linkText}}</router-link>
    </span>
</template>

<script lang="ts">
    import Vue from "vue";

    import Filters from "../interfaces/Filters";

    import TQGlobals from "../classes/TQGlobals";


    export default Vue.extend({
        
        data: function () {
            return {
                filters: null as Filters | null
            }
        },

        computed: {
            linkText: function () {
                let linkText = "Set Filters";

                if (this.filters != null) {
                    let numActiveFilters = 0;

                    if (this.filters.itemNames.length > 0)
                        numActiveFilters++;
                    if (this.filters.playerName != null)
                        numActiveFilters++;
                    if (this.filters.isBuying != null)
                        numActiveFilters++;
                    if (this.filters.minPrice != null)
                        numActiveFilters++;
                    if (this.filters.maxPrice != null)
                        numActiveFilters++;
                    if (this.filters.minGoodPriceDeviation != null)
                        numActiveFilters++;
                    if (this.filters.maxBadPriceDeviation != null)
                        numActiveFilters++;

                    // STUB add more filters

                    if (numActiveFilters > 0)
                        linkText += " (" + numActiveFilters.toString() + " active)";
                }

                return linkText + " >>";
            }
        },

        mounted: function () {
            TQGlobals.init(() => {
                this.filters = TQGlobals.filters;
            });
        }
    });
</script>
