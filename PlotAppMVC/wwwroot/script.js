//var map;
//function initialize(plots) {
//    map = L.map('map').setView([50.041187, 21.999121], 18);

//    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
//        maxZoom: 19,
//        attribution: '© OpenStreetMap'
//    }).addTo(map);

//    addPopUps(plots);
//}
//function addPopUps(plots) {
//    if (!plots) return;

//    for (let i = 0; i < plots.length; i++) {
//        var marker = L.marker([plots[i].longitude, plots[i].latitude]).addTo(map);
//        marker.bindPopup(`${plots[i].city} ${plots[i].plotNumber} ${plots[i].area}ha`);
//    }
//}

//async function addPopUp(key, city, plotNumber) {

//    const area = await fetch(`https://plot.search.api.ongeo.pl/1.0/autocomplete/?api_key=${key}
//&query=${city}%20${plotNumber}&highlightEnd=%3C/b%3E`)
//        .then((response) => response.json());

//    if (Object.keys(area).length === 0) return;

//    const lat = area[0].point.coordinates[0];
//    const lang = area[0].point.coordinates[1];

//    return [lat, lang];
//}

//function goToPopUp(longitude, latitude) {
//    map.setView([longitude, latitude], 18);
//}