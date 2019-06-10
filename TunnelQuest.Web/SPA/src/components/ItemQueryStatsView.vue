<style>
    .tqItemQueryStatsTable {
        display: table;
    }

    .tqItemQueryStatsTable > div {
        display: table-row;
    }

    .tqItemQueryStatsTable > div > span {
        display: table-cell;
    }

    .tqItemQueryStatBox {
        width: 35px;
    }

    .tqItemQueryStatBoxEmpty {
        background-color: #555 !important;
    }

    .tqItemQueryStatBoxEmpty:focus {
        background-color: #edfeff !important;
    }

</style>

<template>
    <div class="tqItemQueryStatsTable">
        <div>
            <span>Strength</span>
            <span>
                <input type="tel" maxlength="3" v-model="stats.minStrength" class="tqItemQueryStatBox" @change="onStatsChanged" />
            </span>

            <span>Stamina</span>
            <span>
                <input type="tel" maxlength="3" v-model="stats.minStamina" class="tqItemQueryStatBox" @change="onStatsChanged" />
            </span>

            <span>Agility</span>
            <span>
                <input type="tel" maxlength="3" v-model="stats.minAgility" class="tqItemQueryStatBox" @change="onStatsChanged" />
            </span>

            <span>Dexterity</span>
            <span>
                <input type="tel" maxlength="3" v-model="stats.minDexterity" class="tqItemQueryStatBox" @change="onStatsChanged" />
            </span>

            <span>Wisdom</span>
            <span>
                <input type="tel" maxlength="3" v-model="stats.minWisdom" class="tqItemQueryStatBox" @change="onStatsChanged" />
            </span>

            <span>Intelligence</span>
            <span>
                <input type="tel" maxlength="3" v-model="stats.minIntelligence" class="tqItemQueryStatBox" @change="onStatsChanged" />
            </span>

            <span>Charisma</span>
            <span>
                <input type="tel" maxlength="3" v-model="stats.minCharisma" class="tqItemQueryStatBox" @change="onStatsChanged" />
            </span>

            <span>HP</span>
            <span>
                <input type="tel" maxlength="3" v-model="stats.minHitPoints" class="tqItemQueryStatBox" @change="onStatsChanged" />
            </span>

            <span>Mana</span>
            <span>
                <input type="tel" maxlength="3" v-model="stats.minMana" class="tqItemQueryStatBox" @change="onStatsChanged" />
            </span>

            <span>AC</span>
            <span>
                <input type="tel" maxlength="3" v-model="stats.minArmorClass" class="tqItemQueryStatBox" @change="onStatsChanged" />
            </span>

            <span>Magic Resist</span>
            <span>
                <input type="tel" maxlength="3" v-model="stats.minMagicResist" class="tqItemQueryStatBox" @change="onStatsChanged" />
            </span>

            <span>Poison Resist</span>
            <span>
                <input type="tel" maxlength="3" v-model="stats.minPoisonResist" class="tqItemQueryStatBox" @change="onStatsChanged" />
            </span>

            <span>Disease Resist</span>
            <span>
                <input type="tel" maxlength="3" v-model="stats.minDiseaseResist" class="tqItemQueryStatBox" @change="onStatsChanged" />
            </span>

            <span>Fire Resist</span>
            <span>
                <input type="tel" maxlength="3" v-model="stats.minFireResist" class="tqItemQueryStatBox" @change="onStatsChanged" />
            </span>

            <span>Cold Resist</span>
            <span>
                <input type="tel" maxlength="3" v-model="stats.minColdResist" class="tqItemQueryStatBox" @change="onStatsChanged" />
            </span>

            <span>Haste</span>
            <span>
                <input type="tel" maxlength="3" v-model="minHasteInteger" class="tqItemQueryStatBox" @change="onMinHasteChanged" />
                <span>%</span>
            </span>
        </div>
    </div>
</template>

<script lang="ts">
    import Vue from "vue";

    import ItemQueryStats from "../interfaces/ItemQueryStats";


    export default Vue.extend({

        props: {
            stats: {
                type: Object as () => ItemQueryStats,
                required: true
            },
        },

        data: function () {
            return {
                minHasteInteger: null as number | null
            }
        },

        mounted: function () {
            if (this.stats.minHaste == null)
                this.minHasteInteger = null;
            else
                this.minHasteInteger = this.stats.minHaste * 100;

            // Give Vue a few milliseconds to update the haste textbox; otherwise it will
            // still be empty when we call updateStatBackgrounds().
            setTimeout(() => { this.updateStatBackgrounds(); }, 10);
        },

        methods: {
            onMinHasteChanged: function () {
                if (this.minHasteInteger == null || (this.minHasteInteger as any) == "")
                    this.stats.minHaste = null;
                else
                    this.stats.minHaste = 0.01 * this.minHasteInteger;

                this.onStatsChanged();
            },

            onStatsChanged: function () {
                this.updateStatBackgrounds();
                this.$emit("stats-changed");
            },

            updateStatBackgrounds: function () {
                let statBoxElements = document.getElementsByClassName("tqItemQueryStatBox");
                for (let i = 0; i < statBoxElements.length; i++) {
                    let statBoxElem = statBoxElements.item(i) as HTMLInputElement;

                    if (statBoxElem.value.trim().length == 0)
                        statBoxElem.classList.add("tqItemQueryStatBoxEmpty");
                    else
                        statBoxElem.classList.remove("tqItemQueryStatBoxEmpty");
                }
            },

        }, // end methods

        components: {
        }
    });
</script>
