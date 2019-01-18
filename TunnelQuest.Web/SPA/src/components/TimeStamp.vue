<template>
    <span>{{text}}</span>
</template>

<script lang="ts">
    import Vue from "vue";
    import * as moment from "moment";

    export default Vue.extend({
        props: {
            time: {
                type: Object as () => Date,
                required: true
            }
        },
        data: function () {
            return {
                text: "",

                // "private"
                interval_: -1,
                moment_: null as moment.Moment | null
            };
        },
        created: function () {
        },
        mounted: function () {
            this.moment_ = moment.utc(this.time);
            this.tick();
            this.interval_ = setInterval(this.tick, 60000);
        },
        beforeDestroy: function () {
            clearInterval(this.interval_);
        },
        methods: {
            tick: function () {
                this.text = (this.moment_ as moment.Moment).fromNow();
            }
        },
        components: {
        }
    });
</script>

<style>
</style>