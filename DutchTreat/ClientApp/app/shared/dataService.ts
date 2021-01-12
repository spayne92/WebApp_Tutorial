import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core"
import { map } from "rxjs/operators";

@Injectable() // Designates class may have its own dependencies.
export class DataService {

    // Object smartly injected from Injectable w/o providers registration.
    constructor (private http: HttpClient) { }

    public products: any[] = [];

    loadProducts() {
        return this.http.get("/api/products")
            .pipe( // RXJS operators intercept and manipulate data.
                map((data: any) => {
                    this.products = data;
                    return true;
                }));
    }
}