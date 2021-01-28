export function displayMap(latitude, longitude) {
    let lat = parseFloat(latitude);
    let long = parseFloat(longitude);
    let map = L.map('map').setView([lat, long], 13);
    L.tileLayer('https://{s}.tile.openstreetmap.fr/osmfr/{z}/{x}/{y}.png', {
        attribution: 'données © <a href="//osm.org/copyright">OpenStreetMap</a>/ODbL - rendu <a href="//openstreetmap.fr">OSM France</a>',
        minZoom: 1,
        maxZoom: 20
    }).addTo(map);
    L.marker([lat, long]).addTo(map);
}