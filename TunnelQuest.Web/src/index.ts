import TunnelQuestApp from "./TunnelQuestApp.vue";

const app = new TunnelQuestApp({
    el: "#app",
    data: {
        serverCode: "BLUE",
        hubUrl: "/blue_hub"
    }
});
