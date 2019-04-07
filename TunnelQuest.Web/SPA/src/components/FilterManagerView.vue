<style>
    .tqFilterManagerView {
        background-color: red; /* stub */
    }
</style>

<template>
    <div v-if="manager != null" class="tqFilterManagerView">
        <select ref="dropdown" @change="onDropdownChanged">
            <option v-for="filter in manager.filters" :key="filter.id" :value="filter.id">{{filter.name}}</option>
        </select>

        <button v-if="!manager.selectedFilter.isSystem" @click="onEditClicked">Edit</button>
        <button @click="onNewFilterClicked">New Filter</button>
    </div>
</template>

<script lang="ts">
    import Vue from "vue";

    //import Filter from "../interfaces/Filter";

    import FilterManager from "../classes/FilterManager";
    import TQGlobals from "../classes/TQGlobals";


    export default Vue.extend({
        
        data: function () {
            return {
                manager: null as FilterManager | null
            }
        },

        computed: {
        },

        mounted: function () {
            TQGlobals.init(() => {
                this.manager = TQGlobals.filterManager;
                this.manager.onSelectedFilterChanged(this.onSelectedFilterChanged);
                this.onSelectedFilterChanged(); // to initialize the dropdown to the already-selected filter
            });
        },

        methods: {
            onSelectedFilterChanged: function () {
                if (this.manager == null)
                    return;

                // update the selectedIndex in the dropdown to reflect the newly selected filter
                // (use a short timeout so that vue will have added the new option element before we try to select it)
                setTimeout(() => {
                    if (this.manager == null)
                        return;

                    let dropdown = this.$refs["dropdown"] as HTMLSelectElement;

                    for (let i = 0; i < dropdown.options.length; i++) {
                        if (dropdown.options[i].value == this.manager.selectedFilter.id) {
                            dropdown.selectedIndex = i;
                            return;
                        }
                    }
                }, 10);
            },

            onDropdownChanged: function () {
                if (this.manager == null)
                    return;

                let dropdown = this.$refs["dropdown"] as HTMLSelectElement;
                let newSelectedFilter = this.manager.get(dropdown.options[dropdown.selectedIndex].value);

                if (newSelectedFilter != null)
                    this.manager.setSelectedFilter(newSelectedFilter);
            },

            onEditClicked: function () {
                if (this.manager == null)
                    return;
                this.$router.push("/filter/" + this.manager.selectedFilter.id);
            },

            onNewFilterClicked: function () {
                if (this.manager == null)
                    return;

                let newFilter = this.manager.createNewUserFilter();
                this.manager.setSelectedFilter(newFilter);
                this.$router.push("/filter/" + newFilter.id);
            },

        },


    });
</script>
