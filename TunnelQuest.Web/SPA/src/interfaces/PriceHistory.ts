
interface PriceHistory {
    itemName: string,
    oneMonthMedian: number | null,
    threeMonthMedian: number | null,
    sixMonthMedian: number | null,
    twelveMonthMedian: number | null,
    lifetimeMedian: number | null,
    updatedAtString: string,

    // filled in by client code
    isFetched: boolean
}

export default PriceHistory;