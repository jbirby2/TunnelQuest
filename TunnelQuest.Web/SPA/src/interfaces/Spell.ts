

interface Spell {
    spellName: string,
    iconFileName: string | null,
    description: string | null,
    requirements: { [classCode: string]: number },
    details: Array<string>,
    sources: Array<string>
}

export default Spell;
