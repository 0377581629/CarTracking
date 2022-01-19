(function () {
    $(function () {
    const _postService = abp.services.app.post;
    let postForm = $('#PostForm');
    let saveButton = $('#SaveButton');
    let _imageHolder = $('#AvatarHolder');
    let CancelAvatar = $('#CancelAvatar');
    let ChangeAvatar = $('#ChangeAvatar');
    let _imageValue = $('#Avatar');
    let TagSelector = $('#TagsSelector');
    let backToListingPage = $('#backToListingPage');

    let _$PostForm = null;
    _$PostForm = $('.form-validation-post');
    _$PostForm.validate();

    new FroalaEditor('#Description', frEditorConfig);

    let categoriesTree = new CategoriesTree();
    categoriesTree.init($('.category-tree'));

    let mainCategoryId = $('#CategoryId');
    baseHelper.SimpleRequiredSelector(mainCategoryId, app.localize('PleaseSelect'), "/Cms/GetPagedCategories");

    TagSelector.select2({
        width: '100%',
        placeholder: app.localize('PleaseSelect'),
        multiple: true,
        ajax: {
            url: abp.appPath + "api/services/app/Tags/GetAll",
            dataType: 'json',
            quietMillis: 100,
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
                        text: item.tags.name,
                        id: item.tags.id
                    }
                });

                if (data.result.totalCount === 0) {
                    res.splice(0, 0, {
                        text: app.localize('NotFound')
                    });
                }

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
    });

    ChangeAvatar.on('click', function () {
        _fileManagerModal.open({
            allowExtension: "*.jpg;*.png;*.jpeg",
            maxSelectCount: 1
        }, function (selected) {
            if (selected !== undefined && selected.length >= 1) {
                if (_imageValue) _imageValue.val(selected[selected.length - 1].path);
                if (_imageHolder) _imageHolder.attr('src', selected[selected.length - 1].path);
            }
        });
    });

    CancelAvatar.on('click', function () {
        _imageHolder.attr('src', '');
        _imageValue.val('')
    });

    backToListingPage.on('click', function () {
        window.location = '/Cms/Post';
    });
    
    function convertKeyForArrayInput(arr, keyNew) {
        let arrayOutput = [];
        if (arr.length > 0) {
            arr.forEach(function (value) {
                let obj = {};
                obj[keyNew] = value;
                arrayOutput.push(obj);
            })
        }
        return arrayOutput;
    }

    saveButton.on('click', function () {
        let selectorsValid = baseHelper.ValidSelectors();
        
        if (!_$PostForm.valid() || !selectorsValid) {
            return;
        }
        
        let post = postForm.serializeFormToObject();
        post.ListCategories = convertKeyForArrayInput(categoriesTree.getSelectedCategoriess(), 'categoryId');
        post.listTags = convertKeyForArrayInput($("#TagsSelector").val(), 'tagId');
        _postService.createOrEdit(
            post
        ).done(function () {
            abp.notify.info(app.localize('SavedSuccessfully'));
            window.location = '/Cms/Post/Index';
        }).always(function () {
        });
    });
    });
})();

