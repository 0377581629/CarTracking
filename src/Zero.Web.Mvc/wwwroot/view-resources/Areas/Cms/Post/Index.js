(function () {
    $(function () {

        let _$PostTable = $('#PostTable');
        let _$PostTableFilter = $('#PostTableFilter');
        let _$PostFormFilter = $('#PostFormFilter');
        let _$refreshButton = _$PostFormFilter.find("button[name='RefreshButton']");
        let _createSingleButton = $('#CreateNewButton');
        let _PostService = abp.services.app.post;

        let _scriptUrl = abp.appPath + 'view-resources/Areas/Cms/Post/';
        let _viewUrl = abp.appPath + 'Cms/Post/';

        const _permissions = {
            create: abp.auth.hasPermission('Cms.Post.Create'),
            edit: abp.auth.hasPermission('Cms.Post.Edit'),
            'delete': abp.auth.hasPermission('Cms.Post.Delete')
        };

        const _createOrEditModal = new app.ModalManager({
            viewUrl: _viewUrl + 'CreateOrEditModal',
            scriptUrl: _scriptUrl + '_CreateOrEditModal.js',
            modalClass: 'CreateOrEditPostModal'
        });

        let categoryId = $('#CategoryIdSelector');
        baseHelper.SimpleSelector(categoryId, app.localize('NoneSelect'), "/Cms/GetPagedCategories")

        let getFilter = function () {
            return {
                filter: _$PostTableFilter.val(),
                categoryId: categoryId.val()
            };
        };

        let dataTable = _$PostTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            deferLoad: 0,
            listAction: {
                ajaxFunction: _PostService.getAll,
                inputFilter: getFilter
            },
            columnDefs: [
                {
                    targets: 0,
                    className: 'control responsive',
                    orderable: false,
                    render: function () {
                        return '';
                    }
                },
                {
                    width: 80,
                    targets: 1,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    defaultContent: '',
                    rowAction: {
                        dropDownStyle: false,
                        cssClass: 'text-center',
                        items: [
                            {
                                icon: 'la la-edit text-primary',
                                text: app.localize('Edit'),
                                visible: function (data) {
                                    console.log(data)
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    window.location = "/Cms/Post/CreateOrEdit?id=" + data.record.post.id;
                                }
                            },
                            {
                                icon: 'la la-trash text-danger',
                                text: app.localize('Delete'),
                                visible: function (data) {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    baseHelper.Delete(data.record.post, _PostService, getPost);
                                }
                            }]
                    }
                },
                {
                    targets: 2,
                    data: "post.name",
                    name: "name"
                },
                {
                    targets: 3,
                    width: 250,
                    data: "post.categoryName",
                    name: "categoryName"
                },
                {
                    targets: 4,
                    data: "post.name",
                    name: "name",
                },
                {
                    width: 60,
                    targets: 5,
                    class: "text-right",
                    data: "post.commentCount",
                    name: "commentCount",
                },
                {
                    width: 60,
                    targets: 6,
                    class: "text-right",
                    data: "post.viewCount",
                    name: "viewCount",
                }
            ]
        });

        function getPost() {
            dataTable.ajax.reload();
        }

        if(categoryId) {
            categoryId.on('change', function () {
                getPost();
            })
        }

        if (_$refreshButton) {
            _$refreshButton.on('click', getPost);
        }

        if (_createSingleButton){
            _createSingleButton.click(function () {
                window.location = '/Cms/Post/CreateOrEdit';
            });
        }

        abp.event.on('app.createOrEditPostModalSaved', getPost);

        $(document).keypress(function (e) {
            let sideBar = $('#kt_quick_sidebar');
            let modalShowing = $('.modal');

            if (e.which === 13 && (sideBar === undefined || !sideBar.hasClass('kt-quick-panel--on')) && (modalShowing === undefined || !modalShowing.hasClass('show'))) {
                getPost();
            }
        });
    });
})();