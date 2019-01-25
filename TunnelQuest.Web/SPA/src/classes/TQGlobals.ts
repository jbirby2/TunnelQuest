
import axios from "axios";

import Settings from "../interfaces/Settings";

import ConnectionWrapper from "../classes/ConnectionWrapper";
import ItemRepo from "../classes/ItemRepo";


class TQGlobals {

    private static isInitialized: boolean = false;
    private static onInitCallbacks: Array<Function> = new Array<Function>();

    static serverCode: string;
    static settings: Settings;
    static connection: ConnectionWrapper;
    static items: ItemRepo;

    static init() {
        if (this.isInitialized)
            return;

        console.log("stub TQGlobals initializing");

        this.serverCode = "BLUE"; // STUB hard-coded
        let hubUrl = "/blue_hub"; // STUB hard-coded

        this.connection = new ConnectionWrapper(hubUrl);
        this.items = new ItemRepo();

        axios.get('/api/settings')
            .then(response => {
                this.settings = response.data as Settings;
                this.isInitialized = true;
                for (let callback of this.onInitCallbacks) {
                    callback();
                }
            })
            .catch(err => {
                // stub
                console.log(err);
            }); // end axios.get(settings)
    }

    static onInit(callback: Function) {
        this.onInitCallbacks.push(callback);

        // if we're already initialized, callback immediately
        if (this.isInitialized)
            callback();
    }
}

export default TQGlobals;