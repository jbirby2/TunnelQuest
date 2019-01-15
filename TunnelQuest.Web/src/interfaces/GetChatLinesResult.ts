
import ChatLine from "./ChatLine";
import Auction from "./Auction";

interface GetChatLinesResult {
    lines: Array<ChatLine>,
    auctions: Array<Auction>
}

export default GetChatLinesResult;