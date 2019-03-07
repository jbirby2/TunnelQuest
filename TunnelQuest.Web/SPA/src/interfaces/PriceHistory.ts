
interface PriceHistory {
    itemName: string,
    oneMonthMedian: number | null,
    threeMonthMedian: number | null,
    sixMonthMedian: number | null,
    twelveMonthMedian: number | null,
    lifetimeMedian: number | null,
    updatedAtString: string
}

export default PriceHistory;