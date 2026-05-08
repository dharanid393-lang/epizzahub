

function AddToCart(ItemId, UnitPrice, Quantity) {
    $.ajax({
        type: "GET",
        url: "/Cart/AddToCart/" + ItemId + "/" + UnitPrice + "/" + Quantity,
        success: function (response) {
            $("#cartCounter").text(response.count)
        },
        error: function (event) {
            alert("Error in adding item. Please try again")
        }
    })
}

// delete cart item from the cart page and update the cart table
function deleteItem(id) {
    if (id > 0) {
        if (confirm("Do you want to delete this item from cart ?")) {

            $.ajax({
                type: "DELETE",
                url: "/Cart/DeleteItem/" + id,
                success: function (response) {
                    if (response.count > 0) {
                        alert("Item Deleted");
                        location.reload();
                    }
                },
                error: function (event) {
                    alert("Error in adding item. Please try again")
                }
            })
        }

    }
}

//this update quantity method will be called on the cart item page where user can update the quantity of items in the cart
function updateQuantity(id, currentQuantity, expectedQuantity) {
    $.ajax({
        type: "PUT",
        url: "/Cart/UpdateQuantity/" + id + "/" + expectedQuantity,
        success: function (response) {
            if (response.count > 0) {
                alert("Quanity Updated");
                location.reload();
            }
        },
        error: function (event) {
            alert("Error in adding item. Please try again")
        }
    })
}


$(document).ready(function () {
    $.ajax({
        type: "GET",
        url: "/Cart/GetCartCount/",
        success: function (response) {
            $("#cartCounter").text(response.count)
        },
        error: function (event) {
            alert("Error in adding item. Please try again")
        }
    })
})
