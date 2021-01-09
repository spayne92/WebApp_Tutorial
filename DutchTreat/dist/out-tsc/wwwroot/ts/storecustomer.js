"use strict";
// Exports ONLY the tagged classes. Removes from global scope.
//export class StoreCustomer {
// Requires loader like webpack, but we aren't, so using global JS scope.
class StoreCustomer {
    // Adds private class parameters of types.
    constructor(firstName, lastName) {
        this.firstName = firstName;
        this.lastName = lastName;
        // Syntax for type specification.
        this.visits = 0;
    } // Private fields, set, and wired up to be used auto-magically.
    // Syntax for method return type.
    showName() {
        alert(this.firstName + " " + this.lastName);
        return true;
    }
}
let cust = new StoreCustomer("John", "Doe");
//cust.visits = "test"; // Doesn't work.
cust.visits = 10; // Works, because type correct.
// Types checked during coding/compiling, but not enforce dat run time.
//# sourceMappingURL=storecustomer.js.map