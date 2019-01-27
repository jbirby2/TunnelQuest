
<style>

    .tqAuctionItem {
        color: #e0e0e0;
        position: relative;
        display: inline-block;
        padding: 5px;
    }

    .tqAuctionItem > span {
        display: block;
        margin-top: 3px;
    }

    .tqAuctionItemIcon {
        position: absolute;
        top: 10px;
        right: 10px;
    }

    .tqAuctionItemName {
        font-size: 1.2em;
        color: #e049ff;
        font-weight: bold;
        white-space: nowrap;
    }

    .tqAuctionItemStat {
        margin-right: 10px;
    }

    .tqAuctionItemStat span:nth-child(1) {
        /* make sure stat names with spaces in them are never split up across two lines ("SV MAGIC", "SV FIRE", etc) */
        white-space: nowrap;
    }

    .tqAuctionItemStatValuePositive {
        color: #ffffff;
        font-weight: bold;
    }

    .tqAuctionItemStatValueNegative {
        color: #ff0000;
        font-weight: bold;
    }

    .tqAuctionItemInfo {
        display: block;
        color: #efef00;
        font-style: italic;
        text-align: center;
    }

    @media screen and (min-width: 952px) {
        /* start of desktop styles */
        .tqAuctionItem {
            width: 400px;
            min-height: 120px;
        }
    }

    @media screen and (max-width: 951px) {
        /* start of large tablet styles */
        .tqAuctionItem {
            width: 350px;
            min-height: 100px;
        }
    }

    @media screen and (max-width: 767px) {
        /* start of medium tablet styles */
        .tqAuctionItem {
            width: 280px;
            min-height: 80px;
        }
    }

    @media screen and (max-width: 609px) {
        .tqAuctionItem {
            width: 225px;
        }
    }

    @media screen and (max-width: 479px) {
        /* start of phone styles */
        .tqAuctionItem {
            width: 100%;
        }
        .tqAuctionItemIcon {
            right: 20px;
        }
    }

</style>

<template>
    <span class="tqAuctionItem">
        <img v-if="item.iconFileName != null" :src="iconUrl" class="tqAuctionItemIcon" />

        <span class="tqAuctionItemName">
            {{ item.itemName }}
        </span>

        <span>
            <span v-for="(flag, index) in flags" class="tqAuctionItemStat">
                {{flag}}
            </span>
        </span>

        <span v-if="item.slots && item.slots.length > 0">
            <span>Slots: </span>
            <span v-for="(slotCode, index) in item.slots" class="tqAuctionItemStat">
                {{slotCode}}
            </span>
        </span>

        <span v-if="item.maxCharges != null">
            <span v-if="item.isExpendable">
                <span class="tqAuctionItemStat">EXPENDABLE</span>
            </span>
            <span class="tqAuctionItemStat">
                <span>Charges: </span>
                <span class="tqAuctionItemStatValuePositive">{{item.maxCharges}}</span>
            </span>
        </span>

        <span v-if="item.weaponSkillCode != null">
            <span class="tqAuctionItemStat">
                <span>Skill: </span>
                <span class="tqAuctionItemStatValuePositive">{{item.weaponSkillCode}}</span>
            </span>
            <span class="tqAuctionItemStat">
                <span>Attack Delay: </span>
                <span class="tqAuctionItemStatValuePositive">{{item.attackDelay}}</span>
            </span>
        </span>

        <span v-if="item.attackDamage != null">
            <span class="tqAuctionItemStat">
                <span>Damage: </span>
                <span class="tqAuctionItemStatValuePositive">{{item.attackDamage}}</span>
            </span>
        </span>

        <span v-if="item.armorClass != null">
            <span class="tqAuctionItemStat">
                <span>AC: </span>
                <span :class="getStatValueCssClass(item.armorClass)">{{item.armorClass}}</span>
            </span>
        </span>

        <span>
            <span v-for="(stat, index) in stats" class="tqAuctionItemStat">
                <span>{{stat.code}}:&nbsp;</span><span :class="getStatValueCssClass(stat.value)">{{stat.value}} </span>
            </span>
        </span>

        <span v-if="item.singingModifier != null">
            <span>Singing Instrument: </span>
            <span :class="getStatValueCssClass(item.singingModifier)">{{ item.singingModifier }}</span>
        </span>
        <span v-if="item.percussionModifier != null">
            <span>Percussion Instrument: </span>
            <span :class="getStatValueCssClass(item.percussionModifier)">{{ item.percussionModifier }}</span>
        </span>
        <span v-if="item.stringedModifier != null">
            <span>Stringed Instrument: </span>
            <span :class="getStatValueCssClass(item.stringedModifier)">{{ item.stringedModifier }}</span>
        </span>
        <span v-if="item.brassModifier != null">
            <span>Brass Instrument: </span>
            <span :class="getStatValueCssClass(item.brassModifier)">{{ item.brassModifier }}</span>
        </span>
        <span v-if="item.windModifier != null">
            <span>Wind Instrument: </span>
            <span :class="getStatValueCssClass(item.windModifier)">{{ item.windModifier }}</span>
        </span>

        <span v-if="item.requiredLevel != null">
            <span>Required level of </span>
            <span class="tqAuctionItemStatValuePositive">{{ item.requiredLevel }}</span>
        </span>

        <span v-if="item.effectSpellName != null">
            <span>Effect: </span>
            <span class="tqAuctionItemStatValuePositive">{{item.effectSpellName}}</span>
            <span v-if="effectTypeDesc != null">
                <!-- the next line is a nightmare but don't break it up into multiple lines; otherwise spaces will appear in ugly spots in the rendered UI -->
                <span> ({{effectTypeDesc}}</span><span v-if="item.effectCastingTime != null && item.effectTypeCode != 'Combat'"><span>, Casting Time: </span><span class="tqAuctionItemStatValuePositive">{{castingTimeString}}</span></span><span>)</span>
                <span v-if="item.effectMinimumLevel != null">
                    <span> at Level </span>
                    <span :class="getStatValueCssClass(item.effectMinimumLevel)">{{item.effectMinimumLevel}}</span>
                </span>
            </span>
        </span>

        <span v-if="item.haste != null">
            <span>Haste: </span>
            <span :class="getStatValueCssClass(item.haste)">{{ item.haste * 100 }}%</span>
        </span>

        <span>
            <span class="tqAuctionItemStat">
                <span>Weight: </span>
                <span>{{ item.weight.toFixed(1) }}</span>
            </span>
            <span v-if="item.range != null" class="tqAuctionItemStat">
                <span>Range: </span>
                <span class="tqAuctionItemStatValuePositive">{{ item.range }}</span>
            </span>
            <span v-if="item.sizeCode != null" class="tqAuctionItemStat">
                <span>Size: </span>
                <span>{{ item.sizeCode }}</span>
            </span>
            <span v-if="item.weightReduction != null" class="tqAuctionItemStat">
                <span>Weight Reduction: </span>
                <span :class="getStatValueCssClass(item.weightReduction)">{{ item.weightReduction * 100 }}%</span>
            </span>
        </span>

        <span v-if="item.capacity != null">
            <span class="tqAuctionItemStat">
                <span>Capacity: </span>
                <span class="tqAuctionItemStatValuePositive">{{ item.capacity }}</span>
            </span>
            <span class="tqAuctionItemStat">
                <span>Size Capacity: </span>
                <span class="tqAuctionItemStatValuePositive">{{ item.capacitySizeCode }}</span>
            </span>
        </span>

        <span v-if="item.classes && item.classes.length > 0">
            Classes: {{ buildClassesString() }}
        </span>

        <span v-if="item.races && item.races.length > 0">
            Races: {{ buildRacesString() }}
        </span>

        <span v-if="item.deities && item.deities.length > 0">
            Deities: {{ buildDeitiesString() }}
        </span>

        <span v-if="item.info && item.info.length > 0">
            <span v-for="(info, index) in item.info" class="tqAuctionItemInfo">{{info}}</span>
        </span>

        <span>
            <slot name="footer"></slot>
        </span>
    </span>
</template>

<script lang="ts">
    import Vue from "vue";

    import Item from "../interfaces/Item";
import TQGlobals from '@/classes/TQGlobals';

    export default Vue.extend({

        props: {
            item: {
                type: Object as () => Item,
                required: true
            }
        },

        computed: {
            iconUrl: function () {
                return "/game_assets/" + this.item.iconFileName;
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
                    return "tqAuctionItemStatValueNegative";
                else
                    return "tqAuctionItemStatValuePositive";
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
        }
    });
</script>

