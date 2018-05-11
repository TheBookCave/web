
//Do the price changing code
$(".cart-item").each(function () {
  let id = $(this).attr("id");

  if ($("#itemQuantity-" + id).val() == 1) {
    $("#itemDecrement-" + id).css('visibility', 'hidden');
  }

  $("#itemIncrement-" + id).on("click", function () {
    let quantity = $("#itemQuantity-" + id).val();
    let dataToSend = { orderItemId: id, increment: true }
    $.post("AjaxChangeOrderItemQuantity", dataToSend, function (data, status) {
      $("#itemQuantity-" + id).val(++quantity);
      if (quantity === 2) {
        $("#itemDecrement-" + id).css('visibility', 'visible');;
      }
    }).fail(function () {
      console.log("AjaxChangeOrderItemQuantity failed");
    });
  });

  $("#itemDecrement-" + id).on("click", function () {
    let quantity = $("#itemQuantity-" + id).val();
    if (quantity > 1) {
      let dataToSend = { orderItemId: id, increment: false }
      $.post("AjaxChangeOrderItemQuantity", dataToSend, function (data, status) {
        $("#itemQuantity-" + id).val(--quantity);
        if (quantity === 1) {
          $("#itemDecrement-" + id).css('visibility', 'hidden');
        }
      }).fail(function () {
        console.log("AjaxChangeOrderItemQuantity failed");
      });
    }
    else {
      console.log("Quantity needs to be more than 1");
    }
  });

  $("#itemRemove-" + id).on("click", function () {
    let dataToSend = { orderItemId: id }
    $.post("AjaxRemoveOrderItem", dataToSend, function (data, status) {
      $("#" + id).remove();
      if ($(".cart-item").length === 0) {
        $("#cart").html("<p>Your cart is currently empty.</p>");
      }
    }).fail(function () {
      console.log("AjaxRemoveOrderItem failed");
    });
  });
});