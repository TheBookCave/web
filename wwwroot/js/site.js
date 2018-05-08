// Write your JavaScript code.

/// Sets the current page in the navbar as active
$('a[href="' + this.location.pathname + '"]').parents('li,ul').addClass('active');



/// Shows the user what the current value of the price range in book list filter
// This should probably be moved to antoher js file
$("#currentPriceRangeValue").text($("#priceRange").val());

$("#priceRange").change(function() {
  $("#currentPriceRangeValue").text($("#priceRange").val());
});


$(function () {
  $('[data-toggle="tooltip"]').tooltip()
})