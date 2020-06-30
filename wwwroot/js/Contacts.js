
function FillProperties(Donor, Acceptor, Except = []) {
    for (prop in Donor) {
        if (!Except.find(elem => elem == prop)) {
            Acceptor[prop] = Donor[prop].toString() == "[object Object]"? "" : Donor[prop];
        }
    }
}

function ClearVueData(VueObject)
{
    for (Field in VueObject._data)
    {
        VueObject[Field] = "";
    }
}

function Save(VueObject)
{
    let SentData = JSON.stringify(VueObject._data);
    let Request = new XMLHttpRequest();
    let Address = "/api/contacts"; 
    Request.open("POST", Address, true);
    Request.setRequestHeader('Content-Type', 'application/json; charset=UTF-8')
    Request.send(SentData);
    Request.onreadystatechange = function () {
        if (this.readyState == 4) {
            window.location.reload();
        }
    }

}

function CreateNew(VueObject)
{
    ClearVueData(VueObject);
    document.getElementById("ContactInfo").hidden = false;
    document.getElementById("list").hidden = true;
}

function Return()
{
    //window.location.reload();
    document.getElementById("list").hidden = false;
    document.getElementById("ContactInfo").hidden = true;
}

function Delete(id)
{
    let request = new XMLHttpRequest();
    request.open("DELETE", "api/Contacts/" + String(id), true);
    request.send();
    request.onreadystatechange = function () {
        if (this.readyState == 4) {
            //let responseObject = JSON.parse(this.responseText);
            ////console.log(JSON.stringify(Vue));
            //FillProperties(responseObject, VueObject, []);
            //document.getElementById("ContactInfo").hidden = false;
            //document.getElementById("list").hidden = true;
            ////console.log(JSON.stringify(Vue));
            window.location.reload();
        }
    }
}

function Edit(id, VueObject) {
    let request = new XMLHttpRequest();
    request.open("GET", "api/Contacts/" + String(id), true);
    request.send();
    request.onreadystatechange = function () {
        if (this.readyState == 4) {
            let responseObject = JSON.parse(this.responseText);
            //console.log(JSON.stringify(Vue));
            FillProperties(responseObject, VueObject, []);
            document.getElementById("ContactInfo").hidden = false;
            document.getElementById("list").hidden = true;
            //console.log(JSON.stringify(Vue));
        }
    }
}
