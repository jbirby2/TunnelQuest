
interface Filter {

    // STUB TODO delete this and its vue, etc
    matchAny: boolean,

    id: string,
    isSystem: boolean,
    name: string,
    settings: {
        itemNames: Array<string>,
        playerName: string | null,
        isBuying: boolean | null,
        minPrice: number | null,
        maxPrice: number | null,
        minGoodPriceDeviation: number | null,
        maxBadPriceDeviation: number | null,
        // stub add more filters
    }
}

export default Filter;