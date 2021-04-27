// JScript File
function visualizzaImmobile(address){
    wheight=screen.height
    wwidth=screen.width  
    mywidth=530
    myheight=330
    mytop=(wheight-myheight)/2
    myleft=(wwidth-mywidth)/2
    window.open('MappaImmobile.aspx?addr='+address,'popup','width='+mywidth+'px,height='+myheight+'px,top='+mytop+'px,left='+myleft+'px');
}
function load() {
      if (GBrowserIsCompatible()) {
        var map = new GMap2(document.getElementById("map"));
        map.setCenter(new GLatLng(37.4419, -122.1419), 13);
        
       
      }
    }
    
        
    

    function showAddress(address) {
    var map = new GMap2(document.getElementById("map"));
    map.addControl(new GMapTypeControl()); 
    map.addControl(new GLargeMapControl()); 
    map.addControl(new GScaleControl());
    map.addControl(new GOverviewMapControl());

    
    var geocoder = new GClientGeocoder();
    geocoder.getLatLng(    address,    function(point) {      
    if (!point) {        
        alert(address + " non trovato");      
        } else { 
                map.setCenter(point, 17);
                var marker = new GMarker(point);
                map.addOverlay(marker);
                marker.openInfoWindowHtml(address);
                }    
               }  
             );
         }