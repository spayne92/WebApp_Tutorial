import { Component, OnInit } from "@angular/core"
import { DataService } from '../shared/dataService';

@Component({
    selector: "product-list",
    templateUrl: "productList.component.html",
    styleUrls: []
})
export class ProductList implements OnInit{

    // Builds private member of class and injects object.
    constructor(private data: DataService) {
    }

    public products: any[] = [];

    // Interface OnInit causes method to be called after Angular bookkeeping completed.
    ngOnInit(): void {
        // Subscribes to async HTTP request and calls method everytime it returns.
        this.data.loadProducts()
            .subscribe(success => {
                if (success) {
                    this.products = this.data.products;
                }
            })
    }
}