$(function () {
    let _scriptUrl = abp.appPath + 'view-resources/Areas/Cms/Menu/';
    let _viewUrl = abp.appPath + 'Cms/Menu/';

    let _menuService = abp.services.app.menu;

    const _permissions = {
        create: abp.auth.hasPermission('Cms.Menu.Create'),
        edit: abp.auth.hasPermission('Cms.Menu.Edit'),
        'delete': abp.auth.hasPermission('Cms.Menu.Delete')
    };

    const _createOrEditModal = new app.ModalManager({
        viewUrl: _viewUrl + 'CreateOrEditModal',
        scriptUrl: _scriptUrl + '_CreateOrEditModal.js',
        modalClass: 'CreateOrEditMenuModal'
    });

    let _createMenuButton = $('#CreateNewButton');
    let _updateMenuStructureButton = $('#UpdateStructureButton');
    let _menuWrapper = $('#MenuWrapper');
    let _dd = _menuWrapper.find('.dd');
    let _ddList = _dd.find('#dd-list');
    let _menusFormFilter = $('#MenusFormFilter');
    let _menusFunctionsWrapper = $('#MenuFunctionsWrapper')
    
    let menuGroupSelector;
    menuGroupSelector = $('#MenuGroupSelector');
    baseHelper.SimpleRequiredSelector(menuGroupSelector, app.localize('PleaseSelect'), "/Cms/GetPagedMenuGroups");
    
    function getMenus() {
        abp.ui.setBusy(_menuWrapper);
        _ddList.empty();
        _menuService.getAllNested({
            menuGroupId: menuGroupSelector.val()
        }).done(function (data) {
            _ddList.html(baseHelper.NestedItemsHtml(data, _permissions.edit, _permissions.delete));
            _dd.nestable({
                beforeDragStop: function (l, e, p) {
                    // l is the main container
                    console.log(l);
                    // e is the element that was moved
                    console.log(e);
                    // p is the place where element was moved.
                    console.log(p);
                }
            });
            _dd.nestable('expandAll');
        }).always(function () {
            abp.ui.clearBusy(_menuWrapper);
        });
        
        $('.dd-empty').addClass('hidden');
        
        if (menuGroupSelector.val() !== undefined && menuGroupSelector.val() !== null)
            _menusFunctionsWrapper.show();
        else
            _menusFunctionsWrapper.hide();
    }

    _menuWrapper.on('click', '.customSettingClass', function () {
        let id = $(this).data('id');
        _createOrEditModal.open({
            id: id
        }, function (data) {
            _ddList.find('.customLabelClass[data-id="' + id + '"]').html(data.displayName);
        });
    });

    _menuWrapper.on('click', '.customDeleteClass', function () {
        let obj = {
            id: $(this).data('id')
        };
        baseHelper.Delete(obj, _menuService, getMenus);
    });

    if (_createMenuButton) {
        _createMenuButton.on('click', function () {
            if(menuGroupSelector.val() === null || menuGroupSelector.val() === undefined){
                abp.message.error(app.localize('PleaseSelectMenuGroup'), app.localize('Error'));
            }else{
                _createOrEditModal.open({
                    menuGroupId: menuGroupSelector.val()
                }, function (data) {
                    _ddList.append(baseHelper.NestedItemHtml(data, _permissions.edit, _permissions.delete));
                });
            }
        })
    }

    if (_updateMenuStructureButton) {
        _updateMenuStructureButton.on('click', function () {
            abp.ui.setBusy(_menuWrapper);
            _menuService.updateStructure({
                nestedItems: _dd.nestable('toArray')
            }).done(function () {
                abp.notify.success(app.localize('Successfully'));
            }).always(function () {
                abp.ui.clearBusy(_menuWrapper);
            });
        });
    }

    _menusFormFilter.on('click', '.filter-btn', function () {
        getMenus();
    });

    $('#RefreshButton').on('click', getMenus);
    
    getMenus();
});
