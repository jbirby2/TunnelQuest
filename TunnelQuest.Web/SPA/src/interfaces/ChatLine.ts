
import Auction from "./Auction";

interface ChatLine {
    id: number,
    playerName: string,
    text: string,
    auctions: {[auctionId:number]:Auction},
    sentAtString: string
}

export default ChatLine;