
<style>
    .tqSpell {
        display: block;
    }


    .tqSpellSection {
        display: block;
        margin-top: 10px;
    }

    .tqSpellLine {
        display: block;
        margin-top: 3px;
    }

    .tqSpellIcon {
        position: absolute;
        top: 10px;
        right: 10px;
    }

    .tqSpellName {
        margin-top: 5px;
        font-size: 1.2em;
        color: #e049ff;
        font-weight: bold;
        white-space: nowrap;
    }

    .tqSpellDesc {
        margin-right: 60px;
        font-style: italic;
    }

    .tqSpellDetail {
        margin-left: 10px;
    }

</style>

<template>
    <span class="tqSpell">
        <img v-if="spell.iconFileName != null" :src="iconUrl" class="tqSpellIcon" />

        <span v-if="showSpellName" class="tqSpellSection tqSpellName">
            {{ spell.spellName }}
        </span>

        <span class="tqSpellSection tqSpellDesc">
            {{ spell.description }}
        </span>

        <span v-if="spell.details" class="tqSpellSection">
            <span class="tqSpellLine">Effect Details:</span>
            <span v-for="detail in spell.details" class="tqSpellLine tqSpellDetail">{{ detail }}</span>
        </span>

        <span v-if="spell.sources" class="tqSpellSection">
            <span v-for="source in spell.sources" class="tqSpellLine">{{ source }}</span>
        </span>

        <span v-if="spell.requirements" class="tqSpellSection">
            Classes: {{ buildClassesString() }}
        </span>
    </span>
</template>

<script lang="ts">
    import Vue from "vue";

    import Spell from "../interfaces/Spell";

    import TQGlobals from '../classes/TQGlobals';


    export default Vue.extend({
        props: {
            spell: {
                type: Object as () => Spell,
                required: true
            },
            showSpellName: {
                type: Boolean,
                required: true
            }
        },

        computed: {
            iconUrl: function () {
                return "/game_assets/" + this.spell.iconFileName;
            },
        },

        methods: {
            buildClassesString() {
                let str = "";
                if (this.spell.requirements) {
                    for (let classCode in this.spell.requirements) {
                        if (str != "")
                            str += ", ";
                        str += TQGlobals.settings.classes[classCode] + " (L" + this.spell.requirements[classCode].toString() + ")";
                    }
                }
                return str;
            }
        },

        data: function () {
            return {
            };
        }
    });
</script>

