<style>
</style>

<template>
    <div>
        <div>
            <label>
                <input type="checkbox" ref="includeWtbCheckbox" @change="doSearch" checked />include unknown WTB
            </label>
            &nbsp;
            <label>
                <input type="checkbox" ref="includeWtsCheckbox" @change="doSearch" checked />include unknown WTS
            </label>
        </div>
        <div>
            <input type="text" ref="itemNameBox" @keyup="doSearch" placeholder="Start typing an item name here" />
        </div>
        <div>
            <filter-items-search-result-view v-for="result in results" :key="result.itemName" :result="result" :filter="filter"></filter-items-search-result-view>
        </div>
        <div>
            <div>Items in filter:</div>
            <filter-items-record-view v-for="itemName in filter.settings.itemNames" :filter="filter" :itemName="itemName"></filter-items-record-view>
        </div>
    </div>
</template>

<script lang="ts">
    import * as _ from "lodash";
    import axios from "axios";

    import mixins from 'vue-typed-mixins';

    import Filter from "../../interfaces/Filter";
    import FilterItemSearchResult from "../../interfaces/FilterItemSearchResult";

    import FilterSettingView from "../../mixins/FilterSettingView";

    import TQGlobals from "../../classes/TQGlobals";

    import FilterItemsSearchResultView from "./FilterItemsSearchResultView.vue";
    import FilterItemsRecordView from "./FilterItemsRecordView.vue";


    export default mixins(FilterSettingView).extend({

        props: {
            filter: {
                type: Object as () => Filter,
                required: true
            },
        },

        data: function () {
            return {
                results: [] as Array<FilterItemSearchResult>
            }
        },

        mounted: function () {
            //let itemNameBox = this.$refs.itemNameBox as HTMLInputElement;
            //stub
        },

        methods: {
            doSearch: _.debounce(function (this: any) {
                let startingWith = (this.$refs.itemNameBox as HTMLInputElement).value.trim();

                if (startingWith.length == 0)
                    this.results = [];
                else if (startingWith.length < TQGlobals.settings.minFilterItemNameLength)
                    return;

                let includeBuying = (this.$refs.includeWtbCheckbox as HTMLInputElement).checked;
                let includeSelling = (this.$refs.includeWtsCheckbox as HTMLInputElement).checked;
                let includeUnknownItems = includeBuying || includeSelling;
                
                axios.get('/api/filter_items', {
                    params: {
                        serverCode: TQGlobals.serverCode,
                        startingWith: startingWith,
                        includeUnknownItems: includeUnknownItems,
                        includeBuying: includeBuying,
                        includeSelling: includeSelling
                    }
                })
                .then(response => {
                    this.results = response.data as Array<FilterItemSearchResult>;
                })
                .catch(err => {
                    // stub
                    console.log(err);
                }); // end axios.get(filter_items)

            }, 300), // end onItemNameBoxKeyUp

        }, // end methods

        components: {
            FilterItemsSearchResultView,
            FilterItemsRecordView
        }
    });
</script>
