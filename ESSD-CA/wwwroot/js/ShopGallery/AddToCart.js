window.onload = function () {
    let addElemList = document.getElementsByClassName("addQuantity");

    for (let i = 0; i < addElemList.length; i++) {
        addElemList[i].addEventListener("click", onAddToCart);
    }
}

function onAddToCart(event) {
    let elem = event.currentTarget.parentNode;
    var elemValue = Number(elem.children[0].value);
    var productId = elem.children[1].getAttribute("productId");
    sendQty(productId, elemValue);
}

function sendQty(productId, qty) {

    let xhr = new XMLHttpRequest();

    xhr.open("POST", "/ShopGallery/AddToCart");
    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");

    xhr.onreadystatechange = function () {
        if (this.readyState === XMLHttpRequest.DONE) {
            if (this.status == 200) {
                let data = JSON.parse(this.responseText);
                console.log("Operation Status: " + data.success);
                parent.location.reload();
            }

        }
    }
    xhr.send(JSON.stringify(
        {
            ProductId: productId,
            Count: qty
        }
    ));
}