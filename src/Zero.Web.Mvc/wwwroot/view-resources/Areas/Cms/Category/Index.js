$(function () {
    let _scriptUrl = abp.appPath + 'view-resources/Areas/Cms/Category/';
    let _viewUrl = abp.appPath + 'Cms/Category/';

    let _categoryService = abp.services.app.category;

    const _permissions = {
        create: abp.auth.hasPermission('Cms.Category.Create'),
        edit: abp.auth.hasPermission('Cms.Category.Edit'),
        'delete': abp.auth.hasPermission('Cms.Category.Delete')
    };

    const _createOrEditModal = new app.ModalManager({
        viewUrl: _viewUrl + 'CreateOrEditModal',
        scriptUrl: _scriptUrl + '_CreateOrEditModal.js',
        modalClass: 'CreateOrEditCategoryModal'
    });

    let _createCategoryButton = $('#CreateNewButton');
    let _updateCategoryStructureButton = $('#UpdateStructureButton');
    let _categoryWrapper = $('#CategoryWrapper');
    let _dd = _categoryWrapper.find('.dd');
    let _ddList = _dd.find('#dd-list');

    function getCategories() {
        abp.ui.setBusy(_categoryWrapper);
        _ddList.empty();
        _categoryService.getAllNested().done(function (data) {
            _ddList.html(baseHelper.NestedItemsHtml(data, _permissions.edit, _permissions.delete));
            _dd.nestable({
                beforeDragStop: function(l,e, p){
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
            abp.ui.clearBusy(_categoryWrapper);
        });
    }

    _categoryWrapper.on('click', '.customSettingClass', function () {
        let id = $(this).data('id');
        _createOrEditModal.open({
            id: id
        }, function (data) {
            _ddList.find('.customLabelClass[data-id="' + id + '"]').html(data.displayName);
        });
    });

    _categoryWrapper.on('click', '.customDeleteClass', function () {
        let obj = {
            id: $(this).data('id')
        };
        baseHelper.Delete(obj, _categoryService, getCategories);
    });

    if (_createCategoryButton) {
        _createCategoryButton.on('click', function () {
            _createOrEditModal.open({}, function (data) {
                _ddList.append(baseHelper.NestedItemHtml(data, _permissions.edit, _permissions.delete));
            });
        })
    }

    if (_updateCategoryStructureButton) {
        _updateCategoryStructureButton.on('click', function () {
            abp.ui.setBusy(_categoryWrapper);
            _categoryService.updateStructure({
                nestedItems: _dd.nestable('toArray')
            }).done(function () {
                abp.notify.success(app.localize('UpdateStructureSuccessfully'));
            }).always(function () {
                abp.ui.clearBusy(_categoryWrapper);
            });
        });
    }

    getCategories();
});
