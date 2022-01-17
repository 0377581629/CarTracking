(function ($) {
    app.modals.FileManagerModal = function () {
        let _modalManager;
        let modal;
        let _fileManager;
        let _fileManagerPager;
        let _fileManagerDataSource;
        let _fileManagerPageSize = 30;
        let _btnSelect;
        let _allowExtension;
        let _selectorResult = [];
        let _maxSelectCount = 1;

        let initFileManager = _.debounce(function () {
            InitFileManager();
        }, 300);

        function InitFileManager() {
            _fileManager = modal.find('#fileManager');
            _fileManagerPager = modal.find('#fileManagerPager');
            _fileManagerDataSource = new kendo.data.FileManagerDataSource({
                schema: kendo.data.schemas.filemanager,
                transport: {
                    read: {
                        url: "/App/FilesManager/Read",
                        method: "POST",
                        data: {
                            filter: _modalManager.getArgs().allowExtension
                        }
                    },
                    create: {
                        url: "/App/FilesManager/Create",
                        method: "POST"
                    },
                    update: {
                        url: "/App/FilesManager/Update",
                        method: "POST"
                    },
                    destroy: {
                        url: "/App/FilesManager/Destroy",
                        method: "POST"
                    }
                },
                pageSize: _fileManagerPageSize
            });

            let fileManagerConfig = {
                dataSource: _fileManagerDataSource,
                uploadUrl: "/App/FilesManager/Upload",
                upload: {
                    upload: function (e) {
                        e.data = {
                            __RequestVerificationToken: abp.security.antiForgery.getToken()
                        }
                    }
                },
                contextMenu: {
                    items: [
                        {name: "rename", text: app.localize('Rename'), command: "RenameCommand"},
                        {name: "delete", text: app.localize('Delete'), command: "DeleteCommand"}
                    ]
                },
                select: FileManagerOnSelect,
                previewPane: {
                    singleFileTemplate: kendo.template($("#file-manager-preview-template").html())
                },
                views: {
                    list: {
                        scrollable: "endless",
                        height: 400,
                        template: kendo.template($("#file-manager-list-view-template").html()),
                    },
                    grid: {
                        pageable: {
                            pageSize: _fileManagerPageSize
                        },
                        columns: [
                            {
                                title: '',
                                template: function (item) {
                                    let icon = !item.isDirectory ? kendo.getFileGroup(item.extension, true) : 'folder';

                                    if (kendo.getFileGroup(item.extension, true) === "file-image" && item.size < 1000000) {
                                        return '<img src="' + item.actualPath + '" style="max-width: 28px; max-height: 28px"/>'
                                    }

                                    return '<div class=\'file-group-icon\'>' +
                                        '<span class=\'k-icon k-i-' + icon + '\'></span>' +
                                        '</div>';
                                },
                                width: "40px",
                                headerAttributes: {
                                    "class": "k-text-center",
                                    style: "font-weight: bold"
                                },
                                attributes: {
                                    "class": "table-cell k-text-center"
                                }
                            },
                            {
                                field: 'name',
                                title: app.localize('Name'),
                                template: function (item) {
                                    return '<div class=\'file-name\'>' + kendo.htmlEncode(item.name + item.extension) + '<div>';
                                },
                                headerAttributes: {
                                    "class": "k-text-center",
                                    style: "font-weight: bold"
                                }
                            },
                            {
                                field: 'size',
                                title: app.localize('FileSize'),
                                template: function (item) {
                                    if (!item.isDirectory) {
                                        return kendo.getFileSizeMessage(item.size);
                                    } else {
                                        return '';
                                    }
                                },
                                width: "80px",
                                headerAttributes: {
                                    "class": "k-text-center",
                                    style: "font-weight: bold"
                                },
                                attributes: {
                                    "class": "table-cell k-text-right"
                                }
                            },
                            {
                                field: 'modified',
                                title: app.localize('FileModifiedDate'),
                                template: function (item) {
                                    if (item.modified.getYear() > 1)
                                        return kendo.toString(item.modified, "G");
                                    return '';
                                },
                                width: "100px",
                                headerAttributes: {
                                    "class": "k-text-center",
                                    style: "font-weight: bold"
                                },
                                attributes: {
                                    "class": "table-cell k-text-center"
                                }
                            }
                        ]
                    }
                }
            };

            if (abp.localization.currentLanguage.name === 'vi') {
                fileManagerConfig = $.extend({
                    messages: {
                        toolbar: {
                            createFolder: "Thư mục mới",
                            upload: "Tải lên",
                            sortDirection: "Sort Direction",
                            sortDirectionAsc: "Sort Direction Ascending",
                            sortDirectionDesc: "Sort Direction Descending",
                            sortField: "Sắp xếp bởi",
                            nameField: "Tên",
                            sizeField: "Kích thước",
                            typeField: "Phân loại",
                            dateModifiedField: "Ngày chỉnh sửa",
                            dateCreatedField: "Ngày tạo",
                            listView: "Xem danh sách",
                            gridView: "Xem lưới",
                            search: "Tìm kiếm",
                            details: "Xem chi tiết",
                            detailsChecked: "Bật",
                            detailsUnchecked: "Tắt",
                            "delete": "Xóa",
                            rename: "Đổi tên"
                        },
                        views: {
                            nameField: "Tên",
                            sizeField: "Kích thước",
                            typeField: "Phân loại",
                            dateModifiedField: "Ngày chỉnh sửa",
                            dateCreatedField: "Ngày tạo",
                            items: "items"
                        },
                        dialogs: {
                            upload: {
                                title: "Tải lên",
                                clear: "Xóa danh sách",
                                done: "Hoàn thành"
                            },
                            moveConfirm: {
                                title: "Di chuyển",
                                content: "<p style='text-align: center;'>Bạn có chắc chắn muốn sao chép hoặc di chuyển?</p>",
                                okText: "Xác nhận",
                                cancel: "Hủy",
                                close: "Thoát"
                            },
                            deleteConfirm: {
                                title: "Xóa",
                                content: "<p style='text-align: center;'>Bạn có chắc chắn muốn xóa các tệp tin đã chọn ?</br>Bạn không thể hoàn lại thao tác này.</p>",
                                okText: "Xác nhận",
                                cancel: "Hủy",
                                close: "Thoát"
                            },
                            renamePrompt: {
                                title: "Đổi tên",
                                content: "<p style='text-align: center;'>Nhập tên file mới.</p>",
                                okText: "Xác nhận",
                                cancel: "Hủy",
                                close: "Thoát"
                            }
                        },
                        previewPane: {
                            noFileSelected: "Chưa có file nào được chọn",
                            extension: "Phân loại",
                            size: "Kích thước",
                            created: "Ngày tạo",
                            createdUtc: "Ngày tạo UTC",
                            modified: "Ngày chỉnh sửa",
                            modifiedUtc: "Ngày chỉnh sửa UTC",
                            items: "items"
                        }
                    }
                }, fileManagerConfig);
            }

            _fileManager.kendoFileManager(fileManagerConfig);

            _btnSelect.click(function () {
                _modalManager.setResult(_selectorResult);
                _modalManager.close();
            });
        }

        function FileManagerOnSelect(e) {
            _selectorResult = [];
            if (e.entries !== undefined && e.entries.length <= _maxSelectCount && e.entries.every(item => jQuery.inArray(item.extension.toLowerCase(), _allowExtension) !== -1)) {
                _btnSelect.removeClass('hidden');
                for (let i = 0; i < e.entries.length; i++) {
                    let item = e.entries[i];
                    _selectorResult.push({
                        name: item.name + item.extension,
                        path: item.actualPath
                    });
                }
            } else {
                _btnSelect.addClass('hidden');
            }
        }

        this.init = function (modalManager) {
            _modalManager = modalManager;
            modal = _modalManager.getModal();
            modal.find('.modal-dialog').addClass('modal-xl');
            modal.find('#Allowed').html(_modalManager.getArgs().allowExtension);
            let modalArgs = _modalManager.getArgs();
            if (modalArgs !== undefined && modalArgs.allowExtension !== undefined)
                _allowExtension = _modalManager.getArgs().allowExtension.replaceAll('*', '').split(";");
            if (modalArgs !== undefined && modalArgs.maxSelectCount !== undefined)
                _maxSelectCount = parseInt(_modalManager.getArgs().maxSelectCount);

            if (_allowExtension === undefined) {
                _allowExtension = [];
            }

            if (_maxSelectCount === undefined) {
                _maxSelectCount = 1;
            }

            _btnSelect = modal.find('#btnSelectFile');

            initFileManager();
        };
    };
})(jQuery);