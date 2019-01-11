
import Auction from "./Auction";

interface ChatLine {
    id: number,
    playerName: string,
    text: string,
    sentAt: Date,
    auctions: Auction[]
}

export default ChatLine;