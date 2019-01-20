
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
    chatLineId: number
}
export default Auction;
