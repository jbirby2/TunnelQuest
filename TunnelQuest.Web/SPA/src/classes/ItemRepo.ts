
import axios from "axios";

import Item from "../interfaces/Item";

import SpellRepo from "./SpellRepo";


class ItemRepo {

    private pendingItemNames: Array<string> = new Array<string>();
    private items: { [itemName: string]: Item } = {};
    private spellRepo: SpellRepo;

    constructor(spells: SpellRepo) {
        this.spellRepo = spells;
    }

    public queue(itemName: string) {
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

            // will be manually set later by code
            blankItem.isFetched = false;
            blankItem.effectSpell = null;


            this.items[itemName] = blankItem;
            this.pendingItemNames.push(itemName);

            return blankItem;
        }
    }


    public fetchQueuedItems(callback: Function | null = null) {
        // stub
        console.log("ItemRepo.fetchQueuedItems()");

        if (this.pendingItemNames.length == 0) {
            if (callback != null)
                callback();
            return;
        }

        axios.post('/api/items', { itemNames: this.pendingItemNames.splice(0) }) // splice clears the array here
            .then(response => {
                let result = response.data as Array<Item>;

                // stub
                console.log("ItemRepo.fetchQueuedItems() result:");
                console.log(result);

                for (var item of result) {
                    // update the blank .items[] object with the actual data
                    let blankItem = this.items[item.itemName];
                    if (blankItem) {
                        blankItem.iconFileName = item.iconFileName || null;
                        blankItem.isMagic = item.isMagic || false;
                        blankItem.isLore = item.isLore || false;
                        blankItem.isNoDrop = item.isNoDrop || false;
                        blankItem.isNoTrade = item.isNoTrade || false;
                        blankItem.isTemporary = item.isTemporary || false;
                        blankItem.isQuestItem = item.isQuestItem || false;
                        blankItem.isArtifact = item.isArtifact || false;
                        blankItem.requiredLevel = item.requiredLevel || null;
                        blankItem.weight = item.weight || 0;
                        blankItem.sizeCode = item.sizeCode || null;

                        // stats

                        blankItem.strength = item.strength || null;
                        blankItem.stamina = item.stamina || null;
                        blankItem.agility = item.agility || null;
                        blankItem.dexterity = item.dexterity || null;
                        blankItem.wisdom = item.wisdom || null;
                        blankItem.intelligence = item.intelligence || null;
                        blankItem.charisma = item.charisma || null;
                        blankItem.hitPoints = item.hitPoints || null;
                        blankItem.mana = item.mana || null;
                        blankItem.armorClass = item.armorClass || null;
                        blankItem.magicResist = item.magicResist || null;
                        blankItem.poisonResist = item.poisonResist || null;
                        blankItem.diseaseResist = item.diseaseResist || null;
                        blankItem.fireResist = item.fireResist || null;
                        blankItem.coldResist = item.coldResist || null;
                        blankItem.haste = item.haste || null;

                        // bard instruments

                        blankItem.singingModifier = item.singingModifier || null;
                        blankItem.percussionModifier = item.percussionModifier || null;
                        blankItem.stringedModifier = item.stringedModifier || null;
                        blankItem.brassModifier = item.brassModifier || null;
                        blankItem.windModifier = item.windModifier || null;

                        // spell effect

                        blankItem.effectSpellName = item.effectSpellName || null;
                        blankItem.effectTypeCode = item.effectTypeCode || null;
                        blankItem.effectMinimumLevel = item.effectMinimumLevel || null;
                        blankItem.effectCastingTime = item.effectCastingTime || null;

                        // weapons

                        blankItem.weaponSkillCode = item.weaponSkillCode || null;
                        blankItem.attackDamage = item.attackDamage || null;
                        blankItem.attackDelay = item.attackDelay || null;
                        blankItem.range = item.range || null;

                        // containers

                        blankItem.capacity = item.capacity || null;
                        blankItem.capacitySizeCode = item.capacitySizeCode || null;
                        blankItem.weightReduction = item.weightReduction || null;

                        // consumables

                        blankItem.isExpendable = item.isExpendable || null;
                        blankItem.maxCharges = item.maxCharges || null;

                        // relationships

                        blankItem.races = item.races || [];
                        blankItem.classes = item.classes || [];
                        blankItem.slots = item.slots || [];
                        blankItem.deities = item.deities || [];
                        blankItem.info = item.info || [];

                        // if the item is a spell scroll, then also go ahead and pull its spell
                        if (blankItem.effectSpellName != null && blankItem.effectTypeCode == "LearnSpell")
                            blankItem.effectSpell = this.spellRepo.queue(blankItem.effectSpellName);

                        blankItem.isFetched = true;
                    }
                    else {
                        console.log("ERROR got back unexpected item.itemName: " + item.itemName);
                    }
                } // end foreach (item)

                this.spellRepo.fetchQueuedSpells(callback);
            })
            .catch(err => {
                // stub
                console.log(err);
            });
    }

}

export default ItemRepo;