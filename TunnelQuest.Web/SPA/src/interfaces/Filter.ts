
import ItemQuery from "./ItemQuery";

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
        items: ItemQuery,
        minPrice: number | null,
        maxPrice: number | null,
        minGoodPriceDeviation: number | null,
        maxBadPriceDeviation: number | null,
    }
}

export default Filter;