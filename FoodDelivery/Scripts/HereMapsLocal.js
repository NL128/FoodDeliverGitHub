var returnObject = [];
var tmp;
var selectionId = null;
var map = null;
var renderId = -1;
var lastCooridnate = null;
// set up containers for the map  + panel
var mapContainer = document.getElementById('map'),
    suggestionsContainer = document.getElementById('output');

//Step 1: initialize communication with the platform

var APIKEY = 'kI9Red6f6Bmm5ummtOtMXgTIbML55lQDCFqpgX2ZgLo';
var AUTOCOMPLETION_URL = 'https://autocomplete.geocoder.ls.hereapi.com/6.2/suggest.json',
    ajaxRequest = new XMLHttpRequest(),
    query = '';

/**
 * If the text in the text box  has changed, and is not empty,
 * send a geocoding auto-completion request to the server.
 *
 * @@param {Object} textBox the textBox DOM object linked to this event
 * @@param {Object} event the DOM event which fired this listener
 */
var textBox = document.getElementById("searchBox");
var event = textBox.addEventListener("keypress", function () { autoCompleteListener(this.value); }, false);
function autoCompleteListener(val) {

    if (query != val) {
        if (val.length >= 3) {

            /**
             * A full list of available request parameters can be found in the Geocoder Autocompletion
             * API documentation.
             *
             */
            var params = '?' +
                'query=' + encodeURIComponent(val) +   // The search text which is the basis of the query
                '&beginHighlight=' + encodeURIComponent('<mark>') + //  Mark the beginning of the match in a token.
                '&endHighlight=' + encodeURIComponent('</mark>') + //  Mark the end of the match in a token.
                '&maxresults=5' +  // The upper limit the for number of suggestions to be included
                // in the response.  Default is set to 5.
                '&apikey=' + APIKEY;
            ajaxRequest.open('GET', AUTOCOMPLETION_URL + params);
            ajaxRequest.send();
        }
    }
    query = val;
}

function closeWindow() {
    document.getElementById("confirmationI").setAttribute("class", "popUp-food-default");
}

/**
 *  This is the event listener which processes the XMLHttpRequest response returned from the server.
 */

function onAutoCompleteSuccess() {
    /*
     * The styling of the suggestions response on the map is entirely under the developer's control.
     * A representitive styling can be found the full JS + HTML code of this example
     * in the functions below:
     */
    clearOldSuggestions();
    addSuggestionsToPanel(this.response);  // In this context, 'this' means the XMLHttpRequest itself.


}
function CallUpdateAfterSearch() {
    if (selectionId != null) {

        document.getElementById("confirmationI").setAttribute("class", "popUp-food");
        renderMap();
        addSuggestionsToMap(tmp, renderId);

        //inisialize fields
        fieldInitialization(selectionId, renderId);


    } else {
        alert("Molimo vas da popunite adresu!");
    }
}

function fieldInitialization(id, renderID) {

    var splitId = id.split(" ")[1];

    if (tmp != null && tmp != "undefined") {
        setFields(splitId, renderID);
    }


}
function setFields(splitId, rId) {
    if (rId == -1) {
        document.querySelector("#municipalityConfirmId").value = stripHtml(tmp.suggestions[splitId].address.district);
        document.querySelector("#addressConfirmId").value = stripHtml(tmp.suggestions[splitId].address.street);
        document.querySelector("#houseNumber").value = stripHtml(tmp.suggestions[splitId].address.houseNumber);
        document.querySelector("#postalCodeId").value = stripHtml(tmp.suggestions[splitId].address.postalCode);
    } else {
        document.querySelector("#municipalityConfirmId").value = stripHtml(tmp.address.district);
        document.querySelector("#addressConfirmId").value = stripHtml(tmp.address.street);
        document.querySelector("#houseNumber").value = stripHtml(tmp.address.houseNumber);
        document.querySelector("#postalCodeId").value = stripHtml(tmp.address.postalCode);
    }
}
function stripHtml(html) {
    // Create a new div element
    var temporalDivElement = document.createElement("div");
    // Set the HTML content with the providen
    temporalDivElement.innerHTML = html;
    // Retrieve the text property of the element (cross-browser support)
    return temporalDivElement.textContent || temporalDivElement.innerText || "";
}
function textConnect(id) {


    var res = tmp.suggestions[id].address.street + "," + tmp.suggestions[id].address.houseNumber + "," + tmp.suggestions[id].address.district + "," + tmp.suggestions[id].address.postalCode;
    textBox.value = stripHtml(res);
}

/**
 * This function will be called if a communication error occurs during the XMLHttpRequest
 */
function onAutoCompleteFailed() {
    alert('Ups adresa nije pronadjena molimo vas pokusajte ponovo !');
}

// Attach the event listeners to the XMLHttpRequest object
ajaxRequest.addEventListener("load", onAutoCompleteSuccess);
ajaxRequest.addEventListener("error", onAutoCompleteFailed);
ajaxRequest.responseType = "json";


/**
 * Boilerplate map initialization code starts below:
 */




var platform = new H.service.Platform({
    apikey: APIKEY,
    useCIT: false,
    useHTTPS: true
});
var defaultLayers = platform.createDefaultLayers();

var geocoder = platform.getGeocodingService();
var group = new H.map.Group();

group.addEventListener('tap', function (evt) {
    map.setCenter(evt.target.getGeometry());
    openBubble(
        evt.target.getGeometry(), evt.target.getData());
}, false);


function renderMap() {

    //Step 2: initialize a map - this map is centered over Belgrade
    if (map == null && map != "undefined") {
        map = new H.Map(mapContainer,
            defaultLayers.vector.normal.map, {
            center: { lat: 44.787197, lng: 20.457273 },
            zoom: 12

        });

        map.addObject(group);
        map.addEventListener("dragend", function () {

            map.getViewModel().setLookAtData({
                bounds: group.getBoundingBox()
            });
        }, false);
        map.addEventListener('mapviewchangeend', function () {
            var newZoom = map.getZoom();
            if (newZoom >= 18) {
                // zoomed in
                map.setZoom(18);
            } else if (newZoom <= 9) {
                // zoomed out
                map.setZoom(15);
            } else {
                map.setZoom(14);
            }

        })

        //Step 3: make the map interactive
        // MapEvents enables the event system
        // Behavior implements default interactions for pan/zoom (also on mobile touch environments)
        var behavior = new H.mapevents.Behavior(new H.mapevents.MapEvents(map));

        // Create the default UI components
        var ui = H.ui.UI.createDefault(map, defaultLayers);
    }


}

// Hold a reference to any infobubble opened
var bubble;

/**
 * Function to Open/Close an infobubble on the map.
 * @@param  {H.geo.Point} position     The location on the map.
 * @@param  {String} text              The contents of the infobubble.
 */
function openBubble(position, text) {
    if (!bubble) {
        bubble = new H.ui.InfoBubble(
            position,
            // The FO property holds the province name.
            { content: '<small>' + text + '</small>' });
        ui.addBubble(bubble);
    } else {
        bubble.setPosition(position);
        bubble.setContent('<small>' + text + '</small>');
        bubble.open();
    }
}


/**
 * The Geocoder Autocomplete API response retrieves a complete addresses and a `locationId`.
 * for each suggestion.
 *
 * You can subsequently use the Geocoder API to geocode the address based on the ID and
 * thus obtain the geographic coordinates of the address.
 *
 * For demonstration purposes only, this function makes a geocoding request
 * for every `locationId` found in the array of suggestions and displays it on the map.
 *
 * A more typical use-case would only make a single geocoding request - for example
 * when the user has selected a single suggestion from a list.
 *
 * @@param {Object} response
 */
function addSuggestionsToMap(response, id) {

    /**
     * This function will be called once the Geocoder REST API provides a response
     * @@param  {Object} result          A JSONP object representing the  location(s) found.
     */
    var onGeocodeSuccess = function (result) {
        var marker,
            locations = result.Response.View[0].Result,
            i;


        // Add a marker for each location found
        for (i = 0; i < locations.length; i++) {
            marker = new H.map.Marker({
                lat: locations[i].Location.DisplayPosition.Latitude,
                lng: locations[i].Location.DisplayPosition.Longitude
            });
            marker.setData(locations[i].Location.Address.Label);
            group.addObject(marker);
        }

        map.getViewModel().setLookAtData({
            bounds: group.getBoundingBox()
        });
        if (group.getObjects().length < 2) {
            map.setZoom(15);

        }
    },
        /**
         * This function will be called if a communication error occurs during the JSON-P request
         * @@param  {Object} error  The error message received.
         */
        onGeocodeError = function (error) {
            alert('Ooops!');
        },
        /**
         * This function uses the geocoder service to calculate and display information
         * about a location based on its unique `locationId`.
         *
         * A full list of available request parameters can be found in the Geocoder API documentation.
         * see: http://developer.here.com/rest-apis/documentation/geocoder/topics/resource-search.html
         *
         * @@param {string} locationId    The id assigned to a given location
         */
        geocodeByLocationId = function (locationId) {
            geocodingParameters = {
                locationId: locationId
            };

            geocoder.geocode(
                geocodingParameters,
                onGeocodeSuccess,
                onGeocodeError
            );
        }

    /*
     * Loop through all the geocoding suggestions and make a request to the geocoder service
     * to find out more information about them.
     */

    if (response != null && response != "undefined" && id == -1) {
        response.suggestions.forEach(function (item, index, array) {
            geocodeByLocationId(item.locationId);
        });
    } else if (id >= 0) {
        geocodeByLocationId(response.locationId);
    }

    else {
        alert("Ucitajte stranicu ponovo !");
    }
}


/**
 * Removes all H.map.Marker points from the map and adds closes the info bubble
 */
function clearOldSuggestions() {
    group.removeAll();
    if (bubble) {
        bubble.close();
    }
}

/**
 * Format the geocoding autocompletion repsonse object's data for display
 *
 * @@param {Object} response
 */
function clearSuggestionsInList() {
    suggestionsContainer.innerHTML = "";
}
function addSuggestionsToPanel(response) {

    clearSuggestionsInList();

    var r = response;
    tmp = r;


    for (let i = 0; i < r.suggestions.length; i++) {

        var li = document.createElement("li");
        li.setAttribute("id", "sugestion " + i);
        li.addEventListener("click", function () { getSelectionOnTab(this.id); }, false);
        li.addEventListener("touchend", function () { getSelectionOnTab(this.id); }, false);

        li.innerHTML = r.suggestions[i].address.street + "," + r.suggestions[i].address.city + "," + r.suggestions[i].address.houseNumber + "," + r.suggestions[i].address.postalCode + " , " + r.suggestions[i].address.district;
        suggestionsContainer.append(li);
    }
}
function getSelectionOnTab(id) {
    selectionId = id;
    var splitId = id.split(" ")[1];

    textConnect(splitId);
    clearSuggestionsInList();
    group.removeAll();
    group = new H.map.Group();
    tmp = tmp.suggestions[splitId];
    renderId = 0;
    //console.log(tmp.suggestions[splitId].address.district);






}
function getSearchInput() {
    document.getElementById("showSearchInputs").setAttribute("class", "changeInnerWidth90 displayInputs");
}
function confirmInputValues() {

    var fullname = document.querySelector("#setFullName");
    fullname.innerHTML = document.querySelector("#municipalityConfirmId").value + "," +
                    document.querySelector("#addressConfirmId").value + " , " +
                    document.querySelector("#houseNumber").value + " , " +
                    document.querySelector("#postalCodeId").value;
                    document.querySelector("#currentMunicipality").innerHTML = document.querySelector("#municipalityConfirmId").value;
                    document.querySelector("#confirmationI").setAttribute("class", "popUp-food-default");
}