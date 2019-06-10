
import ItemQueryStats from "./ItemQueryStats";

interface ItemQuery {
    queryType: string, // valid values: "name", "stats"
    names: Array<string>,
    stats: ItemQueryStats
}

export default ItemQuery;