(function ($) {
    app.modals.CreateOrEditPointModal = function () {

        const _PointService = abp.services.app.point;

        let _modalManager;
        let _$PointInformationForm = null;

        let modal;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            modal = _modalManager.getModal();

            _modalManager.initControl();

            let managementUnitSelector = modal.find('#ManagementUnitId');
            baseHelper.SimpleSelector(managementUnitSelector, app.localize('NoneSelect'), 'Lib/GetPagedManagementUnits', true);

            let pointTypeSelector = modal.find('#PointTypeId');
            baseHelper.SimpleSelector(pointTypeSelector, app.localize('NoneSelect'), 'Lib/GetPagedPointTypes', true);

            _$PointInformationForm = _modalManager.getModal().find('form[name=PointInformationsForm]');
            _$PointInformationForm.validate();
            
            //Begin Map
            let pointLatitude = modal.find('#Point_Latitude');
            let pointLongitude = modal.find('#Point_Longitude');

            //Display map
            const apiKey = "AAPK3373b61cf92b49eaa87fce9d3d4037a5dkWdLy4S9V518BcT_ebYPvh_SUzlx6AmkbdFKnaFrFnetTUR005gMS083ONDA0yU";
            let map = L.map('map').setView([21.02095, 105.80954], 12);
            L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
                attribution: '&copy; <a href="https://osm.org/copyright">OpenStreetMap</a> contributors'
            }).addTo(map);

            // Click to map
            let geocodeService = L.esri.Geocoding.geocodeService({
                apikey: apiKey 
            });
            
            let theMarker = {};
            
            if(pointLatitude.val() !== undefined && pointLongitude.val() !== undefined){
                // convert "," character of lat and lon get from html to "." to use leaflet 
                let latText = pointLatitude.val().toString();
                latText = latText.replace(",", ".");
                let lat = parseFloat(latText);
                
                let lonText = pointLongitude.val().toString();
                lonText = lonText.replace(",", ".");
                let lon = parseFloat(lonText);
                
                // theMarker = L.marker([pointLatitude.val(),pointLongitude.val()]).addTo(map).bindPopup(result.address.Match_addr).openPopup();
                theMarker = L.marker([lat,lon]).addTo(map);
            }

            map.on('click', function (e) {
                geocodeService.reverse().latlng(e.latlng).run(function (error, result) {
                    if (error) {
                        return;
                    }
                    
                    pointLatitude.val(result.latlng.lat);
                    pointLongitude.val(result.latlng.lng)

                    //Clear existing marker, 
                    if (theMarker !== undefined) {
                        map.removeLayer(theMarker);
                    }

                    //Add a marker to show where you clicked
                    theMarker = L.marker(result.latlng).addTo(map).bindPopup(result.address.Match_addr).openPopup();
                });
            });
            
            
            //Search address
            const searchControl = L.esri.Geocoding.geosearch({
                position: "topright",
                placeholder: "Tìm kiếm địa điểm",
                useMapBounds: false,
                providers: [
                    L.esri.Geocoding.arcgisOnlineProvider({
                        apikey: apiKey,
                        nearby: {
                            lat: 21.02095,
                            lng: 105.80954,
                        }
                    })
                ]
            }).addTo(map);

            const results = L.layerGroup().addTo(map);

            searchControl.on("results", (data) => {
                results.clearLayers();
                for (let i = data.results.length - 1; i >= 0; i--) {
                    const lngLatString = `${Math.round(data.results[i].latlng.lng * 100000) / 100000}, ${
                        Math.round(data.results[i].latlng.lat * 100000) / 100000
                    }`;
                    const marker = L.marker(data.results[i].latlng);
                    marker.bindPopup(`<b>${lngLatString}</b><p>${data.results[i].properties.LongLabel}</p>`);
                    results.addLayer(marker);
                    marker.openPopup();
                }
            });
            
            //End Map
        };

        this.save = function () {
            if (!_$PointInformationForm.valid()) {
                return;
            }

            const Point = _$PointInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _PointService.createOrEdit(
                Point
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditPointModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);