<style>
</style>

<template>
    <div>
        <span>STUB!</span>
        <div>{{result.itemName}}</div>
        <div>{{result.displayText}}</div>
        <div>{{result.isKnownItem}}</div>
    </div>
</template>

<script lang="ts">
    import Vue from "vue";

    import FilterItemSearchResult from "../../interfaces/FilterItemSearchResult";


    export default Vue.extend({

        props: {
            result: {
                type: Object as () => FilterItemSearchResult,
                required: true
            }
        },

        data: function () {
            return {
            }
        },

        mounted: function () {
        },

        methods: {
        }
    });
</script>
