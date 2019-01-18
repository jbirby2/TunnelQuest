
interface ChatLine {
    id: number,
    playerName: string,
    text: string,
    sentAt: Date,
    auctionIds: Array<number>
}

export default ChatLine;