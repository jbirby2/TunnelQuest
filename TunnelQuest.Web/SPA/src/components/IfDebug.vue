<style>
</style>

<template>
    <span v-if="isDebugMode">
        <slot></slot>
    </span>
</template>

<script lang="ts">
    import Vue from "vue";

    export default Vue.extend({
        data: function () {
            return {
                isDebugMode: false
            };
        },

        beforeMount: function () {
            //this.isDebugMode = this.$route.query["debug"] == "true";
            this.isDebugMode = window.location.href.endsWith("debug=true");
        },

    });
</script>
