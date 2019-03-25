
import Vue from "vue";

import Filters from "../interfaces/Filters";

import TQGlobals from "../classes/TQGlobals";


export default Vue.extend({

    props: {
        filters: {
            type: Object as () => Filters,
            required: true
        },
        saveFunction: {
            type: Function,
            required: true
        }
    },

    data: function () {
        return {
        };
    },


    methods: {

    }
});

