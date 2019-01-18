
import Idable from "../interfaces/Idable";


// Simple helper class to represent a fixed-size list of items with ids that automatically trims
// its own entries from the beginning or end as appropriate to maintain maxSize.  Also maintains
// a separate dictionary to reference entries directly by their id, and a .contains() method.
//
// (This is really only meant to be used for the master lists of ChatLines and Auctions in TunnelQuestApp.vue)

class SlidingList<T extends Idable> {

    maxSize: number;

    // (dictionary) : associative array used to directly reference an entry by its id
    dict: Array<T>;

    // ordered array of entries
    array: Array<T>;


    // constructor

    constructor() {
        this.maxSize = 1000; // set a reasonable default until we get the real setting from the server
        this.dict = new Array<T>();
        this.array = new Array<T>();
    }


    // public methods

    contains(id: number) {
        if (this.dict[id])
            return true;
        else
            return false;
    }

    addToStart(entries: Array<T>) {

        for (let i = entries.length - 1; i >= 0; i--) {
            let entry = entries[i];

            // If there's already an entry with this id, replace it with the new object
            if (this.contains(entry.id))
                this.remove(entry.id);

            this.array.unshift(entry);
            this.dict[entry.id] = entry;

            // Do NOT enforce maxSize when adding to start; let users manually add as many rows as they want
            // by repeatedly downscrolling.  This way they can quickly scroll back to top through all the cached
            // entries instead of having to repeatedly wait on loads while going back to the top.  The next time
            // addToEnd() is called by a signalr update, the maxLength will get enforced and the extra entries
            // will finally be trimmed.
            /*
            while (this.array.length > this.maxSize) {
                let removedEntry = this.array.pop();
                if (removedEntry)
                    delete this.dict[removedEntry.id];
            }
            */
        }
    }

    addToEnd(entries: Array<T>) {

        for (let i = 0; i < entries.length; i++) {
            let entry = entries[i];

            // If there's already an entry with this id, replace it with the new object
            if (this.contains(entry.id))
                this.remove(entry.id);

            this.array.push(entry);
            this.dict[entry.id] = entry;

            // enforce maxSize
            while (this.array.length > this.maxSize) {
                let removedEntry = this.array.shift();
                if (removedEntry)
                    delete this.dict[removedEntry.id];
            }
        }
    }

    consoleDump(name: string) {
        console.log(name + ".consoleDump():");

        console.log("dict:");
        console.log(this.dict);

        console.log("array:");
        console.log(this.array);
    }


    // private

    private remove(id: number) {
        // sanity check
        if (!this.contains(id))
            return;

        let entryToRemove = this.dict[id];
        let arrayIndexToRemove = this.array.indexOf(entryToRemove);

        this.array.splice(arrayIndexToRemove, 1);
        delete this.dict[id];
    }
}

export default SlidingList;