
import Auction from "../interfaces/Auction";

interface ChatLine {
    id: number,
    playerName: string,
    text: string,
    sentAtString: string,
    auctionIds: Array<number>,

    // not actually passed in from server, but set in client code
    auctions: Array<Auction>
}

export default ChatLine;