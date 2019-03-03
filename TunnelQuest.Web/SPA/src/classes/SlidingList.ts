
import Vue from "vue";
import Idable from "../interfaces/Idable";


// Simple helper class to represent a fixed-size list of objects with ids that automatically trims
// its earliest entries to maintain maxSize.  
//
// Also maintains a separate dictionary to reference entries directly by their id.
//
// (This is really only meant to be used for the master lists of ChatLines and Auctions)

class SlidingList<T extends Idable> {

    maxSize: number;

    // (dictionary) : associative array used to directly reference an entry by its id
    dict: Map<number, T>;

    // ordered array of entries
    array: Array<T>;

    sortFunction: (a: T, b: T) => number;

    // constructor

    constructor(sortFn: (a: T, b: T) => number) {
        this.maxSize = 100; // set a reasonable default until we get the real setting from the server
        this.dict = new Map<number, T>();
        this.array = new Array<T>();
        this.sortFunction = sortFn;
    }


    // public methods

    add(newEntry:T) {
        let existingEntry = this.dict.get(newEntry.id);
        if (existingEntry) {
            // there's already an existing entry for this id; replace it with the new object (at its existing index in the array)
            let existingIndex = this.array.indexOf(existingEntry);
            Vue.set(this.array, existingIndex, newEntry); // same as "this.array[existingIndex] = newEntry;", but causes the UI to update with the new values
        }
        else
            this.array.push(newEntry);

        this.dict.set(newEntry.id, newEntry);
    }

    enforceMaxSize() {
        while (this.array.length > this.maxSize) {
            let removedEntry = this.array.shift();
            if (removedEntry)
                this.dict.delete(removedEntry.id);
        }
    }

    sort() {
        // sort before enforcing maxSize to make sure we're always removing the entries from the start of the list
        // according to the provided sorting function
        this.array.sort(this.sortFunction);
    }

    remove(id: number) {
        let existingEntry = this.dict.get(id);
        if (existingEntry) {
            this.dict.delete(id);

            let existingIndex = this.array.indexOf(existingEntry);
            this.array.splice(existingIndex, 1);
        }
    }

    clear() {
        while (this.array.length > 0) {
            let removedEntry = this.array.pop();
            if (removedEntry)
                this.dict.delete(removedEntry.id);
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