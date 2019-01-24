
import * as moment from "moment";
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

    chatLine: ChatLine,                 // not actually passed in from server, but set in client code
    updatedAtMoment: moment.Moment,     // not actually passed in from server, but set in client code
    firstSeenDate: Date,                // not actually passed in from server, but set in client code
}
export default Auction;
