<template>
    <div class="tqheader">
        <div>TunnelQuest</div>
        <div v-if="isDisconnected" class="tqheaderconnect">Scroll up to see new auctions</div>
    </div>
</template>

<script lang="ts">
    import Vue from "vue";

    export default Vue.extend({
        props: {
        },
        data: function () {
            return {
                isDisconnected: false,

                // "private"
                htmlElement_: {} as HTMLElement
            };
        },
        created: function () {
        },
        mounted: function () {
            this.htmlElement_ = this.$el as HTMLElement;
            this.$parent.$on("scroll", this.onScroll);
            this.$parent.$on("disconnected", this.onDisconnected);
            this.$parent.$on("connecting", this.onConnecting);
        },
        methods: {
            onScroll: function () {
                if (window.pageYOffset > this.htmlElement_.offsetTop)
                    this.htmlElement_.classList.add("tqstickyheader");
                else
                    this.htmlElement_.classList.remove("tqstickyheader");
            },

            onDisconnected: function () {
                this.isDisconnected = true;
            },

            onConnecting: function () {
                this.isDisconnected = false;
            }
        },
        components: {
        }
    });
</script>

<style>
    .tqheader {
        background-color: #CCCCCC;
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