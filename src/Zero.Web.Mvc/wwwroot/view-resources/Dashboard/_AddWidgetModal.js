(function ($) {
    app.modals.AddWidgetModal = function () {

        var _dashboardService = abp.services.app.dashboard;
        var _modalManager;

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };

        this.save = function () {
            var widgetId = $('#WidgetSelect').val();
            
            _modalManager.setBusy(true);
            _dashboardService.addWidget({
                widgetId: widgetId,
                pageId: $('#PageId').val(),
                height: $('#' + widgetId + 'Height').val(),
                width: $('#' + widgetId + 'Width').val()
            }).done(function () {
                abp.notify.info(app.localize('AddedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.addWidgetModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);