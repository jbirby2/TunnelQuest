
import axios from "axios";

import Spell from "../interfaces/Spell";

class SpellRepo {

    private pendingSpellNames: Array<string> = new Array<string>();
    private spells: { [spellName: string]: Spell } = {};

    constructor() {
    }

    public get(spellName: string, fetchImmediately: boolean = true) {
        let spell = this.spells[spellName];

        if (spell)
            return spell;
        else {
            // it's important to actually declare every single property explicitly now, so that the properties exist
            // when Vue wires into them, before we get the spell data back from the server
            let blankSpell = {} as Spell;

            blankSpell.spellName = spellName;
            blankSpell.iconFileName = null;
            blankSpell.description = null;
            blankSpell.requirements = {};
            blankSpell.details = [];
            blankSpell.sources = [];

            this.spells[spellName] = blankSpell;
            this.pendingSpellNames.push(spellName);

            if (fetchImmediately)
                this.fetchPendingSpells();

            return blankSpell;
        }
    }


    public fetchPendingSpells() {
        if (this.pendingSpellNames.length == 0)
            return;

        // stub
        console.log("SpellRepo.fetchPendingSpells()");
        
        axios.post('/api/spells', { spellNames: this.pendingSpellNames })
            .then(response => {
                let result = response.data as Array<Spell>;

                // stub
                console.log(result);

                for (var spell of result) {

                    // remove the spellName from pendingSpellNames
                    if (this.pendingSpellNames.indexOf(spell.spellName) >= 0)
                        this.pendingSpellNames.splice(this.pendingSpellNames.indexOf(spell.spellName), 1);

                    // update the blank .spells[] object with the actual data
                    let blankSpell = this.spells[spell.spellName];

                    blankSpell.iconFileName = spell.iconFileName;
                    blankSpell.description = spell.description;
                    blankSpell.requirements = spell.requirements;
                    blankSpell.details = spell.details;
                    blankSpell.sources = spell.sources;
                }
            })
            .catch(err => {
                // stub
                console.log(err);
            });
    }

}

export default SpellRepo;