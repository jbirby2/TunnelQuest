
<style>
    .tqheader {
        background-color: #d19200;
        z-index: 999;
    }

    .tqstickyheader {
        position: fixed;
        top: 0;
        width: 100%
    }

    .tqheaderconnect {
        background-color: #F8C471;
    }
</style>

<template>
    <div class="tqheader">
        <router-link to="/">Auction House View</router-link> |
        <router-link to="/chat">Chat View</router-link>
    </div>
</template>

<script lang="ts">
    import Vue from "vue";

    export default Vue.extend({
        data: function () {
            return {
                // "private"
                htmlElement_: {} as HTMLElement
            };
        },
        mounted: function () {
            this.htmlElement_ = this.$el as HTMLElement;
            window.addEventListener("scroll", this.onScroll);
        },
        methods: {
            onScroll: function () {
                if (window.pageYOffset > this.htmlElement_.offsetTop)
                    this.htmlElement_.classList.add("tqstickyheader");
                else
                    this.htmlElement_.classList.remove("tqstickyheader");
            }
        },
    });
</script>
