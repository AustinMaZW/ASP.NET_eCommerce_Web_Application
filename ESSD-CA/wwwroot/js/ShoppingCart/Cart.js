window.onload = function () {
    let inputs = document.getElementsByClassName("numberInCart");
    let deletes = document.getElementsByClassName("deleteBtn");
    for (var i = 0; i < inputs.length; i++) {
        inputs[i].addEventListener("change", changeIn);
        deletes[i].addEventListener("click", Delete);
    }
}

function Delete(event) {
    let elem = event.currentTarget;
    sendNumOption(0, elem.getAttribute("product_id"));
    var tbody = document.getElementById("tbd");
    tbody.removeChild(this.parentNode.parentNode);
    let totals = document.getElementsByClassName("total");
    let divtotal = document.getElementById("totalP");
    var totalPrice = 0;
    for (var i = 0; i < totals.length; i++) {
        totalPrice += Number(totals[i].innerHTML);
    }
    divtotal.innerHTML = Number(totalPrice.toFixed(2));

}
function changeIn(event) {
    let elem = event.currentTarget;
    if (Number(elem.value) <= 0) {
        sendNumOption(Number(elem.value), elem.getAttribute("product_id"));
        var tbody = document.getElementById("tbd");
        tbody.removeChild(this.parentNode.parentNode);
        let totals = document.getElementsByClassName("total");
        let divtotal = document.getElementById("totalP");
        var totalPrice = 0;
        for (var i = 0; i < totals.length; i++) {
            totalPrice += Number(totals[i].innerHTML);
        }
        divtotal.innerHTML = Number(totalPrice.toFixed(2));
    }
    else
    {
        sendNumOption(Number(elem.value), elem.getAttribute("product_id"));
        let totals = document.getElementsByClassName("total");
        let input = document.getElementsByClassName("numberInCart");
        let prices = document.getElementsByClassName("unitPrice");
        for (var i = 0; i < input.length; i++)
        {
            if (input[i] === elem)
            {
                totals[i].innerHTML = Number((Number(prices[i].innerHTML) * Number(elem.value)).toFixed(2));
            }
        }
        let divtotal = document.getElementById("totalP");
        var totalPrice = 0;
        for (var i = 0; i < totals.length; i++)
        {
            totalPrice += Number(totals[i].innerHTML);
        }
        divtotal.innerHTML = Number(totalPrice.toFixed(2));
    }
}

function sendNumOption(nums, productId)
{
    let xhr = new XMLHttpRequest();
    xhr.open("POST", "/ShoppingCart/AdditemCart");
    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");
    xhr.onreadystatechange = function ()
    {
        if (this.readyState === XMLHttpRequest.DONE)
        {
            if (this.status == 200)
            {
                let data = JSON.parse(this.responseText);
                console.log("Operation Status: " + data.success);
            }

        }
    }
    xhr.send(JSON.stringify(
        {
            ProductId: productId,
            Count: nums
        }
    ));
}