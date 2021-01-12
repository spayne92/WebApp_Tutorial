import { __decorate } from "tslib";
import { Component } from "@angular/core";
let ProductList = class ProductList {
    // Builds private member of class and injects object.
    constructor(data) {
        this.data = data;
        this.products = data.products;
    }
};
ProductList = __decorate([
    Component({
        selector: "product-list",
        templateUrl: "productList.component.html",
        styleUrls: []
    })
], ProductList);
export { ProductList };
//# sourceMappingURL=productList.component.js.map