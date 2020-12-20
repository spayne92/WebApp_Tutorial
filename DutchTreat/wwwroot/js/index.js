// Immediately Invoked Function Expression (IIFE) contains JS to HTML page it is referenced in. Normally global scope.
$(document).ready(function () { // jQuery 'ready' method ensures DOM is loaded before execution.
    console.log("Hello Pluralsight");

    var theForm = $("#theForm");
    // JQuery API provides 'hide' method, handling different browsers appropriate hiding.
    theForm.hide();

    var button = $("#buyButton");
    button.on("click", function () { // Anonymous function declared in-line to be executed on click.
        console.log("Buying Item");
    });
    // Anonymous function is an unnamed method much like a C# lamda. Useful for when a method will not be reused.

    // JQuery allows list to be returned directly with syntax similar to CSS selectors.
    var productInfo = $(".product-props li");   //document.getElementsByClassName("product-props");
                                                //var listItems = productInfo.item[0].children;
    productInfo.on("click", function () {
        console.log("You clicked on " + $(this).text()); // 'this' keyword accesses clicked object
    });

    var $loginToggle = $("#loginToggle");
    var $popupForm = $(".popup-form");

    $loginToggle.on("click", function () {
        $popupForm.fadeToggle(1000); // fades on click over amount of time
    });



})();