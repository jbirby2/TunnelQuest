<template>
    <span :class="cssClass">
        <span style="vertical-align:middle;">{{text}}</span>
    </span>
</template>

<script lang="ts">
    import Vue from "vue";
    import * as moment from "moment";

    export default Vue.extend({
        props: {
            cssClass: {
                type: String,
                required: false
            },
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
            this.updateText();
        },
        watch: {
            timeString: function (newValue, oldValue) {
                this.moment_ = moment.utc(newValue).local();
                this.updateText();
            }
        },
        beforeDestroy: function () {
            if (this.timer_ > 0)
                clearTimeout(this.timer_);
        },
        methods: {
            
            updateText: function () {
                let timestampMoment = (this.moment_ as moment.Moment);
                let nowMoment = moment.default();
                let sinceTimestamp = moment.duration(nowMoment.diff(timestampMoment));

                let asMinutes = sinceTimestamp.asMinutes();
                let asHours = sinceTimestamp.asHours();

                let timerDelay = -1;
                let newText = "";
                if (asMinutes < 1) {
                    newText = Math.floor(sinceTimestamp.asSeconds()).toString() + " seconds ago";
                    timerDelay = 1000; // 1 second
                }
                else if (asMinutes < 2) {
                    newText = "1 minute ago";
                    timerDelay = 60000; // 1 minute
                }
                else if (asHours < 1) {
                    newText = sinceTimestamp.minutes().toString() + " minutes ago";
                    timerDelay = 60000; // 1 minute
                }
                else if (asHours < 2) {
                    newText = "1 hour ago";
                    timerDelay = 3600000; // 1 hour
                }
                else if (sinceTimestamp.asDays() < 1) {
                    newText = sinceTimestamp.hours().toString() + " hours ago";
                    timerDelay = 3600000; // 1 hour
                }
                else
                    newText = timestampMoment.format('YYYY-MM-DD HH:mm:ss');

                this.text = newText;
                //this.text = timestampMoment.fromNow();

                if (timerDelay > 0)
                    this.timer_ = setTimeout(this.updateText, timerDelay);
            }
        },
        components: {
        }
    });
</script>

<style>
</style>