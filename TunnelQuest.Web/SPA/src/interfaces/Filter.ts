
interface Filter {
    id: string,
    isSystem: boolean,
    name: string,
    metaData: {
        itemIsKnown: { [itemName: string]: boolean }
    },
    settings: {
        isPermanent: boolean | null,
        itemNames: Array<string>,
        playerName: string | null,
        isBuying: boolean | null,
        minPrice: number | null,
        maxPrice: number | null,
        minGoodPriceDeviation: number | null,
        maxBadPriceDeviation: number | null,
    }
}

export default Filter;