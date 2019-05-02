
interface Filter {
    id: string,
    isSystem: boolean,
    name: string,
    metaData: {
        itemIsKnown: { [itemName: string]: boolean }
    },
    settings: {
        isPermanent: boolean | null,
        playerName: string | null,
        isBuying: boolean | null,
        items: {
            filterType: string, // valid values: "name", "stats"
            names: Array<string>,
            stats: {
                minStrength: number | null,
                minStamina: number | null,
                minAgility: number | null,
                minDexterity: number | null,
                minWisdom: number | null,
                minIntelligence: number | null,
                minCharisma: number | null,
            }
        },
        minPrice: number | null,
        maxPrice: number | null,
        minGoodPriceDeviation: number | null,
        maxBadPriceDeviation: number | null,
    }
}

export default Filter;