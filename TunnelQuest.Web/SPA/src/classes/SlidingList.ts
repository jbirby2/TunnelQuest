
import Vue from "vue";
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

    sortFunction: (a: T, b: T) => number;

    // constructor

    constructor(sortFn: (a: T, b: T) => number) {
        this.maxSize = 100; // set a reasonable default until we get the real setting from the server
        this.dict = new Array<T>();
        this.array = new Array<T>();
        this.sortFunction = sortFn;
    }


    // public methods

    // entries is expected to be an associative array
    add(entries: Array<T>, enforceMaxSize: boolean) {
        
        for (let newEntryId in entries) {
            let newEntry = entries[newEntryId];

            let existingEntry = this.dict[newEntry.id];
            if (existingEntry) {
                // there's already an existing entry for this id; replace it with the new object (at its existing index in the array)

                let existingIndex = this.array.indexOf(existingEntry);
                Vue.set(this.array, existingIndex, newEntry); // same as "this.array[existingIndex] = newEntry;", but causes the UI to update with the new values
            }
            else
                this.array.push(newEntry);

            this.dict[newEntry.id] = newEntry;
        } // end for (let newEntry of entries)

        // enforce maxSize
        if (enforceMaxSize) {
            while (this.array.length > this.maxSize) {
                let removedEntry = this.array.shift();
                if (removedEntry)
                    delete this.dict[removedEntry.id];
            }
        }

        this.array.sort(this.sortFunction);
    } // end function add()


    clear() {
        while (this.array.length > 0) {
            let removedEntry = this.array.shift();
            if (removedEntry)
                delete this.dict[removedEntry.id];
        }
    }

    consoleDump(name: string) {
        console.log(name + ".consoleDump():");

        console.log("dict:");
        console.log(this.dict);

        console.log("array:");
        console.log(this.array);
    }
}

export default SlidingList;