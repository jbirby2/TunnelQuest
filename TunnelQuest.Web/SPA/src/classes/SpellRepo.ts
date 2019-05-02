
import axios from "axios";

import Spell from "../interfaces/Spell";

class SpellRepo {

    private pendingSpellNames: Array<string> = new Array<string>();
    private spells: { [spellName: string]: Spell } = {};

    constructor() {
    }

    public queue(spellName: string) {
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

            return blankSpell;
        }
    }


    public getQueuedSpellsAsync(callback: Function | null = null) {
        // stub
        console.log("SpellRepo.getQueuedSpellsAsync()");

        if (this.pendingSpellNames.length == 0) {
            if (callback != null)
                callback();
            return;
        }

        axios.post('/api/spells', { spellNames: this.pendingSpellNames.splice(0) }) // splice clears the array here
            .then(response => {
                let result = response.data as Array<Spell>;

                // stub
                console.log("ItemRepo.getQueuedSpellsAsync() result:");
                console.log(result);

                for (var spell of result) {
                    // update the blank .spells[] object with the actual data
                    let blankSpell = this.spells[spell.spellName];

                    if (blankSpell) {
                        blankSpell.iconFileName = spell.iconFileName || null;
                        blankSpell.description = spell.description || null;
                        blankSpell.requirements = spell.requirements || {};
                        blankSpell.details = spell.details || [];
                        blankSpell.sources = spell.sources || [];
                    }
                    else {
                        console.log("ERROR got back unexpected spell.spellName: " + spell.spellName);
                    }
                }

                if (callback != null)
                    callback();
            })
            .catch(err => {
                // stub
                console.log(err);
            });
    }

}

export default SpellRepo;