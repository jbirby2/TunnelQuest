
<style>
    .tqheader {
        background-color: #ce9000;
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
        <div>
            <router-link to="/">Auction House</router-link>
            <router-link to="/newspaper">Newspaper</router-link>
            <router-link to="/chat">Chat</router-link>
        </div>
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
                if (window.pageYOffset > this.htmlElement_.offsetTop) {
                    this.htmlElement_.classList.add("tqstickyheader");
                }
                else {
                    this.htmlElement_.classList.remove("tqstickyheader");
                }
            }
        }
    });
</script>