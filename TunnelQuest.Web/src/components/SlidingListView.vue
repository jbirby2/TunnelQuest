<template>
    <div class="slidingListView" @scroll="onScroll">
        *inside slidingListView*
        <slot></slot>
    </div>
</template>

<script lang="ts">
    import Vue from "vue";

    import Idable from "../interfaces/Idable";

    import SlidingList from "../classes/SlidingList";


    export default Vue.extend({
        props: {
            slidingList: {
                type: Object as () => SlidingList<Idable>,
                required: true
            }
        },
        data: function () {
            return {
                isAtTop: true,
                isAtBottom: false
            };
        },
        methods: {
            onScroll: function (e: any) {

                let wasAtTop = this.isAtTop;
                let wasAtBottom = this.isAtBottom;

                if (this.$el.scrollTop == 0) {
                    this.isAtTop = true;
                    this.isAtBottom = false;
                }
                else {
                    this.isAtTop = false;
                    this.isAtBottom = (this.$el.scrollHeight == this.$el.scrollTop + this.$el.clientHeight);
                }

                if (wasAtTop)
                    this.$emit("left-top");
                else if (wasAtBottom)
                    this.$emit("left-bottom");

                if (this.isAtTop)
                    this.$emit("hit-top");
                else if (this.isAtBottom)
                    this.$emit("hit-bottom");
            }
        },
        created: function () {
        },
        mounted: function () {
        },
        components: {
        }
    });
</script>

<style>
    .slidingListView {
        border: 1px solid black;
        height: 400px;
        overflow-y: scroll;
    }
</style>