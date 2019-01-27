
import axios from "axios";

import Item from "../interfaces/Item";

class ItemRepo {

    private pendingItemNames: Array<string> = new Array<string>();
    private fetchTimer: number = -1;
    private items: { [itemName: string]: Item } = {};

    constructor() {
        this.fetchTimer = setInterval(() => this.fetchPendingItems, 5000);
    }

    public get(itemName: string, fetchImmediately: boolean = true) {
        let item = this.items[itemName];

        if (item)
            return item;
        else {
            // it's important to actually declare every single property explicitly now, so that the properties exist
            // when Vue wires into them, before we get the item data back from the server
            let blankItem = {} as Item;

            blankItem.itemName = itemName;

            blankItem.iconFileName = null;
            blankItem.isMagic = false;
            blankItem.isLore = false;
            blankItem.isNoDrop = false;
            blankItem.isNoTrade = false;
            blankItem.isTemporary = false;
            blankItem.isQuestItem = false;
            blankItem.isArtifact = false;
            blankItem.requiredLevel = null;
            blankItem.weight = 0;
            blankItem.sizeCode = "";

            // stats

            blankItem.strength = null;
            blankItem.stamina = null;
            blankItem.agility = null;
            blankItem.dexterity = null;
            blankItem.wisdom = null;
            blankItem.intelligence = null;
            blankItem.charisma = null;
            blankItem.hitPoints = null;
            blankItem.mana = null;
            blankItem.armorClass = null;
            blankItem.magicResist = null;
            blankItem.poisonResist = null;
            blankItem.diseaseResist = null;
            blankItem.fireResist = null;
            blankItem.coldResist = null;
            blankItem.haste = null;

            // bard instruments

            blankItem.singingModifier = null;
            blankItem.percussionModifier = null;
            blankItem.stringedModifier = null;
            blankItem.brassModifier = null;
            blankItem.windModifier = null;

            // spell effect

            blankItem.effectSpellName = null;
            blankItem.effectTypeCode = null;
            blankItem.effectMinimumLevel = null;
            blankItem.effectCastingTime = null;

            // weapons

            blankItem.weaponSkillCode = null;
            blankItem.attackDamage = null;
            blankItem.attackDelay = null;
            blankItem.range = null;

            // containers

            blankItem.capacity = null;
            blankItem.capacitySizeCode = null;
            blankItem.weightReduction = null;

            // consumables

            blankItem.isExpendable = null;
            blankItem.maxCharges = null;

            // relationships

            blankItem.races = [];
            blankItem.classes = [];
            blankItem.slots = [];
            blankItem.deities = [];
            blankItem.info = [];

            this.items[itemName] = blankItem;
            this.pendingItemNames.push(itemName);

            if (fetchImmediately)
                this.fetchPendingItems();

            return blankItem;
        }
    }


    public fetchPendingItems() {
        if (this.pendingItemNames.length == 0)
            return;

        // stub
        console.log("ItemRepo.fetchPendingItems()");
        
        axios.post('/api/items', { itemNames: this.pendingItemNames })
            .then(response => {
                let result = response.data as Array<Item>;

                // stub
                console.log(result);

                for (var item of result) {

                    // remove the itemName from pendingItemNames
                    this.pendingItemNames.splice(this.pendingItemNames.indexOf(item.itemName), 1);

                    // update the blank .items[] object with the actual data
                    let blankItem = this.items[item.itemName];

                    blankItem.iconFileName = item.iconFileName;
                    blankItem.isMagic = item.isMagic;
                    blankItem.isLore = item.isLore;
                    blankItem.isNoDrop = item.isNoDrop;
                    blankItem.isNoTrade = item.isNoTrade;
                    blankItem.isTemporary = item.isTemporary;
                    blankItem.isQuestItem = item.isQuestItem;
                    blankItem.isArtifact = item.isArtifact;
                    blankItem.requiredLevel = item.requiredLevel;
                    blankItem.weight = item.weight;
                    blankItem.sizeCode = item.sizeCode;

                    // stats

                    blankItem.strength = item.strength;
                    blankItem.stamina = item.stamina;
                    blankItem.agility = item.agility;
                    blankItem.dexterity = item.dexterity;
                    blankItem.wisdom = item.wisdom;
                    blankItem.intelligence = item.intelligence;
                    blankItem.charisma = item.charisma;
                    blankItem.hitPoints = item.hitPoints;
                    blankItem.mana = item.mana;
                    blankItem.armorClass = item.armorClass;
                    blankItem.magicResist = item.magicResist;
                    blankItem.poisonResist = item.poisonResist;
                    blankItem.diseaseResist = item.diseaseResist;
                    blankItem.fireResist = item.fireResist;
                    blankItem.coldResist = item.coldResist;
                    blankItem.haste = item.haste;

                    // bard instruments

                    blankItem.singingModifier = item.singingModifier;
                    blankItem.percussionModifier = item.percussionModifier;
                    blankItem.stringedModifier = item.stringedModifier;
                    blankItem.brassModifier = item.brassModifier;
                    blankItem.windModifier = item.windModifier;

                    // spell effect

                    blankItem.effectSpellName = item.effectSpellName;
                    blankItem.effectTypeCode = item.effectTypeCode;
                    blankItem.effectMinimumLevel = item.effectMinimumLevel;
                    blankItem.effectCastingTime = item.effectCastingTime;

                    // weapons

                    blankItem.weaponSkillCode = item.weaponSkillCode;
                    blankItem.attackDamage = item.attackDamage;
                    blankItem.attackDelay = item.attackDelay;
                    blankItem.range = item.range;

                    // containers

                    blankItem.capacity = item.capacity;
                    blankItem.capacitySizeCode = item.capacitySizeCode;
                    blankItem.weightReduction = item.weightReduction;

                    // consumables

                    blankItem.isExpendable = item.isExpendable;
                    blankItem.maxCharges = item.maxCharges;

                    // relationships

                    blankItem.races = item.races;
                    blankItem.classes = item.classes;
                    blankItem.slots = item.slots;
                    blankItem.deities = item.deities;
                    blankItem.info = item.info;
                }
            })
            .catch(err => {
                // stub
                console.log(err);
            });
    }

}

export default ItemRepo;