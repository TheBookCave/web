
///Assign functionality to all buttons in the cart
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
    //Tell the server with ajax to increment the quantity of the given item based on id
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
      //Tell the server with ajax to decrement the quantity of the given item based on id
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
    //Tell the server with ajax to remove the given item based on id
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
    finalPrice += parseFloat($("#itemTotalPrice-" + id).html().substring(1));
  });
  $("#cartFinalPrice").val("Total: $" + finalPrice);
}

function updateItemTotalPrice(id, increment) {
  let quantity = parseFloat($("#itemQuantity-" + id).val());
  let singleItemPrice = parseFloat($("#itemTotalPrice-" + id).html().substring(1));
  if (increment === true) {
    singleItemPrice = singleItemPrice / (quantity - 1);
  }
  else if (increment === false) {
    singleItemPrice = singleItemPrice / (quantity + 1);
  }
  else {
    singleItemPrice = singleItemPrice / (quantity);
  }
  $("#itemTotalPrice-" + id).html("$" + quantity * singleItemPrice);
}