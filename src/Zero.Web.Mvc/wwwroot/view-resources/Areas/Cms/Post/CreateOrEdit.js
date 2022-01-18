(function ($) {

    new FroalaEditor('#Description', frEditorConfigSimple);
    let postForm = $('#PostForm');
    let saveButton = $('#SaveButton');
    let _imageHolder = $('#AvatarHolder');
    let CancelAvatar = $('#CancelAvatar');
    let ChangeAvatar = $('#ChangeAvatar');
    let _imageValue = $('#Avatar');
    let TagSelector = $('#TagsSelector');
    let backToListingPage = $('#backToListingPage');

    let featuresTree = new FeaturesTree();
    console.log('$(\'.category-tree\')',$('.category-tree'))
    featuresTree.init($('.category-tree'));
    
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


    let _$PostForm = null;
    _$PostForm = $('.form-validation-post');
    _$PostForm.validate();

    const _postService = abp.services.app.post;

    saveButton.on('click', function () {
        console.log('vao');

        var x = featuresTree.getFeatureValues();
        console.log('x',x);
        return;
        if (!_$PostForm.valid()) {
            return;
        }
        var post = postForm.serializeFormToObject();
        
        let data = $("#TagsSelector").val();
        let listTags = [];
        if(data.length >0){
            data.forEach( (e)=>{
                listTags.push({TagId: e}) ;
            })
            data.listTags = listTags;
        }
        post.listTags = listTags;
        
        _postService.createOrEdit(
            post
        ).done(function () {
            abp.notify.info(app.localize('SavedSuccessfully'));
            window.location = '/Cms/Post/Index';
        }).always(function () {
        });
    });



    function GetTagsDetail() {
        let details = [];
        if (TagSelector) {
            TagSelector.find('.criteriaInAdmissionPlanDetailRow').each(function () {
                let rowId = $(this).attr('rowId');
                let detail = {
                    id: criteriaDetailTable.find('.detailId[rowId="' + rowId + '"]').val(),
                    admissionPlanId: criteriaDetailTable.find('.admissionPlanId[rowId="' + rowId + '"]').val(),
                    criteriaId: criteriaDetailTable.find('.criteriaId[rowId="' + rowId + '"]').val(),
                    isRequired: criteriaDetailTable.find('.detailIsRequired[rowId="' + rowId + '"]').prop('checked')
                };
                details.push(detail);
            });
        }
        return details;
    }

    let CategorySelector = $('#CategorySelector');

    CategorySelector.select2({
        width: '100%',
        ajax: {
            url: abp.appPath + "api/services/app/Category/GetAllNested",
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
                console.log(data)
                let res = $.map(data.result, function (item) {
                    return {
                        text: item.displayName,
                        id: item.id
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

    
    backToListingPage.on('click', function () {
        window.location = '/Cms/Post';
    })

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
                console.log('data',data);

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
    
})(jQuery);


