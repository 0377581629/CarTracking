(function ($) {
    app.modals.CreateOrEditRouteModal = function () {

        const _RouteService = abp.services.app.route;

        let _modalManager;
        let _$RouteInformationForm = null;

        let modal;

        let routeDetailTable;
        let addRouteDetailBtn;
        let detailRowCount = 0;
        let lstPointIds;
        let listTime;
        let routeDetail;
        
        this.init = function (modalManager) {
            _modalManager = modalManager;

            modal = _modalManager.getModal();

            _modalManager.initControl();
            
            listTime = modal.find('#Route_ListTime');
            routeDetail = modal.find('#Route_RouteDetail');

            let managementUnitSelector = modal.find('#ManagementUnitId');
            baseHelper.SimpleSelector(managementUnitSelector, app.localize('NoneSelect'), 'Lib/GetPagedManagementUnits', true);

            routeDetailTable = modal.find('#RouteDetailTable');
            addRouteDetailBtn = modal.find('#btnAddDetailRouteDetail');

            if (addRouteDetailBtn) {
                addRouteDetailBtn.on('click', function () {
                    detailRowCount++;
                    $.get(abp.appPath + 'Lib/Route/NewRouteDetail').then(function (res) {
                        routeDetailTable.find('#LastDetailRowRouteDetail').before(res);
                        baseHelper.RefreshUI(routeDetailTable);
                        InitRouteDetailSelector();
                        if (detailRowCount > 2) {
                            modal.find('.leaflet-routing-add-waypoint ').trigger("click");
                        }
                    });
                });
                // L.map('map').setView([21.02095, 105.80954], 12);
            }

            routeDetailTable.on('click', '.btnDeleteDetail', function () {
                let rowId = $(this).attr('rowId');
                routeDetailTable.find('.detailRow[rowId="' + rowId + '"]').remove();
                baseHelper.RefreshUI(routeDetailTable);
            });

            baseHelper.RefreshUI(routeDetailTable);

            InitRouteDetailSelector();

            _$RouteInformationForm = _modalManager.getModal().find('form[name=RouteInformationsForm]');
            _$RouteInformationForm.validate();

            //Begin Map

            //Display map
            const apiKey = "AAPK3373b61cf92b49eaa87fce9d3d4037a5dkWdLy4S9V518BcT_ebYPvh_SUzlx6AmkbdFKnaFrFnetTUR005gMS083ONDA0yU";
            let map = L.map('map').setView([21.02095, 105.80954], 12);
            L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
                attribution: '&copy; <a href="https://osm.org/copyright">OpenStreetMap</a> contributors'
            }).addTo(map);

            //End Map

            let control = L.Routing.control({
                routeWhileDragging: true,
                geocoder: L.Control.Geocoder.nominatim(),
                fitSelectedRoutes: true
            }).addTo(map);

            routeDetailTable.on('select2:select', '.endPointSelector', function (e) {
                FillEstimateDistanceAndTime();
                let data = e.params.data;
                if (detailRowCount > 1) {
                    control.spliceWaypoints(control.getWaypoints().length - 1, 1, [data.latitude, data.longitude]);
                } else {
                    control.spliceWaypoints(0, 1, [data.latitude, data.longitude]);
                    L.marker([data.latitude, data.longitude]).addTo(map).bindPopup('Bắt đầu').openPopup();
                }
            });

            //Draw route in map when open EditModal
            lstPointIds = modal.find('#Route_ListPoint').val().split(',');
            if (lstPointIds.length > 1) {
                _RouteService.getPointByIds(
                    lstPointIds
                ).done(function (lstPoint) {
                    $.each(lstPoint, function( index, point ) {
                        detailRowCount++;
                        if (detailRowCount === 1) {
                            control.spliceWaypoints(0, 1, [point.latitude, point.longitude]);
                            L.marker([point.latitude, point.longitude]).addTo(map).bindPopup('Bắt đầu').openPopup();
                        } else if (detailRowCount === 2) {
                            control.spliceWaypoints(control.getWaypoints().length - 1, 1, [point.latitude, point.longitude]);
                        } else {
                            modal.find('.leaflet-routing-add-waypoint ').trigger("click");
                            control.spliceWaypoints(control.getWaypoints().length - 1, 1, [point.latitude, point.longitude]);
                        }
                    });
                })
            }
            FillEstimateDistanceAndTime();
            
            function FillEstimateDistanceAndTime(){
                control.on('routesfound', function(e) {
                    let routes = e.routes;
                    let summary = routes[0].summary;
                    modal.find('#Route_EstimateDistance').val(summary.totalDistance / 1000);
                    modal.find('#Route_EstimatedTime').val(summary.totalTime % 3600 / 60);
                });
            }
            
            
            
        };

        function InitRouteDetailSelector() {
            if (routeDetailTable) {
                routeDetailTable.find('.detailRow').each(function () {
                    let rowId = $(this).attr('rowId');

                    let endPointSelector = routeDetailTable.find('.endPointSelector[rowId="' + rowId + '"][initSelector="false"]');
                    endPointSelector.select2({
                        placeholder: app.localize('PleaseSelect'),
                        allowClear: true,
                        width: '100%',
                        ajax: {
                            url: abp.appPath + "api/services/app/Lib/GetPagedPoints",
                            dataType: 'json',
                            delay: 50,
                            data: function (params) {
                                return {
                                    filter: params.term,
                                    skipCount: ((params.page || 1) - 1) * 10,
                                };
                            },
                            processResults: function (data, params) {
                                params.page = params.page || 1;
                                let res = $.map(data.result.items, function (item) {
                                    return {
                                        id: item.id,
                                        text: item.name,
                                        latitude: item.latitude,
                                        longitude: item.longitude,
                                    }
                                });

                                return {
                                    results: res,
                                    pagination: {
                                        more: (params.page * 10) < data.result.totalCount
                                    }
                                };
                            },
                            cache: true
                        },
                        language: abp.localization.currentLanguage.name
                    })
                    endPointSelector.removeAttr('initSelector');
                });
            }
        }

        function GetRouteDetails() {
            let details = [];
            if (routeDetailTable) {
                routeDetailTable.find('.detailRow').each(function () {
                    let rowId = $(this).attr('rowId');
                    details.push({
                        id: routeDetailTable.find('.detailId[rowId="' + rowId + '"]').val(),
                        routeId: routeDetailTable.find('.routeId[rowId="' + rowId + '"]').val(),
                        endPointId: routeDetailTable.find('.endPointSelector[rowId="' + rowId + '"]').val(),
                        time: routeDetailTable.find('.time[rowId="' + rowId + '"]').val(),
                        pointName: routeDetailTable.find('.pointName[rowId="' + rowId + '"]').val(),
                    })
                });
            }
            return details;
        }

        this.save = function () {
            if (!_$RouteInformationForm.valid()) {
                return;
            }

            const Route = _$RouteInformationForm.serializeFormToObject();

            Route.routeDetails = GetRouteDetails();

            let listPointIds = [];
            $.each(Route.routeDetails, function (index, routeDetail) {
                listPointIds.push(routeDetail.endPointId);
            });
            Route.listPoint = listPointIds.join();

            let listTime = [];
            $.each(Route.routeDetails, function (index, routeDetail) {
                listTime.push(routeDetail.time);
            });
            Route.listTime = listTime.join(' -> ');

            let listPointName = [];
            $.each(Route.routeDetails, function (index, routeDetail) {
                listPointName.push(routeDetail.pointName);
            });
            Route.routeDetail = listPointName.join(' -> ');

            _modalManager.setBusy(true);
            _RouteService.createOrEdit(
                Route
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditRouteModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);