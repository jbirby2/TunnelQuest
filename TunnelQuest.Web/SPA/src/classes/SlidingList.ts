
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


    // constructor

    constructor() {
        this.maxSize = 100; // set a reasonable default until we get the real setting from the server
        this.dict = new Array<T>();
        this.array = new Array<T>();
    }


    // public methods

    add(entries: Array<T>) {
        // STUB
        //console.log("STUB SlidingList.add()");
        //console.log(entries);

        for (let newEntryId in entries) {
            let newEntry = entries[newEntryId];

            let existingEntry = this.dict[newEntry.id];
            if (existingEntry) {
                // there's already an existing entry for this id; replace it with the new object (at its existing index in the array)

                let existingIndex = this.array.indexOf(existingEntry);
                Vue.set(this.array, existingIndex, newEntry); // same as "this.array[existingIndex] = newEntry;", but causes the UI to update with the new values
            }
            else {
                // there is not already an existing entry for this id; figure out whether to add it to the beginning or the end of the array

                if (this.array.length == 0) {
                    this.array.push(newEntry);
                }
                else {
                    if (newEntry.id < this.array[0].id) {
                        // add the new entry to the start of the array
                        this.array.unshift(newEntry);

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
                    else {
                        // add the new entry to the end of the array
                        this.array.push(newEntry);
                        
                        // enforce maxSize
                        while (this.array.length > this.maxSize) {
                            let removedEntry = this.array.shift();
                            if (removedEntry)
                                delete this.dict[removedEntry.id];
                        }
                    }
                }
            }

            this.dict[newEntry.id] = newEntry;

        } // end for (let newEntry of entries)
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