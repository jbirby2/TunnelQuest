
import ChatLineAuctionInfo from "../interfaces/ChatLineAuctionInfo";

interface ChatLine {
    id: number,
    playerName: string,
    text: string,
    sentAtString: string,
    auctions: Array<ChatLineAuctionInfo>
}

export default ChatLine;