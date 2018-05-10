
//Todo Ajax and finsih code
$(".cart-item").each(function() {
  let id = $(this).attr("id");

  $("#itemIncrement-" + id).on("click", function() {
    let quantity = $("#itemQuantity-" + id).val();
    $("#itemQuantity-" + id).val(++quantity);
  });

  $("#itemDecrement-" + id).on("click", function() {
    let quantity = $("#itemQuantity-" + id).val();
    $("#itemQuantity-" + id).val(--quantity);
  });
  
  $("#itemRemove-" + id).on("click", function() {
    $("#" + id).remove();
  });
});