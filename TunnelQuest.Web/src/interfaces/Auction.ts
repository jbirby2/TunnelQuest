
interface Auction {
    id: number,
    itemName: string,
    isKnownItem: boolean,
    isBuying: boolean,
    price: number | null,
    isOrBestOffer: boolean,
    isAcceptingTrades: boolean,
    createdAt: Date,
    updatedAt: Date
}
export default Auction;
