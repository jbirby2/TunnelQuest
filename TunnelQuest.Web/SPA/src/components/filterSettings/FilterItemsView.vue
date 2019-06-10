<style>
    .tqFilterItemsView {
    }

    .tqFilterItemsViewSearchPanel {
        display: table-row;
    }

    .tqFilterItemsViewSearchPanel > span {
        display: table-cell;
    }

    .tqFilterItemsViewSearchPanel > span:nth-child(1) {
        width: 100%;
    }

    .tqFilterItemsViewSearchOptions {
        color: #999999;
    }

    .tqFilterItemsViewListSeparator {
        background-color: rgba(120, 120, 120, 0.7);
        color: #efefef;
    }

</style>

<template>
    <div>
        <div>
            <input id="tqFilterByNameRadio" type="radio" value="name" v-model="filter.settings.items.queryType" @change="onFilterChanged" />
            <label for="tqFilterByNameRadio">
                Filter by Name
            </label>
            <input id="tqFilterByStatsRadio" type="radio" value="stats" v-model="filter.settings.items.queryType" @change="onFilterChanged" />
            <label for="tqFilterByStatsRadio">
                Filter by Stats
            </label>
        </div>
        <div v-if="filter.settings.items.queryType == 'name'">
            <div class="tqFilterItemsViewSearchPanel">
                <span>
                    <input type="text" v-model="searchText" @keyup="doSearch" placeholder="Start typing an item name here" />
                </span>
                <span>
                    <input type="button" @click="onToggleSearchOptionsClicked" :value="showSearchOptions ? 'Hide Options' : 'Show Options'" />
                </span>
            </div>
            <div v-if="showSearchOptions" class="tqFilterItemsViewSearchOptions">
                <label>
                    <input type="checkbox" v-model="includeUnknownWtb" @change="doSearch" />include unknown WTB
                </label>
                &nbsp;
                <label>
                    <input type="checkbox" v-model="includeUnknownWts" @change="doSearch" />include unknown WTS
                </label>
            </div>
            <div>
                <div v-if="lastSearchedText.length > 0 || results.length > 0" class="tqFilterItemsViewListSeparator">
                    Search Results
                </div>
                <div v-if="results.length == 0 && lastSearchedText.length > 0">
                    There are no item names starting with "{{lastSearchedText}}"
                </div>
                <div v-else>
                    <filter-items-search-result-view v-for="result in results" :key="result.itemName" :result="result" :filter="filter" @item-added-to-filter="onItemAddedToFilter"></filter-items-search-result-view>
                </div>
                <div v-if="lastSearchedText.length > 0 || results.length > 0" class="tqFilterItemsViewListSeparator">
                    Items in Filter
                </div>
                <filter-items-record-view v-for="itemName in filter.settings.items.names" :filter="filter" :itemName="itemName"></filter-items-record-view>
            </div>
        </div>
        <div v-else>
            <item-query-stats-view :stats="filter.settings.items.stats" @stats-changed="onFilterChanged"></item-query-stats-view>
        </div>
    </div>
</template>

<script lang="ts">
    import Vue from "vue";
    import * as _ from "lodash";
    import axios from "axios";

    import Filter from "../../interfaces/Filter";
    import FilterItemSearchResult from "../../interfaces/FilterItemSearchResult";

    import TQGlobals from "../../classes/TQGlobals";

    import FilterItemsSearchResultView from "./FilterItemsSearchResultView.vue";
    import FilterItemsRecordView from "./FilterItemsRecordView.vue";
    import ItemQueryStatsView from "../ItemQueryStatsView.vue";

    export default Vue.extend({

        props: {
            filter: {
                type: Object as () => Filter,
                required: true
            },
        },

        data: function () {
            return {
                searchText: "",
                lastSearchedText: "",
                showSearchOptions: false,
                includeUnknownWtb: true,
                includeUnknownWts: true,
                results: [] as Array<FilterItemSearchResult>,
            }
        },

        mounted: function () {
        },

        methods: {
            onFilterChanged: function () {
                TQGlobals.filterManager.saveUserFilters();
            },

            onToggleSearchOptionsClicked: function() {
                this.showSearchOptions = !this.showSearchOptions;
            },

            onItemAddedToFilter: function () {
                // reset the search box & results
                this.searchText = "";
                this.lastSearchedText = "";
                this.results = [];
            },

            doSearch: _.debounce(function (this: any) {
                this.searchText = (this.searchText as string).trim();

                if (this.searchText.length == 0)
                    this.results = [];
                else if (this.searchText.length < TQGlobals.settings.minFilterItemNameLength)
                    return;

                axios.get('/api/filter_items', {
                    params: {
                        serverCode: TQGlobals.serverCode,
                        startingWith: this.searchText,
                        includeUnknownItems: (this.includeUnknownWtb || this.includeUnknownWts),
                        includeBuying: this.includeUnknownWtb,
                        includeSelling: this.includeUnknownWts
                    }
                })
                .then(response => {
                    this.results = response.data as Array<FilterItemSearchResult>;
                    this.lastSearchedText = this.searchText;
                })
                .catch(err => {
                    // stub
                    console.log(err);
                }); // end axios.get(filter_items)

            }, 300), // end onItemNameBoxKeyUp

        }, // end methods

        components: {
            FilterItemsSearchResultView,
            FilterItemsRecordView,
            ItemQueryStatsView
        }
    });
</script>
