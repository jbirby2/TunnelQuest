
import * as signalR from "@aspnet/signalr";

// ConnectionWrapper is a simple wrapper for a signalR connection that provides additional 
// functionality and safety checks.

class ConnectionWrapper {
    
    private connection: signalR.HubConnection | null;
    private isConnecting: boolean = false;
    private onConnectedCallbacks: Array<Function>;
    private onDisconnectedCallbacks: Array<Function>;

    constructor() {
        this.connection = null;
        this.onConnectedCallbacks = new Array<Function>();
        this.onDisconnectedCallbacks = new Array<Function>();
    }

    public setHubUrl(hubUrl: string) {
        this.connection = new signalR.HubConnectionBuilder()
            .withUrl(hubUrl)
            .build();
    }

    public isConnected() {
        return (this.isConnecting || this.connection == null || this.connection.state == signalR.HubConnectionState.Connected);
    }



    public on(serverMessage: string, callback: (...args: any[]) => void) {
        if (this.connection == null)
            throw new Error("must call ConnectionWrapper.setHubUrl() first");

        this.connection.on(serverMessage, callback);
    }

    public off(serverMessage: string, callback: (...args: any[]) => void) {
        if (this.connection == null)
            throw new Error("must call ConnectionWrapper.setHubUrl() first");

        this.connection.off(serverMessage, callback);
    }



    public onConnected(callback: Function) {
        if (this.onConnectedCallbacks.indexOf(callback) < 0)
            this.onConnectedCallbacks.push(callback);
    }

    public offConnected(callback: Function) {
        let index = this.onConnectedCallbacks.indexOf(callback);
        if (index >= 0)
            this.onConnectedCallbacks.splice(index, 1);
    }



    public onDisconnected(callback: Function) {
        if (this.onDisconnectedCallbacks.indexOf(callback) < 0)
            this.onDisconnectedCallbacks.push(callback);
    }

    public offDisconnected(callback: Function) {
        let index = this.onDisconnectedCallbacks.indexOf(callback);
        if (index >= 0)
            this.onDisconnectedCallbacks.splice(index, 1);
    }



    public connect() {
        if (this.connection == null)
            throw new Error("must call ConnectionWrapper.setHubUrl() first");

        if (this.isConnected())
            return; // already connected

        this.isConnecting = true;

        // stub
        console.log("connecting");

        this.connection
            .start()
            .then(() => {
                // stub
                console.log("connected");

                for (var callback of this.onConnectedCallbacks) {
                    callback();
                }

                this.isConnecting = false;
            })
            .catch(err => {
                // stub
                console.log(err);
                this.isConnecting = false;
            });
    }

    public disconnect() {
        if (this.connection == null)
            throw new Error("must call ConnectionWrapper.setHubUrl() first");

        if (!this.isConnected())
            return; // already disconnected

        // stub
        console.log("disconnecting");

        this.connection
            .stop()
            .then(() => {
                // stub
                console.log("disconnected");

                for (var callback of this.onDisconnectedCallbacks) {
                    callback();
                }
            })
            .catch(err => {
                // stub
                console.log(err);
            });
    }

    public destroy() {
        this.connection = null;
    }
}

export default ConnectionWrapper;