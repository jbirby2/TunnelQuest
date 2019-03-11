
import axios from "axios";

import PriceHistory from "../interfaces/PriceHistory";

class PriceHistoryRepo {

    private pendingItemNames: Array<string> = new Array<string>();
    private priceHistories: { [itemName: string]: PriceHistory } = {};

    constructor() {
    }

    public get(itemName: string, fetchImmediately: boolean = true) {
        let priceHistory = this.priceHistories[itemName];

        if (priceHistory)
            return priceHistory;
        else {
            // it's important to actually declare every single property explicitly now, so that the properties exist
            // when Vue wires into them, before we get the price history data back from the server
            let blankPriceHistory = {} as PriceHistory;

            blankPriceHistory.isFetched = false;
            blankPriceHistory.itemName = itemName;
            blankPriceHistory.oneMonthMedian = null;
            blankPriceHistory.threeMonthMedian = null;
            blankPriceHistory.sixMonthMedian = null;
            blankPriceHistory.twelveMonthMedian = null;
            blankPriceHistory.lifetimeMedian = null;
            blankPriceHistory.updatedAtString = "";


            this.priceHistories[itemName] = blankPriceHistory;
            this.pendingItemNames.push(itemName);

            if (fetchImmediately)
                this.fetchPendingPriceHistories();

            return blankPriceHistory;
        }
    }


    public fetchPendingPriceHistories() {
        if (this.pendingItemNames.length == 0)
            return;

        // stub
        console.log("PriceHistoryRepo.fetchPendingPriceHistories()");
        
        axios.post('/api/price_history', { itemNames: this.pendingItemNames.splice(0) }) // splice clears the array here
            .then(response => {
                let result = response.data as Array<PriceHistory>;

                // stub
                console.log(result);

                for (var priceHistory of result) {
                    // update the blank .priceHistories[] object with the actual data
                    let blankPriceHistory = this.priceHistories[priceHistory.itemName];

                    blankPriceHistory.isFetched = true;
                    blankPriceHistory.oneMonthMedian = priceHistory.oneMonthMedian;
                    blankPriceHistory.threeMonthMedian = priceHistory.threeMonthMedian;
                    blankPriceHistory.sixMonthMedian = priceHistory.sixMonthMedian;
                    blankPriceHistory.twelveMonthMedian = priceHistory.twelveMonthMedian;
                    blankPriceHistory.lifetimeMedian = priceHistory.lifetimeMedian;
                    blankPriceHistory.updatedAtString = priceHistory.updatedAtString;
                }
            })
            .catch(err => {
                // stub
                console.log(err);
            });
    }

}

export default PriceHistoryRepo;