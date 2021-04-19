window.onload = function () {
    let inputs = document.getElementsByClassName("numberInCart");
    let deletes = document.getElementsByClassName("deleteBtn");
    let checkOutLink = document.getElementById("check-out");    //add event to check unavailable item
    checkOutLink.addEventListener("click", checkOut);
    for (var i = 0; i < inputs.length; i++) {
        inputs[i].addEventListener("change", changeIn);     //Add event to check numbers' change of input tags
        deletes[i].addEventListener("click", DeleteItem);
    }
}

function checkOut(event)    //check unavailable item when click the checkout link
{
    let checkOutTags = document.getElementsByClassName("checkOutTag");
    for (var i = 0; i < checkOutTags.length; i++)
    {
        if (checkOutTags[i].innerHTML === "Not Available!")
        {
            alert("Cannot check out the unavailable items! Please delete the Unavailable item!");
            window.event.returnValue = false;
            break;
        }
    }
}

function ComputeCost()
{
    let totals = document.getElementsByClassName("total");
    let divtotal = document.getElementById("totalP");
    var totalPrice = 0;
    for (var i = 0; i < totals.length; i++) {
        totalPrice += Number(totals[i].innerHTML);
    }
    divtotal.innerHTML = Number(totalPrice.toFixed(2));
}
function DeleteItem(event)      //delete the product when click the delete button / recalculate the total cost
{
    let elem = event.currentTarget;
    sendNumOption(0, elem.getAttribute("product_id"));    
    var tbody = document.getElementById("tbd");    
    tbody.removeChild(this.parentNode.parentNode);
    let totals = document.getElementsByClassName("total");
    let divtotal = document.getElementById("totalP");
    var totalPrice = 0;
    for (var i = 0; i < totals.length; i++) {
        if (totals[i].innerHTML != "Not Available!") {
            totalPrice += Number(totals[i].innerHTML);
        }
        
    }
    divtotal.innerHTML = Number(totalPrice.toFixed(2));
    
}
function changeIn(event)    //change the number of items / recalculate the total cost
{
    let elem = event.currentTarget;
    if (Number(elem.value) <= 0) {      // when number goes to 0, mean delete the item
        sendNumOption(Number(elem.value), elem.getAttribute("product_id"));
        var tbody = document.getElementById("tbd");
        tbody.removeChild(this.parentNode.parentNode);
        //compute the total cost
        let totals = document.getElementsByClassName("total");
        let divtotal = document.getElementById("totalP");
        var totalPrice = 0;
        for (var i = 0; i < totals.length; i++) {
            totalPrice += Number(totals[i].innerHTML);
        }
        divtotal.innerHTML = Number(totalPrice.toFixed(2));
    }
    else{                                       //if number not less and equal to 0, mean number of products change
        sendNumOption(Number(elem.value), elem.getAttribute("product_id"));
        //compute each price
        let totals = document.getElementsByClassName("total");
        let input = document.getElementsByClassName("numberInCart");
        let prices = document.getElementsByClassName("unitPrice");
        for (var i = 0; i < input.length; i++)
        {
            if (input[i] === elem )
            {
                if (totals[i].innerHTML === "Not Available!" )
                totals[i].innerHTML = Number((Number(prices[i].innerHTML) * Number(elem.value)).toFixed(2));
            }
        }
        //compute the total cost
        let divtotal = document.getElementById("totalP");
        var totalPrice = 0;
        for (var i = 0; i < totals.length; i++)
        {

            if (totals[i].innerHTML != "Not Available!") {
                totalPrice += Number(totals[i].innerHTML);
            }
        }
        divtotal.innerHTML = Number(totalPrice.toFixed(2));
    }
}

function sendNumOption(nums, productId)         //send the JSON to the server
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
                parent.location.reload();
/*                if (nums === 0) {           //when product numbers in any one input tag is going to delete, refresh the parent frame to change the icon number
                    parent.location.reload();
                } */            
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
