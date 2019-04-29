
import axios from "axios";

import PriceHistory from "../interfaces/PriceHistory";

class PriceHistoryRepo {

    private serverCode: string;
    private pendingItemNames: Array<string> = new Array<string>();
    private priceHistories: { [itemName: string]: PriceHistory } = {};

    constructor(serverCode: string) {
        this.serverCode = serverCode;
    }

    public queue(itemName: string) {
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

            return blankPriceHistory;
        }
    }


    public fetchQueuedPriceHistories(callback: Function | null = null) {
        
        // stub
        console.log("PriceHistoryRepo.fetchPendingPriceHistories()");

        if (this.pendingItemNames.length == 0) {
            if (callback != null)
                callback();
            return;
        }

        axios.post('/api/price_history', {
                itemNames: this.pendingItemNames.splice(0), // splice clears the array here
                serverCode: this.serverCode
            }) 
            .then(response => {
                let result = response.data as Array<PriceHistory>;

                // stub
                console.log("PriceHistoryRepo.fetchPendingPriceHistories()");
                console.log(result);

                for (var priceHistory of result) {
                    // update the blank .priceHistories[] object with the actual data
                    let blankPriceHistory = this.priceHistories[priceHistory.itemName];

                    if (blankPriceHistory) {
                        blankPriceHistory.isFetched = true;
                        blankPriceHistory.oneMonthMedian = priceHistory.oneMonthMedian || null;
                        blankPriceHistory.threeMonthMedian = priceHistory.threeMonthMedian || null;
                        blankPriceHistory.sixMonthMedian = priceHistory.sixMonthMedian || null;
                        blankPriceHistory.twelveMonthMedian = priceHistory.twelveMonthMedian || null;
                        blankPriceHistory.lifetimeMedian = priceHistory.lifetimeMedian || null;
                        blankPriceHistory.updatedAtString = priceHistory.updatedAtString;
                    }
                    else {
                        console.log("ERROR got back unexpected priceHistory.itemName: " + priceHistory.itemName);
                    }
                }

                if (callback)
                    callback();
            })
            .catch(err => {
                // stub
                console.log(err);
            });
    }

}

export default PriceHistoryRepo;