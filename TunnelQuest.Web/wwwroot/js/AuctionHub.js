var AuctionHub = AuctionHub || {

    "_connection": null,

    "Init": function (hubUrl) {
        this._connection = new signalR.HubConnectionBuilder().withUrl(hubUrl).build();

        this._connection.on("HandleNewLines", function (lines) {
            // stub
            console.log(lines);

            var $stubOutput = $("#stub_output");
            $stubOutput.append(JSON.stringify(lines) + "\r\n");
            if ($stubOutput.length)
                $stubOutput.scrollTop($stubOutput[0].scrollHeight - $stubOutput.height());
        });

        this._connection.start().catch(function (err) {
            return console.error(err.toString()); // stub
        });
    }

};