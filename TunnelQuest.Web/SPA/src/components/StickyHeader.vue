
<style>
    .tqheader {
        background-color: #ce9000;
        z-index: 999;
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

    .tqroutelinks {
    }

    .tqheaderdisco {
        font-style: italic;
    }

</style>

<template>
    <div class="tqheader">
        <div class="tqroutelinks">
            <router-link to="/">Auction House View</router-link>
            <router-link to="/chat">Chat View</router-link>

            <transition name="fade">
                <span class="tqheaderdisco">
                    <span v-if="isDisconnected">Disconnected</span>
                    <span v-if="!isScrolledToTop"> - scroll up to reconnect</span>
                </span>
            </transition>
        </div>
    </div>
</template>

<script lang="ts">
    import Vue from "vue";

    import TQGlobals from "../classes/TQGlobals";

    export default Vue.extend({

        data: function () {
            return {
                isDisconnected: true,
                isScrolledToTop: true,

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
                if (window.pageYOffset > this.htmlElement_.offsetTop) {
                    this.htmlElement_.style.width = this.htmlElement_.clientWidth.toString() + "px";
                    this.htmlElement_.classList.add("tqstickyheader");
                }
                else {
                    this.htmlElement_.classList.remove("tqstickyheader");
                    this.htmlElement_.style.width = null;
                }

                if (document != null && document.documentElement != null) {
                    this.isScrolledToTop = (document.documentElement.scrollTop == 0);
                }
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
