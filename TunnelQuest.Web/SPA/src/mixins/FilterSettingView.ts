
import Vue from "vue";

import Filter from "../interfaces/Filter";


export default Vue.extend({

    props: {
        filter: {
            type: Object as () => Filter,
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

