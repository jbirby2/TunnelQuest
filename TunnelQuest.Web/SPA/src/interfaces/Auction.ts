
import * as moment from "moment";
import ChatLine from "../interfaces/ChatLine";
import Item from "../interfaces/Item";

interface Auction {
    id: number,
    previousAuctionId: number | null,
    itemName: string,
    isKnownItem: boolean,
    isBuying: boolean,
    price: number | null,
    isOrBestOffer: boolean,
    isAcceptingTrades: boolean,
    createdAtString: string,
    updatedAtString: string,
    chatLineId: number,

    chatLine: ChatLine,                 // not actually passed in from server, but set in client code
    item: Item,                         // not actually passed in from server, but set in client code
    updatedAtMoment: moment.Moment,     // not actually passed in from server, but set in client code
    firstSeenDate: Date,                // not actually passed in from server, but set in client code
    isPreviousAuction: boolean          // not actually passed in from server, but set in client code
}
export default Auction;
