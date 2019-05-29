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

    .tqFilterItemsViewStatsTable {
        display: table;
    }

    .tqFilterItemsViewStatsTable > div {
        display: table-row;
    }

    .tqFilterItemsViewStatsTable > div > span {
        display: table-cell;
    }

    .tqFilterItemsStatBox {
        width: 35px;
    }

    .tqFilterItemsStatBoxEmpty {
        background-color: #555 !important;
    }

    .tqFilterItemsStatBoxEmpty:focus {
        background-color: #edfeff !important;
    }

</style>

<template>
    <div>
        <div>
            <input id="tqFilterByNameRadio" type="radio" value="name" v-model="filter.settings.items.filterType" @change="onFilterChanged" />
            <label for="tqFilterByNameRadio">
                Filter by Name
            </label>
            <input id="tqFilterByStatsRadio" type="radio" value="stats" v-model="filter.settings.items.filterType" @change="onFilterChanged" />
            <label for="tqFilterByStatsRadio">
                Filter by Stats
            </label>
        </div>
        <div v-if="filter.settings.items.filterType == 'name'">
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
            <div class="tqFilterItemsViewStatsTable">
                <div>
                    <span>Strength</span>
                    <span>
                        <input type="tel" maxlength="3" v-model="filter.settings.items.stats.minStrength" class="tqFilterItemsStatBox" @change="onFilterChanged" />
                    </span>

                    <span>Stamina</span>
                    <span>
                        <input type="tel" maxlength="3" v-model="filter.settings.items.stats.minStamina" class="tqFilterItemsStatBox" @change="onFilterChanged" />
                    </span>

                    <span>Agility</span>
                    <span>
                        <input type="tel" maxlength="3" v-model="filter.settings.items.stats.minAgility" class="tqFilterItemsStatBox" @change="onFilterChanged" />
                    </span>

                    <span>Dexterity</span>
                    <span>
                        <input type="tel" maxlength="3" v-model="filter.settings.items.stats.minDexterity" class="tqFilterItemsStatBox" @change="onFilterChanged" />
                    </span>

                    <span>Wisdom</span>
                    <span>
                        <input type="tel" maxlength="3" v-model="filter.settings.items.stats.minWisdom" class="tqFilterItemsStatBox" @change="onFilterChanged" />
                    </span>

                    <span>Intelligence</span>
                    <span>
                        <input type="tel" maxlength="3" v-model="filter.settings.items.stats.minIntelligence" class="tqFilterItemsStatBox" @change="onFilterChanged" />
                    </span>

                    <span>Charisma</span>
                    <span>
                        <input type="tel" maxlength="3" v-model="filter.settings.items.stats.minCharisma" class="tqFilterItemsStatBox" @change="onFilterChanged" />
                    </span>

                    <span>HP</span>
                    <span>
                        <input type="tel" maxlength="3" v-model="filter.settings.items.stats.minHitPoints" class="tqFilterItemsStatBox" @change="onFilterChanged" />
                    </span>

                    <span>Mana</span>
                    <span>
                        <input type="tel" maxlength="3" v-model="filter.settings.items.stats.minMana" class="tqFilterItemsStatBox" @change="onFilterChanged" />
                    </span>

                    <span>AC</span>
                    <span>
                        <input type="tel" maxlength="3" v-model="filter.settings.items.stats.minArmorClass" class="tqFilterItemsStatBox" @change="onFilterChanged" />
                    </span>

                    <span>Magic Resist</span>
                    <span>
                        <input type="tel" maxlength="3" v-model="filter.settings.items.stats.minMagicResist" class="tqFilterItemsStatBox" @change="onFilterChanged" />
                    </span>
                </div>
            </div>
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
                searchText: "",
                lastSearchedText: "",
                showSearchOptions: false,
                includeUnknownWtb: true,
                includeUnknownWts: true,
                results: [] as Array<FilterItemSearchResult>,
            }
        },

        mounted: function () {
            this.updateStatBackgrounds();
        },

        methods: {
            onFilterChanged: function () {
                this.updateStatBackgrounds();
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

            updateStatBackgrounds: function () {
                let statBoxElements = document.getElementsByClassName("tqFilterItemsStatBox");
                for (let i = 0; i < statBoxElements.length; i++) {
                    let statBoxElem = statBoxElements.item(i) as HTMLInputElement;
                    if (statBoxElem.value.trim().length == 0)
                        statBoxElem.classList.add("tqFilterItemsStatBoxEmpty");
                    else
                        statBoxElem.classList.remove("tqFilterItemsStatBoxEmpty");
                }
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
            FilterItemsRecordView
        }
    });
</script>
