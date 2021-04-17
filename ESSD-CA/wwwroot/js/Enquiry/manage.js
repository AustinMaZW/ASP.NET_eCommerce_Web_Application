window.onload = function () {
    let updatebtns = document.getElementsByClassName("btn");

    for (let i = 0; i < updatebtns.length; i++) 
        updatebtns[i].addEventListener("click", onArchive);
    }


function onArchive(event) {
    let elem = event.currentTarget;
    var messageId = document.getElementById("message_id");

    sendArchive(true, messageId);
}

function sendArchive(archive, messageId)
{
    let xhr = new XMLHttpRequest();

    xhr.open("POST", "/Enquiry/Archive");
    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");
    xhr.onreadystatechange = function ()
    {
        if (this.readyState === XMLHttpRequest.DONE)
        {
            if (this.status == 200)
            {
                let data = JSON.parse(this.responseText);
            }
        }
    }

    xhr.send(JSON.stringify({
        Id: messageId,
        EnquiryStatus: archive
    }))
}

