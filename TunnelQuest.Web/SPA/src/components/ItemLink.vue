<style>
    .tqItemLink {
        text-decoration: none;
    }

    .tqItemLinkKnown {
        color: #e049ff;
    }

    .tqItemLinkUnknown {
        color: #f7d8ff;
    }
</style>

<template>
    <a :class="'tqItemLink ' + (isKnown ? 'tqItemLinkKnown' : 'tqItemLinkUnknown')" :href="linkHref" @click="onClick">{{linkText}}</a>
</template>

<script lang="ts">
    import Vue from "vue";
    import VueRouter from 'vue-router';

    export default Vue.extend({
        props: {
            itemName: {
                type: String,
                required: true
            },
            aliasText: {
                type: String,
                required: false
            },
            isKnown: {
                type: Boolean,
                required: true
            },

            // There seems to be a Vue bug where this.$router doesn't get set if the
            // component is created programatically, so we work around it by passing
            // the router in as a property
            router: {
                type: Object as () => VueRouter,
                required: false
            }
        },

        data: function () {
            return {
            };
        },

        computed: {
            linkHref: function () {
                return "/item/" + encodeURIComponent(this.itemName);
            },

            linkText: function () {
                return this.aliasText ? this.aliasText : this.itemName;
            },

            urlEncodedItemName: function () {
                return encodeURIComponent(this.itemName);
            }
        },

        methods: {
            onClick: function (e:MouseEvent) {
                e.preventDefault();

                let url = "/item/" + this.urlEncodedItemName;
                if (this.$router)
                    this.$router.push(url);
                else
                    this.router.push(url);
            },
        }

    });
</script>
