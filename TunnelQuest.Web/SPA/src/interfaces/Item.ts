
import ChatLineAuctionInfo from "../interfaces/ChatLineAuctionInfo";

interface ChatLine {
    itemName: string,
    iconFileName: string | null,
    isMagic: boolean,
    isLore: boolean,
    isNoDrop: boolean,
    isNoTrade: boolean,
    isTemporary: boolean,
    isQuestItem: boolean,
    isArtifact: boolean,
    requiredLevel: number | null,
    weight: number,
    sizeCode: string | null,

    // stats

    strength: number | null,
    stamina: number | null,
    agility: number | null,
    dexterity: number | null,
    wisdom: number | null,
    intelligence: number | null,
    charisma: number | null,
    hitPoints: number | null,
    mana: number | null,
    armorClass: number | null,
    magicResist: number | null,
    poisonResist: number | null,
    diseaseResist: number | null,
    fireResist: number | null,
    coldResist: number | null,
    haste: number | null,

    // bard instruments

    singingModifier: number | null,
    percussionModifier: number | null,
    stringedModifier: number | null,
    brassModifier: number | null,
    windModifier: number | null,

    // spell effect

    effectSpellName: string | null,
    effectTypeCode: string | null,
    effectMinimumLevel: number | null,
    effectCastingTime: number | null,

    // weapons

    weaponSkillCode: string | null,
    attackDamage: number | null,
    attackDelay: number | null,
    range: number | null,

    // containers

    capacity: number | null,
    capacitySizeCode: string | null,
    weightReduction: number | null,

    // consumables

    isExpendable: boolean | null,
    maxCharges: number | null,

    // relationships

    races: Array<string>,
    classes: Array<string>,
    slots: Array<string>,
    deities: Array<string>,
    info: Array<string>
}

export default ChatLine;