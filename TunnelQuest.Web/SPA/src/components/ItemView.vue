
<style>

    .tqItem {
        display: block;
        background-color: rgba(0,0,0,0.7);
        color: #e0e0e0;
        position: relative;
        padding: 0 5px 5px 5px;
    }

    .tqItemLine {
        display: block;
        margin-top: 3px;
    }

    .tqItemIcon {
        position: absolute;
        top: 10px;
        right: 10px;
    }

    .tqItemName {
        margin-top: 5px;
        font-size: 1.2em;
        font-weight: bold;
        white-space: nowrap;
    }
    .tqKnownItemName {
        color: #e049ff;
    }
    .tqUnknownItemName {
        color: #f7d8ff;
    }

    .tqItemAliases {
        font-style: italic;
    }
    .tqItemAliases span:nth-child(2) {
        color: #e049ff;
    }

    .tqItemStat {
        margin-right: 7px;
    }

    .tqItemStat span:nth-child(1) {
        /* make sure stat names with spaces in them are never split up across two lines ("SV MAGIC", "SV FIRE", etc) */
        white-space: nowrap;
    }

    .tqItemStatValuePositive {
        color: #ffffff;
        font-weight: bold;
    }

    .tqItemStatValueNegative {
        color: #ff0000;
        font-weight: bold;
    }

    .tqItemInfo {
        display: block;
        color: #efef00;
    }


    @media screen and (min-width: 952px) {
        /* start of desktop styles */
        .tqItem {
            width: 400px;
            min-height: 120px;
        }
    }

    @media screen and (max-width: 951px) {
        /* start of large tablet styles */
        .tqItem {
            width: 350px;
            min-height: 100px;
        }
    }

    @media screen and (max-width: 767px) {
        /* start of medium tablet styles */
        .tqItem {
            width: 280px;
            min-height: 80px;
        }
    }

    @media screen and (max-width: 609px) {
        .tqItem {
            width: 225px;
        }
    }

    @media screen and (max-width: 479px) {
        /* start of phone styles */
        .tqItem {
            width: 100%;
        }
        .tqItemIcon {
            right: 20px;
        }
    }

</style>

<template>
    <span class="tqItem">

        <span :class="'tqItemLine tqItemName ' + (item.isFetched ? 'tqKnownItemName' : 'tqUnknownItemName')">
            {{item.itemName}}
        </span>

        <span v-if="showAliases == true && aliasesString != null" class="tqItemAliases">
            <span>aka </span>
            <span>{{aliasesString}}</span>
        </span>

        <span v-if="item.effectSpell != null && item.effectTypeCode == 'LearnSpell'">
            <spell-view :spell="item.effectSpell" :showSpellName="false"></spell-view>
        </span>
        <span v-else>
            <img v-if="item.iconFileName != null" :src="iconUrl" class="tqItemIcon" />

            <span class="tqItemLine">
                <span v-for="(flag, index) in flags" class="tqItemStat">
                    {{flag}}
                </span>
            </span>

            <span v-if="item.slots && item.slots.length > 0" class="tqItemLine">
                <span>Slots: </span>
                <span v-for="(slotCode, index) in item.slots" class="tqItemStat">
                    {{slotCode}}
                </span>
            </span>

            <span v-if="item.maxCharges != null" class="tqItemLine">
                <span v-if="item.isExpendable">
                    <span class="tqItemStat">EXPENDABLE</span>
                </span>
                <span class="tqItemStat">
                    <span>Charges: </span>
                    <span class="tqItemStatValuePositive">{{item.maxCharges}}</span>
                </span>
            </span>

            <span v-if="item.weaponSkillCode != null" class="tqItemLine">
                <span class="tqItemStat">
                    <span>Skill: </span>
                    <span class="tqItemStatValuePositive">{{item.weaponSkillCode}}</span>
                </span>
                <span class="tqItemStat">
                    <span>Attack Delay: </span>
                    <span class="tqItemStatValuePositive">{{item.attackDelay}}</span>
                </span>
            </span>

            <span v-if="item.attackDamage != null" class="tqItemLine">
                <span class="tqItemStat">
                    <span>Damage: </span>
                    <span class="tqItemStatValuePositive">{{item.attackDamage}}</span>
                </span>
            </span>

            <span v-if="item.armorClass != null" class="tqItemLine">
                <span class="tqItemStat">
                    <span>AC: </span>
                    <span :class="getStatValueCssClass(item.armorClass)">{{item.armorClass}}</span>
                </span>
            </span>

            <span class="tqItemLine">
                <span v-for="(stat, index) in stats" class="tqItemStat">
                    <span>{{stat.code}}:&nbsp;</span><span :class="getStatValueCssClass(stat.value)">{{stat.value}} </span>
                </span>
            </span>

            <span v-if="item.singingModifier != null" class="tqItemLine">
                <span>Singing Instrument: </span>
                <span :class="getStatValueCssClass(item.singingModifier)">{{ item.singingModifier }}</span>
            </span>
            <span v-if="item.percussionModifier != null" class="tqItemLine">
                <span>Percussion Instrument: </span>
                <span :class="getStatValueCssClass(item.percussionModifier)">{{ item.percussionModifier }}</span>
            </span>
            <span v-if="item.stringedModifier != null" class="tqItemLine">
                <span>Stringed Instrument: </span>
                <span :class="getStatValueCssClass(item.stringedModifier)">{{ item.stringedModifier }}</span>
            </span>
            <span v-if="item.brassModifier != null" class="tqItemLine">
                <span>Brass Instrument: </span>
                <span :class="getStatValueCssClass(item.brassModifier)">{{ item.brassModifier }}</span>
            </span>
            <span v-if="item.windModifier != null" class="tqItemLine">
                <span>Wind Instrument: </span>
                <span :class="getStatValueCssClass(item.windModifier)">{{ item.windModifier }}</span>
            </span>

            <span v-if="item.requiredLevel != null" class="tqItemLine">
                <span>Required level of </span>
                <span class="tqItemStatValuePositive">{{ item.requiredLevel }}</span>
            </span>

            <span v-if="item.effectSpellName != null" class="tqItemLine">
                <span>Effect: </span>
                <span class="tqItemStatValuePositive">{{item.effectSpellName}}</span>
                <span v-if="effectTypeDesc != null">
                    <!-- the next line is a nightmare but don't break it up into multiple lines; otherwise spaces will appear in ugly spots in the rendered UI -->
                    <span> ({{effectTypeDesc}}</span><span v-if="item.effectCastingTime != null && item.effectTypeCode != 'Combat'"><span>, Casting Time: </span><span class="tqItemStatValuePositive">{{castingTimeString}}</span></span><span>)</span>
                    <span v-if="item.effectMinimumLevel != null">
                        <span> at Level </span>
                        <span :class="getStatValueCssClass(item.effectMinimumLevel)">{{item.effectMinimumLevel}}</span>
                    </span>
                </span>
            </span>

            <span v-if="item.haste != null" class="tqItemLine">
                <span>Haste: </span>
                <span :class="getStatValueCssClass(item.haste)">{{ item.haste * 100 }}%</span>
            </span>

            <span class="tqItemLine">
                <span class="tqItemStat">
                    <span>Weight: </span>
                    <span>{{ item.weight.toFixed(1) }}</span>
                </span>
                <span v-if="item.range != null" class="tqItemStat">
                    <span>Range: </span>
                    <span class="tqItemStatValuePositive">{{ item.range }}</span>
                </span>
                <span v-if="item.sizeCode != null" class="tqItemStat">
                    <span>Size: </span>
                    <span>{{ item.sizeCode }}</span>
                </span>
                <span v-if="item.weightReduction != null" class="tqItemStat">
                    <span>Weight Reduction: </span>
                    <span :class="getStatValueCssClass(item.weightReduction)">{{ item.weightReduction * 100 }}%</span>
                </span>
            </span>

            <span v-if="item.capacity != null" class="tqItemLine">
                <span class="tqItemStat">
                    <span>Capacity: </span>
                    <span class="tqItemStatValuePositive">{{ item.capacity }}</span>
                </span>
                <span class="tqItemStat">
                    <span>Size Capacity: </span>
                    <span class="tqItemStatValuePositive">{{ item.capacitySizeCode }}</span>
                </span>
            </span>

            <span v-if="item.classes && item.classes.length > 0" class="tqItemLine">
                Classes: {{ buildClassesString() }}
            </span>

            <span v-if="item.races && item.races.length > 0" class="tqItemLine">
                Races: {{ buildRacesString() }}
            </span>

            <span v-if="item.deities && item.deities.length > 0" class="tqItemLine">
                Deities: {{ buildDeitiesString() }}
            </span>

            <span v-if="item.info && item.info.length > 0" class="tqItemLine">
                <span v-for="(info, index) in item.info" class="tqItemInfo">{{info}}</span>
            </span>
        </span>

        <span class="tqItemLine">
            <slot name="footer"></slot>
        </span>
    </span>
</template>

<script lang="ts">
    import Vue from "vue";

    import Item from "../interfaces/Item";

    import TQGlobals from '../classes/TQGlobals';

    import SpellView from "./SpellView.vue";

    export default Vue.extend({

        props: {
            item: {
                type: Object as () => Item,
                required: true
            },
            showAliases: {
                type: Boolean,
                required: true
            },
        },

        computed: {
            iconUrl: function () {
                return "/game_assets/" + this.item.iconFileName;
            },

            aliasesString: function () {
                console.log("STUB");
                console.log();

                if (TQGlobals.settings.aliasesByItemName[this.item.itemName]) {
                    let str = "";
                    for (let alias of TQGlobals.settings.aliasesByItemName[this.item.itemName]) {
                        if (str != "")
                            str += ", ";
                        str += '"' + alias + '"';
                    }
                    return str;
                }
                else
                    return null;
            },

            effectTypeDesc: function () {
                switch (this.item.effectTypeCode) {
                    case "ClickAnySlot":
                        return "Click any slot"
                    case "ClickEquipped":
                        return "Click equipped"
                    case "Combat":
                        return "Combat";
                    case "LearnSpell":
                        return "Learn spell";
                    case "Worn":
                        return "Worn";
                    default:
                        return this.item.effectTypeCode;
                }
            },

            castingTimeString: function () {
                if (this.item.effectCastingTime == null)
                    return "";
                else if (this.item.effectCastingTime == 0)
                    return "Instant";
                else
                    return this.item.effectCastingTime.toFixed(1).toString() + "s";
            },

            flags: function () {
                let flags = new Array<string>();

                if (this.item.isMagic)
                    flags.push("MAGIC");
                if (this.item.isLore)
                    flags.push("LORE");
                if (this.item.isNoDrop)
                    flags.push("NO-DROP");
                if (this.item.isNoTrade)
                    flags.push("NO-TRADE");
                if (this.item.isTemporary)
                    flags.push("TEMPORARY");
                if (this.item.isQuestItem)
                    flags.push("QUEST ITEM");
                if (this.item.isArtifact)
                    flags.push("ARTIFACT");

                return flags;
            },

            stats: function () {
                let stats = new Array();

                if (this.item.strength != null)
                    stats.push({ code: "STR", value: this.formatStatValue(this.item.strength) });
                if (this.item.stamina != null)
                    stats.push({ code: "STA", value: this.formatStatValue(this.item.stamina) });
                if (this.item.agility != null)
                    stats.push({ code: "AGI", value: this.formatStatValue(this.item.agility) });
                if (this.item.dexterity != null)
                    stats.push({ code: "DEX", value: this.formatStatValue(this.item.dexterity) });
                if (this.item.wisdom != null)
                    stats.push({ code: "WIS", value: this.formatStatValue(this.item.wisdom) });
                if (this.item.intelligence != null)
                    stats.push({ code: "INT", value: this.formatStatValue(this.item.intelligence) });
                if (this.item.charisma != null)
                    stats.push({ code: "CHA", value: this.formatStatValue(this.item.charisma) });
                if (this.item.hitPoints != null)
                    stats.push({ code: "HP", value: this.formatStatValue(this.item.hitPoints) });
                if (this.item.mana != null)
                    stats.push({ code: "MANA", value: this.formatStatValue(this.item.mana) });
                if (this.item.magicResist != null)
                    stats.push({ code: "SV MAGIC", value: this.formatStatValue(this.item.magicResist) });
                if (this.item.poisonResist != null)
                    stats.push({ code: "SV POISON", value: this.formatStatValue(this.item.poisonResist) });
                if (this.item.diseaseResist != null)
                    stats.push({ code: "SV DISEASE", value: this.formatStatValue(this.item.diseaseResist) });
                if (this.item.fireResist != null)
                    stats.push({ code: "SV FIRE", value: this.formatStatValue(this.item.fireResist) });
                if (this.item.coldResist != null)
                    stats.push({ code: "SV COLD", value: this.formatStatValue(this.item.coldResist) });

                return stats;
            }
        },

        methods: {
            formatStatValue(statValue: number) {
                if (statValue < 0)
                    return statValue.toString();
                else
                    return "+" + statValue.toString();
            },

            getStatValueCssClass(statValue: number) {
                if (statValue < 0)
                    return "tqItemStatValueNegative";
                else
                    return "tqItemStatValuePositive";
            },

            buildClassesString() {
                return this.buildCommaDelimitedList(TQGlobals.settings.classes, this.item.classes);
            },

            buildRacesString() {
                return this.buildCommaDelimitedList(TQGlobals.settings.races, this.item.races);
            },

            buildDeitiesString() {
                return this.buildCommaDelimitedList(null, this.item.deities);
            },

            buildCommaDelimitedList(allValues: { [code: string]: string } | null, itemValues: Array<string>) {
                if (itemValues.length == 1 && itemValues[0] == "ALL")
                    return "All";
                else {
                    let str = "";
                    for (let i = 0; i < itemValues.length; i++) {
                        if (i > 0)
                            str += ", ";

                        if (allValues == null)
                            str += itemValues[i];
                        else
                            str += allValues[itemValues[i]];
                    }
                    return str;
                }
            }
        },

        components: {
            SpellView
        }
    });
</script>

