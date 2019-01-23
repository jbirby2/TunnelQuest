
<style>
    .tqheader {
        background-color: #f9ae00;
        z-index: 999;
    }

    .tqstickyheader {
        position: fixed;
        top: 0;
        width: 100%
    }

    .tqroutelinks {
    }

    .tqheaderdisco {
        background-color: #ffc644;
        color: #000000;
    }
</style>

<template>
    <div class="tqheader">
        <div class="tqroutelinks">
            <router-link to="/">Auction House View</router-link> |
            <router-link to="/chat">Chat View</router-link>
        </div>

        <transition name="fade">
            <div v-if="isDisconnected" class="tqheaderdisco">Disconnected - scroll up to reconnect</div>
        </transition>
    </div>
</template>

<script lang="ts">
    import Vue from "vue";

    import TQGlobals from "../classes/TQGlobals";

    export default Vue.extend({

        data: function () {
            return {
                isDisconnected: true,

                // "private"
                htmlElement_: {} as HTMLElement
            };
        },

        mounted: function () {
            this.htmlElement_ = this.$el as HTMLElement;
            window.addEventListener("scroll", this.onScroll);

            TQGlobals.onInit(this.onInit);
        },

        methods: {

            onScroll: function () {
                if (window.pageYOffset > this.htmlElement_.offsetTop)
                    this.htmlElement_.classList.add("tqstickyheader");
                else
                    this.htmlElement_.classList.remove("tqstickyheader");
            },

            onInit: function () {
                TQGlobals.connection.onConnected(this.onConnected);
                TQGlobals.connection.onDisconnected(this.onDisconnected);
            },

            onConnected: function () {
                this.isDisconnected = false;
            },

            onDisconnected: function () {
                this.isDisconnected = true
            }
        },
    });
</script>
