<template>
    <span>{{text}}</span>
</template>

<script lang="ts">
    import Vue from "vue";
    import * as moment from "moment";

    export default Vue.extend({
        props: {
            timeString: {
                type: String,
                required: true
            }
        },
        data: function () {
            return {
                text: "",

                // "private"
                timer_: -1,
                moment_: null as moment.Moment | null
            };
        },
        mounted: function () {
            this.moment_ = moment.utc(this.timeString).local();
            this.tick();
        },
        watch: {
            timeString: function (newValue, oldValue) {
                this.moment_ = moment.utc(newValue);
                this.tick();
            }
        },
        beforeDestroy: function () {
            clearTimeout(this.timer_);
        },
        methods: {
            tick: function () {
                let timestampMoment = (this.moment_ as moment.Moment);

                let nowMoment = moment.default();
                let sinceTimestamp = moment.duration(nowMoment.diff(timestampMoment));

                let asMinutes = sinceTimestamp.asMinutes();

                let newText = "";
                if (asMinutes < 1)
                    newText = Math.floor(sinceTimestamp.asSeconds()).toString() + " seconds ago";
                else if (sinceTimestamp.asMinutes() < 2)
                    newText = "1 minute ago";
                else if (sinceTimestamp.asHours() < 1)
                    newText = sinceTimestamp.minutes().toString() + " minutes ago";
                else if (sinceTimestamp.asDays() < 1)
                    newText = sinceTimestamp.hours().toString() + " hours ago";
                /*
                else if (sinceTimestamp.asDays() < 3)
                    newText = Math.floor(sinceTimestamp.asHours()).toString() + " hours ago";
                else if (sinceTimestamp.asMonths() < 1)
                    newText = Math.floor(sinceTimestamp.asDays()).toString() + " days ago";
                else if (sinceTimestamp.asYears() < 1)
                    newText = Math.floor(sinceTimestamp.asMonths()).toString() + " months ago";
                else {
                    newText = Math.floor(sinceTimestamp.asYears()).toString() + " years ago";
                }
                */
                else
                    newText = timestampMoment.format('YYYY-MM-DD HH:mm:ss');

                this.text = newText;
                //this.text = timestampMoment.fromNow();

                if (asMinutes < 1)
                    this.timer_ = setTimeout(this.tick, 1000);
                else
                    this.timer_ = setTimeout(this.tick, 60000);
            }
        },
        components: {
        }
    });
</script>

<style>
</style>