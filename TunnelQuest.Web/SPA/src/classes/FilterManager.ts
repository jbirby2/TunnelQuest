
import Filter from "../interfaces/Filter";

class FilterManager {

    private readonly FILTER_NAME_BASE = "Custom Filter ";
    private selectedFilterChangedCallbacks: Array<Function>;

    public filters: Array<Filter>;
    public selectedFilter: Filter; // everybody is on scout's honor to use setSelectedFilter() instead of setting this directly; exposed so FilterManagerView can bind to it

    constructor() {
        this.filters = new Array<Filter>();
        this.selectedFilterChangedCallbacks = new Array<Function>();

        // create system filters

        this.filters.push(this.createEmptyFilter("All Auctions", true));

        let goodDealsFilter = this.createEmptyFilter("Good Deals Only", true);
        goodDealsFilter.settings.minGoodPriceDeviation = 1;
        this.filters.push(goodDealsFilter);

        // set initial filter
        this.selectedFilter = this.filters[0];
    }

    // public

    public setSelectedFilter(filter: Filter) {
        this.selectedFilter = filter;

        for (let callback of this.selectedFilterChangedCallbacks) {
            callback();
        }
    }

    public onSelectedFilterChanged(callback: Function) {
        if (this.selectedFilterChangedCallbacks.indexOf(callback) < 0)
            this.selectedFilterChangedCallbacks.push(callback);
    }

    public createNewUserFilter() {
        let newFilter = this.createEmptyFilter(this.generateName(), false);
        this.filters.push(newFilter);
        return newFilter;
    }

    public get(id: string) {
        for (let filter of this.filters) {
            if (filter.id == id)
                return filter;
        }
        return null;
    }

    // private

    private createEmptyFilter(name: string, isSystem: boolean) {
        return {
            id: this.generateId(),
            name: name,
            isSystem: isSystem,
            settings: {
                itemNames: new Array<string>()
            }
        } as Filter;
    }

    private generateId() {
        var result, i, j;
        result = '';
        for (j = 0; j < 32; j++) {
            if (j == 8 || j == 12 || j == 16 || j == 20)
                result = result + '-';
            i = Math.floor(Math.random() * 16).toString(16).toUpperCase();
            result = result + i;
        }
        return result;
    }

    public generateName() {
        let uniqueName = false;
        let counter = 1;
        let newName = "";
        while (!uniqueName) {
            newName = this.FILTER_NAME_BASE + counter.toString();
            uniqueName = true;
            for (let existingFilter of this.filters) {
                if (existingFilter.name == newName)
                    uniqueName = false;
            }
            counter++;
        }
        return newName;
    }

}

export default FilterManager;