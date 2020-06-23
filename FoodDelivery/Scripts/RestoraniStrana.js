var kuhinjeZaServer = [];
var pretraga=null;
var opstina=null;
var postCode = -1;

var simpleSearch = 0;
var complexSearch = 0;
function setCheck(id) {
    
    let groupKuhinje = document.getElementById(-1);
    if (id < 0) {
       
        groupKuhinje.checked = true;
        kuhinjeZaServer = [];
        let elements = document.querySelectorAll(".groupKuhinje");
        for (let i = 0; i < elements.length; i++) {
            elements[i].checked = false;
           
            kuhinjeZaServer[i] = elements[i].id;
        }
        
        loadDoc();
      
    } else {
        groupKuhinje.checked = false;
        let el = document.getElementById(id);
        console.log(el.checked);
        if (el.checked) {
            kuhinjeZaServer[id] =parseInt(id);
            el.checked = true;
        } else {
            kuhinjeZaServer[id] = null;
            el.checked = false;
        }
        loadDoc();
    }
    
        
}
function getDataFromFields() {
    let municipality = document.querySelector("#municipalityConfirmId").value;
    
    if (municipality !== null && municipality !== 'undefined' ) {
        opstina = municipality;
    }
   // document.querySelector("#addressConfirmId").value;
       // document.querySelector("#houseNumber").value + " , " +
    let postCodes = document.querySelector("#postalCodeId").value;
    if (postCodes !== null && postCodes !== 'undefined' ) {
        postCode = parseInt(postCodes);
    }

    
    
}
function pretrazi(value) {
    pretraga = value;
    
}
function checkAndConfirm() {
    if (opstina != null && postCode > 0 && pretraga != null) {
        return 1;
    } else if (opstina != null && postCode > 0) {
        return 2;
    }
    return 0;
}
function getCurrentCity() {

}
function loadDoc() {
    getDataFromFields();
    let confirmation = checkAndConfirm();
    if (confirmation==1) {
        fullLoad();
    } else if (confirmation == 2) {
        partialLoad();
    }
    else {
        alert("Molimo vas da izaberete adresu za dostavu !");
    }
}
function fullLoad() {
    let skip =parseInt(document.getElementById("retreivedNumber").value);
    var holder = document.getElementById("PrikazRestoranaId");
    var jKuhinje = JSON.stringify(kuhinjeZaServer);
    var xhttp = new XMLHttpRequest();
    xhttp.onload = function () {
        if (xhttp.status == '200') {
            if (simpleSearch == 0) {
                complexSearch = 0;

                holder.innerHTML = xhttp.response;
                culculateLoadedItems(holder.childNodes.length);
                simpleSearch++;
            } else {
                let el = document.createElement('div');
                el.innerHTML = xhttp.response;
                holder.append(el);
                culculateLoadedItems(holder.childNodes.length);
                simpleSearch++;
            }
        }
    }

    xhttp.open("GET", "Search?opstina=" + opstina + "&postCode=" + postCode + "&restoran=" + pretraga + "&kuhinje=" + jKuhinje + "&skip=" + skip, true);
    xhttp.send();
}
function partialLoad() {
    let skip = parseInt(document.getElementById("retreivedNumber").value);
    var holder = document.getElementById("PrikazRestoranaId");
    var jKuhinje = JSON.stringify(kuhinjeZaServer);
    var xhttp = new XMLHttpRequest();
    xhttp.onload = function () {
        if (xhttp.status == '200') {
            if (complexSearch == 0) {
                simpleSearch = 0;

                holder.innerHTML = xhttp.response;
                culculateLoadedItems(holder.childNodes.length);
                complexSearch++;
            } else {
                let el = document.createElement('div');
                el.innerHTML = xhttp.response;
                holder.append(el);
                culculateLoadedItems(holder.childNodes.length);
                complexSearch++;
            }
        }
    }

    xhttp.open("GET", "SearchPartial?opstina=" + opstina + "&postCode=" + postCode + "&restoran=" + pretraga + "&kuhinje=" + jKuhinje + "&skip=" + skip, true);
    xhttp.send();
}
function culculateLoadedItems(newItems) {
    let loadedItems =parseInt(document.getElementById("retreivedNumber").value);
    loadedItems += newItems;
    document.getElementById("retreivedNumber").value = loadedItems;
}
function loadMoreItems() {
    loadDoc();
}
///Order 

