
import ChatLineToken from "./ChatLineToken";

interface ChatLine {
    id: number,
    playerName: string,
    text: string,
    tokens: Array<ChatLineToken>,
    sentAtString: string
}

export default ChatLine;