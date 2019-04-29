<style>
    .tqFilterItemsRecordView {
        display: table-row;
    }

    .tqFilterItemsRecordView > span {
        display: table-cell;
    }

    .tqFilterItemsRecordView > span:nth-child(2) {
        text-align: center;
    }
</style>

<template>
    <div class="tqFilterItemsRecordView">
        <span>
            <item-link :itemName="itemName" :isKnown="isKnown"></item-link>
        </span>
        <span>
            <input type="button" value="Remove from Filter" @click="onRemoveClicked" />
        </span>
    </div>
</template>

<script lang="ts">
    import Vue from "vue";

    import Filter from "../../interfaces/Filter";

    import TQGlobals from "../../classes/TQGlobals";

    import ItemLink from "../ItemLink.vue";

    export default Vue.extend({

        props: {
            filter: {
                type: Object as () => Filter,
                required: true
            },
            itemName: {
                type: String,
                required: true
            },
        },

        data: function () {
            return {
                isKnown: true
            }
        },

        mounted: function () {
            this.isKnown = this.filter.metaData.itemIsKnown[this.itemName] as boolean;
        },

        methods: {

            onRemoveClicked: function () {
                let index = this.filter.settings.itemNames.indexOf(this.itemName);
                if (index >= 0) {
                    this.filter.settings.itemNames.splice(index, 1);
                    TQGlobals.filterManager.saveUserFilters();
                }
            },

        },

        components: {
            ItemLink
        }
    });
</script>
