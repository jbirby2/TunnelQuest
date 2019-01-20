
interface ChatLine {
    id: number,
    playerName: string,
    text: string,
    sentAtString: string,
    auctionIds: Array<number>
}

export default ChatLine;