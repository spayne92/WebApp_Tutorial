import { Component } from "@angular/core"
import { DataService } from '../shared/dataService';

@Component({
    selector: "product-list",
    templateUrl: "productList.component.html",
    styleUrls: []
})
export class ProductList {

    // Builds private member of class and injects object.
    constructor(private data: DataService) {
        this.products = data.products;
    }

    public products;
}