/// Shows the user what the current value of the price range in book list filter
$("#currentPriceRangeValue").text($("#priceRange").val());

/// Shows the user the current value of the price range in filters
$("#priceRange").change(function() {
  $("#currentPriceRangeValue").text($("#priceRange").val());
});

/// Enables tooltips for the page
$(function () {
  $('[data-toggle="tooltip"]').tooltip()
})

/// Fills the genre filter with all the genres
$.get("Book/GetAllGenres", function (data, status) {
  let genres = $("#genre");
  for (let i = 0; i < data.length; i++) {
    let option= $("<option>").append(data[i].name);
    option.val(data[i].id);
    genres.append(option);
  }
}).fail(function (errorObject) {
  console.log("GetFilterInfo failed");
});