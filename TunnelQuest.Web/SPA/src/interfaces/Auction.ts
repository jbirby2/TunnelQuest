
import ChatLine from "../interfaces/ChatLine";

interface Auction {
    id: number,
    itemName: string,
    isKnownItem: boolean,
    isBuying: boolean,
    price: number | null,
    isOrBestOffer: boolean,
    isAcceptingTrades: boolean,
    createdAtString: string,
    updatedAtString: string,
    chatLineId: number,

    // not actually passed in from server, but set in client code
    chatLine: ChatLine
}
export default Auction;
