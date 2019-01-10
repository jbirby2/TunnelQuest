"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
require("./css/main.css");
var signalR = require("@aspnet/signalr");
//const divMessages: HTMLDivElement = document.querySelector("#divMessages");
//const tbMessage: HTMLInputElement = document.querySelector("#tbMessage");
//const btnSend: HTMLButtonElement = document.querySelector("#btnSend");
//const username = new Date().getTime();
var stub_output = document.querySelector("#stub_output");
var connection = new signalR.HubConnectionBuilder()
    .withUrl("/blue_hub")
    .build();
connection.start().catch(function (err) { return console.log(err); }); // stub
connection.on("HandleNewLines", function (lines) {
    /*
    let m = document.createElement("div");

    m.innerHTML =
        `<div class="message-author">${username}</div><div>${message}</div>`;

    divMessages.appendChild(m);
    divMessages.scrollTop = divMessages.scrollHeight;
    */
    // stub
    stub_output.innerText += JSON.stringify(lines) + "\n";
    stub_output.scrollTop = stub_output.scrollHeight;
    // stub
    console.log(lines);
});
/*
tbMessage.addEventListener("keyup", (e: KeyboardEvent) => {
    if (e.keyCode === 13) {
        send();
    }
});

btnSend.addEventListener("click", send);
*/ 
