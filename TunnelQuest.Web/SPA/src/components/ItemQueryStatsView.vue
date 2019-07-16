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
                <input type="tel" maxlength="3" v-model="minStrength" class="tqItemQueryStatBox" @change="onStatsChanged" />
            </span>

            <span>Stamina</span>
            <span>
                <input type="tel" maxlength="3" v-model="minStamina" class="tqItemQueryStatBox" @change="onStatsChanged" />
            </span>

            <span>Agility</span>
            <span>
                <input type="tel" maxlength="3" v-model="minAgility" class="tqItemQueryStatBox" @change="onStatsChanged" />
            </span>

            <span>Dexterity</span>
            <span>
                <input type="tel" maxlength="3" v-model="minDexterity" class="tqItemQueryStatBox" @change="onStatsChanged" />
            </span>

            <span>Wisdom</span>
            <span>
                <input type="tel" maxlength="3" v-model="minWisdom" class="tqItemQueryStatBox" @change="onStatsChanged" />
            </span>

            <span>Intelligence</span>
            <span>
                <input type="tel" maxlength="3" v-model="minIntelligence" class="tqItemQueryStatBox" @change="onStatsChanged" />
            </span>

            <span>Charisma</span>
            <span>
                <input type="tel" maxlength="3" v-model="minCharisma" class="tqItemQueryStatBox" @change="onStatsChanged" />
            </span>
        </div>
        <div>
            <span>HP</span>
            <span>
                <input type="tel" maxlength="3" v-model="minHitPoints" class="tqItemQueryStatBox" @change="onStatsChanged" />
            </span>

            <span>Mana</span>
            <span>
                <input type="tel" maxlength="3" v-model="minMana" class="tqItemQueryStatBox" @change="onStatsChanged" />
            </span>

            <span>AC</span>
            <span>
                <input type="tel" maxlength="3" v-model="minArmorClass" class="tqItemQueryStatBox" @change="onStatsChanged" />
            </span>
        </div>
        <div>
            <span>Magic Resist</span>
            <span>
                <input type="tel" maxlength="3" v-model="minMagicResist" class="tqItemQueryStatBox" @change="onStatsChanged" />
            </span>

            <span>Poison Resist</span>
            <span>
                <input type="tel" maxlength="3" v-model="minPoisonResist" class="tqItemQueryStatBox" @change="onStatsChanged" />
            </span>

            <span>Disease Resist</span>
            <span>
                <input type="tel" maxlength="3" v-model="minDiseaseResist" class="tqItemQueryStatBox" @change="onStatsChanged" />
            </span>

            <span>Fire Resist</span>
            <span>
                <input type="tel" maxlength="3" v-model="minFireResist" class="tqItemQueryStatBox" @change="onStatsChanged" />
            </span>

            <span>Cold Resist</span>
            <span>
                <input type="tel" maxlength="3" v-model="minColdResist" class="tqItemQueryStatBox" @change="onStatsChanged" />
            </span>
        </div>
        <div>
            <span>Haste</span>
            <span>
                <input type="tel" maxlength="3" v-model="minHaste" class="tqItemQueryStatBox" @change="onStatsChanged" />
                <span>%</span>
            </span>
        </div>
        <div>
            <span>Singing Modifier</span>
            <span>
                <input type="tel" maxlength="3" v-model="minSingingModifier" class="tqItemQueryStatBox" @change="onStatsChanged" />
            </span>

            <span>Percussion Modifier</span>
            <span>
                <input type="tel" maxlength="3" v-model="minPercussionModifier" class="tqItemQueryStatBox" @change="onStatsChanged" />
            </span>

            <span>Stringed Modifier</span>
            <span>
                <input type="tel" maxlength="3" v-model="minStringedModifier" class="tqItemQueryStatBox" @change="onStatsChanged" />
            </span>

            <span>Brass Modifier</span>
            <span>
                <input type="tel" maxlength="3" v-model="minBrassModifier" class="tqItemQueryStatBox" @change="onStatsChanged" />
            </span>

            <span>Wind Modifier</span>
            <span>
                <input type="tel" maxlength="3" v-model="minWindModifier" class="tqItemQueryStatBox" @change="onStatsChanged" />
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
                minStrength: "",
                minStamina: "",
                minAgility: "",
                minDexterity: "",
                minWisdom: "",
                minIntelligence: "",
                minCharisma: "",
                minHitPoints: "",
                minMana: "",
                minArmorClass: "",
                minMagicResist: "",
                minPoisonResist: "",
                minDiseaseResist: "",
                minFireResist: "",
                minColdResist: "",
                minHaste: "",
                minSingingModifier: "",
                minPercussionModifier: "",
                minStringedModifier: "",
                minBrassModifier: "",
                minWindModifier: "",
            }
        },

        mounted: function () {

            if (this.stats.minStrength == null)
                this.minStrength = "";
            else
                this.minStrength = this.stats.minStrength.toString();

            if (this.stats.minStamina == null)
                this.minStamina = "";
            else
                this.minStamina = this.stats.minStamina.toString();

            if (this.stats.minAgility == null)
                this.minAgility = "";
            else
                this.minAgility = this.stats.minAgility.toString();

            if (this.stats.minDexterity == null)
                this.minDexterity = "";
            else
                this.minDexterity = this.stats.minDexterity.toString();

            if (this.stats.minWisdom == null)
                this.minWisdom = "";
            else
                this.minWisdom = this.stats.minWisdom.toString();

            if (this.stats.minIntelligence == null)
                this.minIntelligence = "";
            else
                this.minIntelligence = this.stats.minIntelligence.toString();

            if (this.stats.minCharisma == null)
                this.minCharisma = "";
            else
                this.minCharisma = this.stats.minCharisma.toString();

            if (this.stats.minHitPoints == null)
                this.minHitPoints = "";
            else
                this.minHitPoints = this.stats.minHitPoints.toString();

            if (this.stats.minMana == null)
                this.minMana = "";
            else
                this.minMana = this.stats.minMana.toString();

            if (this.stats.minArmorClass == null)
                this.minArmorClass = "";
            else
                this.minArmorClass = this.stats.minArmorClass.toString();

            if (this.stats.minMagicResist == null)
                this.minMagicResist = "";
            else
                this.minMagicResist = this.stats.minMagicResist.toString();

            if (this.stats.minPoisonResist == null)
                this.minPoisonResist = "";
            else
                this.minPoisonResist = this.stats.minPoisonResist.toString();

            if (this.stats.minDiseaseResist == null)
                this.minDiseaseResist = "";
            else
                this.minDiseaseResist = this.stats.minDiseaseResist.toString();

            if (this.stats.minFireResist == null)
                this.minFireResist = "";
            else
                this.minFireResist = this.stats.minFireResist.toString();

            if (this.stats.minColdResist == null)
                this.minColdResist = "";
            else
                this.minColdResist = this.stats.minColdResist.toString();

            if (this.stats.minHaste == null)
                this.minHaste = "";
            else
                this.minHaste = (this.stats.minHaste * 100).toString();

            if (this.stats.minSingingModifier == null)
                this.minSingingModifier = "";
            else
                this.minSingingModifier = this.stats.minSingingModifier.toString();

            if (this.stats.minPercussionModifier == null)
                this.minPercussionModifier = "";
            else
                this.minPercussionModifier = this.stats.minPercussionModifier.toString();

            if (this.stats.minStringedModifier == null)
                this.minStringedModifier = "";
            else
                this.minStringedModifier = this.stats.minStringedModifier.toString();

            if (this.stats.minBrassModifier == null)
                this.minBrassModifier = "";
            else
                this.minBrassModifier = this.stats.minBrassModifier.toString();

            if (this.stats.minWindModifier == null)
                this.minWindModifier = "";
            else
                this.minWindModifier = this.stats.minWindModifier.toString();

            // Give Vue a few milliseconds to update the haste textbox; otherwise it will
            // still be empty when we call updateStatBackgrounds().
            setTimeout(() => { this.updateStatBackgrounds(); }, 10);
        },

        methods: {

            onStatsChanged: function () {

                this.minStrength = this.minStrength.trim();
                if (this.minStrength.length == 0)
                    this.stats.minStrength = null;
                else
                    this.stats.minStrength = parseInt(this.minStrength);

                this.minStamina = this.minStamina.trim();
                if (this.minStamina.length == 0)
                    this.stats.minStamina = null;
                else
                    this.stats.minStamina = parseInt(this.minStamina);

                this.minAgility = this.minAgility.trim();
                if (this.minAgility.length == 0)
                    this.stats.minAgility = null;
                else
                    this.stats.minAgility = parseInt(this.minAgility);

                this.minDexterity = this.minDexterity.trim();
                if (this.minDexterity.length == 0)
                    this.stats.minDexterity = null;
                else
                    this.stats.minDexterity = parseInt(this.minDexterity);

                this.minWisdom = this.minWisdom.trim();
                if (this.minWisdom.length == 0)
                    this.stats.minWisdom = null;
                else
                    this.stats.minWisdom = parseInt(this.minWisdom);

                this.minIntelligence = this.minIntelligence.trim();
                if (this.minIntelligence.length == 0)
                    this.stats.minIntelligence = null;
                else
                    this.stats.minIntelligence = parseInt(this.minIntelligence);

                this.minCharisma = this.minCharisma.trim();
                if (this.minCharisma.length == 0)
                    this.stats.minCharisma = null;
                else
                    this.stats.minCharisma = parseInt(this.minCharisma);

                this.minHitPoints = this.minHitPoints.trim();
                if (this.minHitPoints.length == 0)
                    this.stats.minHitPoints = null;
                else
                    this.stats.minHitPoints = parseInt(this.minHitPoints);

                this.minMana = this.minMana.trim();
                if (this.minMana.length == 0)
                    this.stats.minMana = null;
                else
                    this.stats.minMana = parseInt(this.minMana);

                this.minArmorClass = this.minArmorClass.trim();
                if (this.minArmorClass.length == 0)
                    this.stats.minArmorClass = null;
                else
                    this.stats.minArmorClass = parseInt(this.minArmorClass);

                this.minMagicResist = this.minMagicResist.trim();
                if (this.minMagicResist.length == 0)
                    this.stats.minMagicResist = null;
                else
                    this.stats.minMagicResist = parseInt(this.minMagicResist);

                this.minPoisonResist = this.minPoisonResist.trim();
                if (this.minPoisonResist.length == 0)
                    this.stats.minPoisonResist = null;
                else
                    this.stats.minPoisonResist = parseInt(this.minPoisonResist);

                this.minDiseaseResist = this.minDiseaseResist.trim();
                if (this.minDiseaseResist.length == 0)
                    this.stats.minDiseaseResist = null;
                else
                    this.stats.minDiseaseResist = parseInt(this.minDiseaseResist);

                this.minFireResist = this.minFireResist.trim();
                if (this.minFireResist.length == 0)
                    this.stats.minFireResist = null;
                else
                    this.stats.minFireResist = parseInt(this.minFireResist);

                this.minColdResist = this.minColdResist.trim();
                if (this.minColdResist.length == 0)
                    this.stats.minColdResist = null;
                else
                    this.stats.minColdResist = parseInt(this.minColdResist);

                this.minHaste = this.minHaste.trim();
                if (this.minHaste.length == 0)
                    this.stats.minHaste = null;
                else
                    this.stats.minHaste = 0.01 * parseInt(this.minHaste);

                this.minSingingModifier = this.minSingingModifier.trim();
                if (this.minSingingModifier.length == 0)
                    this.stats.minSingingModifier = null;
                else
                    this.stats.minSingingModifier = parseInt(this.minSingingModifier);

                this.minPercussionModifier = this.minPercussionModifier.trim();
                if (this.minPercussionModifier.length == 0)
                    this.stats.minPercussionModifier = null;
                else
                    this.stats.minPercussionModifier = parseInt(this.minPercussionModifier);

                this.minStringedModifier = this.minStringedModifier.trim();
                if (this.minStringedModifier.length == 0)
                    this.stats.minStringedModifier = null;
                else
                    this.stats.minStringedModifier = parseInt(this.minStringedModifier);

                this.minBrassModifier = this.minBrassModifier.trim();
                if (this.minBrassModifier.length == 0)
                    this.stats.minBrassModifier = null;
                else
                    this.stats.minBrassModifier = parseInt(this.minBrassModifier);

                this.minWindModifier = this.minWindModifier.trim();
                if (this.minWindModifier.length == 0)
                    this.stats.minWindModifier = null;
                else
                    this.stats.minWindModifier = parseInt(this.minWindModifier);

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
