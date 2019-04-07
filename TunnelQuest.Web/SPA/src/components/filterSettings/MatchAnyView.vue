<style>
</style>

<template>
    <span>
        <span>
            Only show content that matches
        </span>
        <select ref="dropDown" @change="onDropDownChange">
            <option value="1">any</option>
            <option value="0">all</option>
        </select>
        <span>
            of the filters below:
        </span>
    </span>
</template>

<script lang="ts">
    import mixins from 'vue-typed-mixins';

    import FilterSettingView from "../../mixins/FilterSettingView";
    

    export default mixins(FilterSettingView).extend({

        mounted: function () {
            let dropDown = this.$refs.dropDown as HTMLSelectElement;

            if (this.filter.matchAny)
                dropDown.value = "1";
            else
                dropDown.value = "0";
        },

        methods: {

            onDropDownChange: function () {
                let dropDown = this.$refs.dropDown as HTMLSelectElement;

                this.filter.matchAny = (dropDown.value == "1");
                this.saveFunction();
            },

        }
    });
</script>
