console.log("Hello Pluralsight");

var theForm = document.getElementById("theForm");
theForm.hidden = true;

var button = document.getElementById("buyButton");
// "Add"-ing, on top of other listeners.
button.addEventListener("click", function () { // Anonymous function declared in-line to be executed on click.
    console.log("Buying Item");
});
// Anonymous function is an unnamed method much like a C# lamda. Useful for when a method will not be reused.


// Drilling into individual elements from a class name becomes difficult.
// Time to introduce JQuery.
var productInfo = document.getElementsByClassName("product-props");
var listItems = productInfo.item[0].children;