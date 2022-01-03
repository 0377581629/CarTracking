(function ($) {
    app.modals.CreateOrEditWidgetModal = function () {

        const _WidgetsService = abp.services.app.widget;
        const _CmsService = abp.services.app.cms;

        let _modalManager;
        let _$WidgetInformationForm = null;

        let modal;
        let asyncLoad;
        let contentType;
        let contentCount;

        let jsPlainText;
        let cssPlainText;

        let _$PageThemesTable;
        let _$PageThemesTableFilter;
        let _$refreshButton;
        let pageThemeTable;

        let selectedPageThemesIds = [];

        this.init = function (modalManager) {
            _modalManager = modalManager;
            modal = _modalManager.getModal();
            _modalManager.initControl();

            _$PageThemesTable = modal.find('#PageThemesTable');
            _$PageThemesTableFilter = modal.find('#PageThemesTableFilter');
            _$refreshButton = modal.find('#PageThemesTableFilterBtn');

            pageThemeTable = _$PageThemesTable.DataTable({
                paging: true,
                serverSide: true,
                processing: true,
                deferLoad: 0,
                listAction: {
                    ajaxFunction: _CmsService.getPagedPageThemes,
                    inputFilter: function () {
                        return {
                            filter: _$PageThemesTableFilter.val(),
                        };
                    }
                },
                columnDefs: [
                    {
                        targets: 0,
                        width: 40,
                        class: "text-center p-0",
                        data: "id",
                        name: "id",
                        orderable: false,
                        render: function (id) {
                            let checked = false;
                            if (selectedPageThemesIds.length > 0) {
                                selectedPageThemesIds.forEach(function (value, index) {
                                    if (parseInt(value) === parseInt(id))
                                        checked = true;
                                });
                            }
                            return baseHelper.ShowCheckBox(id, 'pageThemeChecker', checked);
                        }
                    },
                    {
                        targets: 1,
                        data: "name",
                        name: "name"
                    }
                ]
            });

            if (_$refreshButton) {
                _$refreshButton.on('click', function () {
                    pageThemeTable.ajax.reload();
                })
            }

            let selectedPageThemes = modal.find('#SelectedPageThemesIds');
            if (selectedPageThemes.val() !== undefined && selectedPageThemes.val().length > 0)
                selectedPageThemesIds = selectedPageThemes.val().split(',');

            _$PageThemesTable.on('change', '.pageThemeChecker', function () {
                let customId = $(this).attr('customId');
                if ($(this).prop('checked'))
                    selectedPageThemesIds.push(customId);
                else
                    selectedPageThemesIds = jQuery.grep(selectedPageThemesIds, function (value) {
                        return value !== customId;
                    });
            });

            contentType = modal.find('#ContentType');
            contentCount = modal.find('#Widget_ContentCount');
            contentType.on('change', function () {
                if (contentType.val() === '1')
                    contentCount.attr('readonly', true);
                else
                    contentCount.attr('readonly', false);
            });
            asyncLoad = modal.find('#Widget_AsyncLoad');
            asyncLoad.on('change', function () {
                if ($(this).prop('checked') === true) {
                    modal.find('.asyncLoadGroup').removeClass('hidden');
                } else {
                    modal.find('.asyncLoadGroup').addClass('hidden');
                }
            });
            
            jsPlainText = CodeMirror.fromTextArea(document.getElementById("Widget_JsPlain"), {
                mode: "javascript",
                styleActiveLine: true,
                lineNumbers: true,
                matchBrackets: true,
                scrollbarStyle: 'simple',
                autoRefresh: true
            });

            cssPlainText = CodeMirror.fromTextArea(document.getElementById("Widget_CssPlain"), {
                mode: "css",
                styleActiveLine: true,
                lineNumbers: true,
                matchBrackets: true,
                scrollbarStyle: 'simple',
                autoRefresh: true
            });

            _$WidgetInformationForm = _modalManager.getModal().find('form[name=WidgetInformationsForm]');
            _$WidgetInformationForm.validate();
        };

        this.save = function () {
            if (!_$WidgetInformationForm.valid()) {
                return;
            }

            const Widget = _$WidgetInformationForm.serializeFormToObject();
            Widget.pageThemesIds = selectedPageThemesIds;
            Widget.jsPlain = jsPlainText.getValue();
            Widget.cssPlain = cssPlainText.getValue();
            
            _modalManager.setBusy(true);
            _WidgetsService.createOrEdit(
                Widget
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditWidgetModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);