
import * as signalR from "@aspnet/signalr";
import axios from "axios";

import Settings from "../interfaces/Settings";


class TQGlobals {

    private static isInitialized: boolean = false;

    static serverCode: string;
    static settings: Settings;
    static connection: signalR.HubConnection;


    static init(done:Function|null = null) {
        if (this.isInitialized) {
            if (done)
                done();
        }
        else {
            // do the initialization

            console.log("stub TQGlobals initializing");

            this.serverCode = "BLUE"; // STUB hard-coded
            let hubUrl = "/blue_hub"; // STUB hard-coded

            this.connection = new signalR.HubConnectionBuilder()
                .withUrl(hubUrl)
                .build();

            axios.get('/api/settings')
                .then(response => {
                    this.settings = response.data as Settings;

                    this.isInitialized = true;

                    if (done)
                        done();
                })
                .catch(err => {
                    // stub
                    console.log(err);
                }); // end axios.get(settings)
        }
    }
}

export default TQGlobals;