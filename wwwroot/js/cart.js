
//Do the price changing code
$(".cart-item").each(function () {
  let id = $(this).attr("id");

  updateItemTotalPrice(id, null);
  updateFinalPrice();

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
      updateItemTotalPrice(id, true);
      updateFinalPrice();
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
        updateItemTotalPrice(id, false);
        updateFinalPrice();
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
      updateFinalPrice();
    }).fail(function () {
      console.log("AjaxRemoveOrderItem failed");
    });
  });
});

function updateFinalPrice() {
  let finalPrice = 0;
  $(".cart-item").each(function () {
    let id = $(this).attr("id");
    finalPrice += 1 * $("#itemTotalPrice-" + id).html().substring(1);
  });
  $("#cartFinalPrice").val("Total: $" + finalPrice);
}

function updateItemTotalPrice(id, increment) {
  let quantity = $("#itemQuantity-" + id).val() * 1;
  let singleItemPrice;
  if (increment === true) {
    singleItemPrice = $("#itemTotalPrice-" + id).html().substring(1) / (quantity - 1);
  } 
  else if (increment === false) {
    singleItemPrice = $("#itemTotalPrice-" + id).html().substring(1) / (quantity + 1);
  }
  else {
    singleItemPrice = $("#itemTotalPrice-" + id).html().substring(1) / (quantity);
  }
  $("#itemTotalPrice-" + id).html("$" + quantity * singleItemPrice);
}