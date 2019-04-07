

<style>
    .tqFilterPage {
    }

</style>


<template>
    <div class="tqFilterPage">
        <site-header></site-header>

        <div v-if="filter != null">
            <input type="text" v-model="filter.name" />
        </div>
    </div>
</template>

<script lang="ts">
    import mixins from 'vue-typed-mixins';

    import Filter from "../interfaces/Filter";

    import TQGlobals from "../classes/TQGlobals";

    import TqPage from "../mixins/TqPage";

    import SiteHeader from "./SiteHeader.vue";


    export default mixins(TqPage).extend({

        name: "FilterPage",

        data: function () {
            return {
                filter: null as Filter | null
            }
        },

        mounted: function () {
            TQGlobals.init(() => {
                this.filter = TQGlobals.filterManager.get(this.$route.params.id);
            });
        },

        methods: {
            //saveFilterFunction: function () {
            //    alert("STUB saveFilterFunction");
            //}
        },

        components: {
            SiteHeader,
        }
    });
</script>
