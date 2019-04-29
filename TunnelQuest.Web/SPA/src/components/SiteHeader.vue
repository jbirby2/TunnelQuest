
<style>
    .tqheader {
        background-color: #d19100;
        z-index: 999;
        width: 100%;
    }

    .tqheader a {
        color: #ffffff;
        font-weight: bold;
        text-decoration: none;
        margin-right: 15px;
    }

    .tqstickyheader {
        position: fixed;
        top: 0;
    }

</style>

<template>
    <div class="tqheader">
        <back-link></back-link>
        <slot></slot>
    </div>
</template>

<script lang="ts">
    import Vue from "vue";

    import BackLink from "./BackLink.vue";


    export default Vue.extend({

        data: function () {
            return {
                htmlElement: {} as HTMLElement
            };
        },

        mounted: function () {
            this.htmlElement = this.$el as HTMLElement;
            window.addEventListener("scroll", this.onScroll);
        },

        methods: {

            onScroll: function () {
                if (window.pageYOffset > this.htmlElement.offsetTop) {
                    this.htmlElement.classList.add("tqstickyheader");
                }
                else {
                    this.htmlElement.classList.remove("tqstickyheader");
                }
            }
        },

        components: {
            BackLink
        }
    });
</script>