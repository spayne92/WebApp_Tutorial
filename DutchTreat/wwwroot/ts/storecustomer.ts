
// Exports ONLY the tagged classes. Removes from global scope.
//export class StoreCustomer {
// Requires loader like webpack, but we aren't, so using global JS scope.

class StoreCustomer {

    // Adds private class parameters of types.
    constructor(private firstName:string, private lastName:string) {
    } // Private fields, set, and wired up to be used auto-magically.

    // Syntax for type specification.
    public visits:number = 0;
    private ourName:string;

    // Syntax for method return type.
    public showName():boolean {
        alert(this.firstName + " " + this.lastName);
        return true;
    }

    set name(val) {
        this.ourName = val;    
    }

    get name() {
        return this.ourName;
    }
}

let cust = new StoreCustomer("John", "Doe");
//cust.visits = "test"; // Doesn't work.
cust.visits = 10;   // Works, because type correct.
// Types checked during coding/compiling, but not enforce dat run time.