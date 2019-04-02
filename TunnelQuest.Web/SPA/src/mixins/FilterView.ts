
import Vue from "vue";

import Filters from "../interfaces/Filters";


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

