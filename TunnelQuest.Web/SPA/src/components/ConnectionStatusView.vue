
<style>
    .tqConnectionStatusView {
        font-style: italic;
    }
</style>

<template>
    <span class="tqConnectionStatusView">
        <span v-if="isConnected == false">
            <span>Disconnected</span>
            <span v-if="isScrolledToTop == false"> - scroll up to reconnect</span>
        </span>
    </span>
</template>

<script lang="ts">
    import Vue from "vue";

    import ConnectionWrapper from "../classes/ConnectionWrapper";


    export default Vue.extend({

        props: {
            connection: {
                type: Object as () => ConnectionWrapper,
                required: true
            }
        },

        data: function () {
            return {
                isConnected: false,
                isScrolledToTop: true
            };
        },

        mounted: function () {
            window.addEventListener("scroll", this.onScroll);

            this.connection.onConnected(this.onConnected);
            this.connection.onDisconnected(this.onDisconnected);

            if (this.connection.isConnected())
                this.isConnected = true;
        },

        methods: {

            onScroll: function () {
                this.isScrolledToTop = (document != null && document.documentElement != null && document.documentElement.scrollTop == 0);
            },

            onConnected: function () {
                this.isConnected = true;
            },

            onDisconnected: function () {
                this.isConnected = false;
            }
        }
    });
</script>