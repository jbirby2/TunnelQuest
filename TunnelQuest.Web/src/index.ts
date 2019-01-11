import "./css/main.css";
import TunnelQuestApp from "./TunnelQuestApp.vue";

const app = new TunnelQuestApp({
    el: "#app",
    data: {
        hubUrl: "/blue_hub"
    }
});
