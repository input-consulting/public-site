$(document).ready(function() {
  var latlong = [59.3343553, 18.0583659];
  var map = L.map('map').setView(latlong, 16);

  L.tileLayer(
    'https://api.tiles.mapbox.com/v4/{id}/{z}/{x}/{y}.png?access_token=pk.eyJ1IjoiaW5wdXQtY29uc3VsdGluZyIsImEiOiJjangzOTQ3eXEwMG02M3l0NnY4aXJnZWJiIn0.KRyG53ib9tf97lLuIP1nlA',
    {
      maxZoom: 20,
      attribution:
        'Map data &copy; <a href="https://www.openstreetmap.org/" target="_blank">OpenStreetMap</a> contributors, ' +
        'Imagery © <a href="https://www.mapbox.com/" target="_blank">Mapbox</a>',
      id: 'mapbox.streets'
    }
  ).addTo(map);

  L.marker(latlong)
    .addTo(map)
    .bindPopup(
      '<b>Input Consulting Stockholm AB</b><br />Kungsgatan 62<br/>111 22 Stockholm<br/>+46 (8) 700 00 80'
    )
    .openPopup();
});
