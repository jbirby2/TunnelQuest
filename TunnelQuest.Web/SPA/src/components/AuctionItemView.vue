
<style>

    .tqAuctionItem {
    }

    @media screen and (min-width: 992px) {
        /* start of desktop styles */
    }

    @media screen and (max-width: 991px) {
        /* start of large tablet styles */
    }

    @media screen and (max-width: 767px) {
        /* start of medium tablet styles */
    }

    @media screen and (max-width: 479px) {
        /* start of phone styles */
    }

</style>

<template>
    <div class="tqAuctionItem">
        <img :src="iconUrl" />
        <span>
            {{ item.itemName }}
        </span>
    </div>
</template>

<script lang="ts">
    import Vue from "vue";

    import Item from "../interfaces/Item";

    export default Vue.extend({
        props: {
            item: {
                type: Object as () => Item,
                required: true
            }
        },
        computed: {
            iconUrl: function () {
                console.log("computed stub " + this.item.iconFileName);
                return "/game_assets/" + this.item.iconFileName;
            }
        }
    });
</script>

