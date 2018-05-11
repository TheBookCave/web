
//Todo Ajax and finsih code
$(".cart-item").each(function () {
  let id = $(this).attr("id");

  $("#itemIncrement-" + id).on("click", function () {
    let quantity = $("#itemQuantity-" + id).val();
    let dataToSend = { orderItemId: id, increment: true }
    $.post("AjaxChangeOrderItemQuantity", dataToSend, function (data, status) {
      $("#itemQuantity-" + id).val(++quantity);
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
    }).fail(function () {
      console.log("AjaxRemoveOrderItem failed");
    });
  });
});