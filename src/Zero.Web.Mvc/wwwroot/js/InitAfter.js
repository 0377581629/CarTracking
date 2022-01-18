var baseHelper = baseHelper || {};

var _globalViewFileModal = new app.ModalManager({
    viewUrl: abp.appPath + 'App/Common/ViewFileModal',
    scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Common/Modals/_ViewFileModal.js',
    modalClass: 'ViewFileModal'
});

var _fileManagerModal = new app.ModalManager({
    viewUrl: abp.appPath + 'App/FilesManager/FileManagerModal',
    scriptUrl: abp.appPath + 'view-resources/Areas/App/FilesManager/_FileManagerModal.js',
    modalClass: 'FileManagerModal'
});

var select2ViLanguage = {
    inputTooLong: function (args) {
        var overChars = args.input.length - args.maximum;

        var message = 'Vui lòng xóa bớt ' + overChars + ' ký tự';

        return message;
    },
    inputTooShort: function (args) {
        var remainingChars = args.minimum - args.input.length;

        var message = 'Vui lòng nhập thêm từ ' + remainingChars +
            ' ký tự trở lên';

        return message;
    },
    loadingMore: function () {
        return 'Đang lấy thêm kết quả…';
    },
    maximumSelected: function (args) {
        var message = 'Chỉ có thể chọn được ' + args.maximum + ' lựa chọn';

        return message;
    },
    noResults: function () {
        return 'Không tìm thấy kết quả';
    },
    searching: function () {
        return 'Đang tìm…';
    },
    removeAllItems: function () {
        return 'Xóa tất cả các mục';
    }
}

FroalaEditor.DefineIcon('imageKendoFileManager', {NAME: 'fileManager', SVG_KEY: 'fileManager'});
FroalaEditor.RegisterCommand('imageKendoFileManager', {
    title: app.localize('SelectFile'),
    focus: false,
    undo: true,
    refreshAfterCallback: false,
    callback: function () {
        let frInstance = this;
        _fileManagerModal.open({
            allowExtension: '*.jpg;*.png;*.jpeg',
            maxResultCount: 1
        }, function(selectedFiles) {
            if (selectedFiles !== undefined && selectedFiles.length >= 1) {
                frInstance.image.insert(selectedFiles[selectedFiles.length - 1].path);
            }
        })
    }
});

FroalaEditor.DefineIcon('videoKendoFileManager', {NAME: 'fileManager', SVG_KEY: 'fileManager'});
FroalaEditor.RegisterCommand('videoKendoFileManager', {
    title: app.localize('SelectFile'),
    focus: false,
    undo: true,
    refreshAfterCallback: false,
    callback: function () {
        let frInstance = this;
        _fileManagerModal.open({
            allowExtension: '*.mp4',
            maxResultCount: 1
        }, function(selectedFiles) {
            if (selectedFiles !== undefined && selectedFiles.length >= 1) {
                frInstance.video.insertFromFileManager(selectedFiles[selectedFiles.length - 1].path);
            }
        })
    }
});

var frEditorBaseConfig = {
    key: "AV:4~?3xROKLJKYHROLDXDR@d2YYGR_Bc1A8@5@4:1B2D2F2F1?1?2A3@1C1",
    enter: FroalaEditor.ENTER_DIV,
    attribution: false,
    charCounterCount: true,
    toolbarButtons: {
        'moreText': {
            'buttons': ['bold', 'italic', 'underline', 'strikeThrough', 'subscript', 'superscript', 'fontFamily', 'fontSize', 'textColor', 'backgroundColor', 'inlineClass', 'inlineStyle', 'clearFormatting']
        },
        'moreParagraph': {
            'buttons': ['alignLeft', 'alignCenter', 'formatOLSimple', 'alignRight', 'alignJustify', 'formatOL', 'formatUL', 'paragraphFormat', 'paragraphStyle', 'lineHeight', 'outdent', 'indent', 'quote']
        },
        'moreRich': {
            'buttons': ['insertLink', 'insertImage', 'insertVideo', 'insertTable', 'emoticons', 'fontAwesome', 'specialCharacters', 'embedly', 'insertHR']
        },
        'moreMisc': {
            'buttons': ['undo', 'redo', 'fullscreen', 'print', 'getPDF', 'spellChecker', 'selectAll', 'html', 'help'],
            'align': 'right',
            'buttonsVisible': 2
        }
    },
    fileUploadURL: '/FroalaApi/UploadFile',
    imageInsertButtons: ["imageBack", "|", "imageByURL", "imageKendoFileManager"],
    videoInsertButtons: ["videoBack", "|", "videoByURL", "videoEmbed", "videoKendoFileManager"],
};

if (abp.localization.currentLanguage.name === 'vi') {
    frEditorBaseConfig['language'] = 'vi';
}

var frEditorConfig = $.extend({
    heightMin: 300,
}, frEditorBaseConfig);

var frEditorConfigInline = $.extend({
    heightMin: 300,
    toolbarInline: true,
}, frEditorBaseConfig);

var frEditorConfigSimple = $.extend({
    heightMin: 200,
}, frEditorBaseConfig);

var frEditorConfigSimpleInline = $.extend({
    heightMin: 200,
    toolbarInline: true,
}, frEditorBaseConfig);

let frEditorConfigHide = $.extend({
    toolbarButtons: ['undo', 'redo', '|', 'bold', 'italic', 'underline'],
    toolbarButtonsXS: ['undo', 'redo', '-', 'bold', 'italic', 'underline']
}, frEditorBaseConfig);

function whatDecimalSeparator() {
    let n = 1.1;
    n = new Intl.NumberFormat(abp.localization.currentLanguage.name).format(n).substring(1, 2);
    return n;
}

function whatThousandSeparator() {
    let decimalSeparator = whatDecimalSeparator();
    if (decimalSeparator === ',')
        return '.';
    return ',';
}

function capitalizeFirstLetter(string) {
    return string.charAt(0).toUpperCase() + string.slice(1);
}

function guid() {
    return 'a' + 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g,
        function (c) {
            let r = Math.random() * 16 | 0, v = c === 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
}

function buildNestedItem(item, showEditButton = false, showDeleteButton = false, handleIconClass = 'la la-bars', customLabel = 'customLabelClass', customEditClass = 'customSettingClass', customDeleteClass = 'customDeleteClass') {
    let ddItem = $('<li>').addClass('dd-item dd-item-alt').attr('data-id', item.id);
    let ddHandle = $('<div>').addClass('dd-handle px-0').append('<span class="icon"><i class="' + handleIconClass + '"></i></span>');
    let ddEdit = $('<div>').addClass('dd-setting').append('<span class="icon ' + customEditClass + '" data-id="' + item.id + '"><i class="la la-cog"></i></span>')
    let ddDelete = $('<div>').addClass('dd-delete').append('<span class="icon ' + customDeleteClass + '" data-id="' + item.id + '"><i class="la la-trash text-danger"></i></span>')
    let ddContent = $('<div>').addClass('dd-content').append('<label class="' + customLabel + '" data-id="' + item.id + '">' + item.displayName + '</label>')
    ddItem.append(ddHandle[0].outerHTML);
    if (showEditButton)
        ddItem.append(ddEdit[0].outerHTML);
    if (showDeleteButton)
        ddItem.append(ddDelete[0].outerHTML);
    ddItem.append(ddContent[0].outerHTML);
    if (item.children !== null) {
        let ol = $('<ol>').addClass('dd-list');
        $.each(item.children, function (index, sub) {
            ol.append(buildNestedItem(sub, showEditButton, showDeleteButton, handleIconClass, customLabel, customEditClass, customDeleteClass));
        });
        ddItem.append(ol[0].outerHTML);
    }
    return ddItem[0].outerHTML;
}

function removeTone(str) {
    str = str.toLowerCase();
    str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
    str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
    str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
    str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
    str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
    str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
    str = str.replace(/đ/g, "d");
    return str;
}

// slugify from https://gist.github.com/mathewbyrne/1280286
function slugify(text) {
    return text.toString().toLowerCase()
        .replace(/\s+/g, '-')           // Replace spaces with -
        .replace(/[^\w\-]+/g, '')       // Remove all non-word chars
        .replace(/\-\-+/g, '-')         // Replace multiple - with single -
        .replace(/^-+/, '')             // Trim - from start of text
        .replace(/-+$/, '');            // Trim - from end of text
}

(function () {
    $(function () {
        $.fn.exists = function () {
            return this.length !== 0;
        }

        baseHelper.MakeSlug = function (text) {
            return slugify(removeTone(text));
        }

        baseHelper.Select2Language = function () {
            if (abp.localization.currentLanguage.name === 'vi')
                return select2ViLanguage;
            return null;
        }

        baseHelper.UpFirstChar = function (str) {
            return capitalizeFirstLetter(str);
        }

        baseHelper.RefreshUI = function (element) {
            if (element) {
                let detailIndex = 1;

                element.find('.detailOrder').each(function () {
                    $(this).html(detailIndex);
                    detailIndex++;
                });

                element.find('.kt-select2').select2({
                    width: '100%',
                    dropdownParent: element,
                    dropdownAutoWidth: true,
                    language: baseHelper.Select2Language()
                });

                element.find('.kt-select2-non-search').select2({
                    width: '100%',
                    dropdownParent: element,
                    dropdownAutoWidth: true,
                    minimumResultsForSearch: -1,
                    language: baseHelper.Select2Language()
                });

                element.find('.kt-select2-multi-select').select2({
                    width: '100%',
                    dropdownAutoWidth: true,
                    multiple: true,
                    closeOnSelect: false,
                    language: baseHelper.Select2Language()
                });

                element.find('.touchSpin').TouchSpin({
                    verticalbuttons: true,
                    verticalupclass: 'btn-secondary',
                    verticaldownclass: 'btn-secondary'
                });

                element.find('.number').number(true, 0, whatDecimalSeparator(), whatThousandSeparator());
                element.find('.number1').number(true, 1, whatDecimalSeparator(), whatThousandSeparator());
                element.find('.number2').number(true, 2, whatDecimalSeparator(), whatThousandSeparator());
                element.find('.number3').number(true, 3, whatDecimalSeparator(), whatThousandSeparator());
                element.find('.numberOther').number(true, 0, '', '');

                element.find('.date-picker').each(function () {
                    $(this).datetimepicker({
                        locale: abp.localization.currentLanguage.name,
                        format: 'L'
                    });

                    if ($(this).attr('init-value') !== undefined && $(this).attr('init-value').length > 0) {
                        let initDate = moment($(this).attr('init-value'), 'DD/MM/YYYY').format('L');
                        $(this).val(initDate);
                    }
                });

                element.find('.datetime-picker').each(function () {
                    $(this).datetimepicker({
                        locale: abp.localization.currentLanguage.name,
                        format: 'L LT'
                    });
                    if ($(this).attr('init-value') !== undefined && $(this).attr('init-value').length > 0) {
                        let initDate = moment($(this).attr('init-value'), 'DD/MM/YYYY hh:mm').format('L LT');
                        $(this).val(initDate);
                    }

                });

                element.find('.month-picker').datetimepicker({
                    locale: abp.localization.currentLanguage.name,
                    format: 'MM/YYYY'
                });

                element.find('.make-slug').focusout(function () {
                    let originalString = $(this).val();
                    if (originalString === '')
                        return; // Do nothing if nothing in the MakeSlug input
                    let slugInput = $('.slug-input[data-target=' + $(this).attr('id') + ']');
                    if (!slugInput) {
                        console.log('Error: MakeSlug has invalid SlugInput');
                        return;
                    }
                    if (slugInput.val() !== '')
                        return; // SlugInput isn't empty. It already generated or entered by user before
                    let slug = baseHelper.MakeSlug(originalString);
                    slugInput.val(slug);
                });

                element.find('.frSimpleEditor[initEditor="false"]').each(function () {
                    let id = $(this).attr('id');
                    if (id !== undefined && id !== null) {
                        new FroalaEditor($(this).get(0), frEditorConfigSimple)
                        $(this).removeAttr('initEditor');
                    }
                });
            }
        }

        baseHelper.MiniMenu = function () {
            if (!$('body').hasClass('aside-minimize'))
                $('body').addClass('aside-minimize');
        }

        baseHelper.SimpleTableIcon = function (funcName) {
            switch (funcName) {
                case 'send':
                    return 'la la-send text-primary';
                case 'approve':
                    return 'la la-check text-success';
                case 'return':
                    return 'la la-reply  text-dark';
                case 'edit':
                    return 'la la-edit text-primary';
                case 'delete':
                    return 'la la-trash text-danger';
                case 'view':
                    return 'la la-eye text-info';
                case 'attach':
                    return 'la la-paperclip text-success'
            }
            return '';
        };

        baseHelper.SimpleSelector = function (element, placeHolder, serviceUrl, showWithCode = false) {
            if (serviceUrl !== undefined && serviceUrl[0] === '/')
                serviceUrl = serviceUrl.substring(1, serviceUrl.length);
            element.select2({
                dropdownParent: element.parent(),
                placeholder: placeHolder,
                allowClear: true,
                width: '100%',
                language: baseHelper.Select2Language(),
                ajax: {
                    url: abp.appPath + "api/services/app/" + serviceUrl,
                    dataType: 'json',
                    delay: 50,
                    data: function (params) {
                        return {
                            filter: params.term,
                            skipCount: ((params.page || 1) - 1) * 10
                        };
                    },
                    processResults: function (data, params) {
                        params.page = params.page || 1;

                        let res = $.map(data.result.items, function (item) {
                            return {
                                text: showWithCode ? item.code + ' - ' + item.name : item.name,
                                id: item.id
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
                }
            });
        }

        baseHelper.SimpleRequiredSelector = function (element, placeHolder, serviceUrl, showWithCode = false) {
            return baseHelper.SimpleSelector(element, placeHolder, serviceUrl, showWithCode);
        }

        baseHelper.SelectSingleFile = function (allow, selectFileButton, cancelFileButton, fileName, fileUrl, imgHolder, wrapper, customClassAfterChange) {
            if (allow === undefined || allow == null)
                allow = "*.jpg,*.png";
            if (selectFileButton) {
                selectFileButton.on('click', function () {
                    _fileManagerModal.open({
                        allowExtension: allow,
                        maxSelectCount: 1
                    }, function (selected) {
                        if (selected !== undefined && selected.length >= 1) {
                            if (fileUrl) fileUrl.val(selected[selected.length - 1].path);
                            if (fileName) fileName.val(selected[selected.length - 1].name);
                            if (imgHolder) imgHolder.attr('src', selected[selected.length - 1].path);
                            if (wrapper && customClassAfterChange) wrapper.addClass(customClassAfterChange);
                        }  if (cancelFileButton) {
                            cancelFileButton.on('click', function () {
                                if (fileUrl) fileUrl.val('');
                                if (fileName) fileName.val('');
                                if (imgHolder) imgHolder.attr('src', imgHolder.attr('default-src'));
                                if (wrapper && customClassAfterChange) wrapper.removeClass(customClassAfterChange);
                            });
                        }
                    });
                });
            }
          
        }

        baseHelper.ShowCheckBox = function (id, customClass = '', checked = false) {
            let checkBoxLabel = $('<label>').addClass('checkbox checkbox-outline').css('display', 'inline-block');
            let checkBoxInput = $('<input>').attr('type', 'checkbox').attr('value', 'true').attr('customId', id).attr('id', guid()).addClass(customClass);
            let checkBoxSpan = $('<span>').addClass('mr-0');
            if (checked === true) {
                checkBoxInput.attr('checked', 'checked');
            }
            checkBoxLabel.append(checkBoxInput[0].outerHTML);
            checkBoxLabel.append(checkBoxSpan[0].outerHTML);
            return checkBoxLabel[0].outerHTML;
        }

        baseHelper.ShowImage = function (image, fallBackImgSrc = '../../Common/Images/no_image_available.svg') {
            let img = $("<img>");
            img.addClass('w-50px h-50px b-rd-50');
            img.attr('src', fallBackImgSrc);
            img.attr('onerror', "src='" + fallBackImgSrc + "'");
            if (image !== undefined && image !== null && image.length !== 0) {
                img.attr('src', image);
            }
            return img[0].outerHTML;
        }

        baseHelper.ShowAvatar = function (avatar) {
            return baseHelper.ShowImage(avatar, '../../Common/Images/default-profile-picture.jpg');
        }

        baseHelper.ShowDefaultStatus = function (status) {
            let $span = $("<span/>");
            if (status === 0) {
                $span.addClass("badge badge-danger").text(app.localize('InvalidStatus'));
            } else if (status === 1) {
                $span.addClass("badge badge-light").text(app.localize('Draft'));
            } else if (status === 2) {
                $span.addClass("badge badge-dark").text(app.localize('WaitingForApproval'));
            } else if (status === 3) {
                $span.addClass("badge badge-success").text(app.localize('Approved'));
            } else if (status === 4) {
                $span.addClass("badge badge-warning").text(app.localize('Return'));
            } else if (status === 5) {
                $span.addClass("badge badge-info").text(app.localize('Locked'));
            }
            return $span[0].outerHTML;
        }

        baseHelper.ShowActive = function (isActive) {
            let $span = $("<span/>");
            if (isActive) {
                $span.addClass("badge badge-success");
            }
            return $span[0].outerHTML;
        }

        baseHelper.ShowDefault = function (isDefault) {
            let $span = $("<span/>");
            if (isDefault) {
                $span.addClass("badge badge-success");
            }
            return $span[0].outerHTML;
        }

        baseHelper.ShowColor = function (colorInHex) {
            let $span = $("<span/>");
            if (colorInHex !== undefined && colorInHex !== null && colorInHex.length > 0) {
                $span.addClass("badge").css('background-color', colorInHex);
            }
            return $span[0].outerHTML;
        }

        baseHelper.ShowWidgetContentType = function (type) {
            switch (type) {
                case 1:
                    return app.localize('WidgetContentType_FixedContent');
                case 2:
                    return app.localize('WidgetContentType_Service');
                case 3:
                    return app.localize('WidgetContentType_ServiceType');
                case 4:
                    return app.localize('WidgetContentType_ServiceCategory');
                case 5:
                    return app.localize('WidgetContentType_ServiceArticle');
                case 6:
                    return app.localize('WidgetContentType_ReviewPost');
                case 7:
                    return app.localize('WidgetContentType_ImageBlock');
                case 8:
                    return app.localize('WidgetContentType_CustomContent');
                case 9:
                    return app.localize('WidgetContentType_ServicePropertyGroup');
                case 12:
                    return app.localize('WidgetContentType_MenuGroup');
            }
        }

        baseHelper.CanEdit = function (havePermission, currentStatus, allowedStatus, allowEdit) {
            if (allowEdit !== null && allowEdit !== undefined && allowEdit === false)
                return false;
            if (currentStatus === null || currentStatus === undefined)
                return havePermission;
            let allowed = [0, 1, 4];
            if (allowedStatus === null || allowedStatus === undefined) {
                allowedStatus = allowed;
            }
            return havePermission && jQuery.inArray(parseInt(currentStatus), allowedStatus) !== -1;
        }

        baseHelper.CanRequestApprove = function (havePermission, currentStatus, allowedStatus, allowEdit) {
            if (allowEdit !== null && allowEdit !== undefined && allowEdit === false)
                return false;
            if (currentStatus === null || currentStatus === undefined)
                return havePermission;
            let allowed = [0, 1];
            if (allowedStatus === null || allowedStatus === undefined) {
                allowedStatus = allowed;
            }
            return havePermission && jQuery.inArray(parseInt(currentStatus), allowedStatus) !== -1;
        }

        baseHelper.RequestApprove = function (obj, service, reloadCallback, targetStatus) {
            abp.message.confirm(
                '',
                app.localize('ApproveRequestMessageWarningTitle'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        service.updateStatus({
                            id: obj.id,
                            status: targetStatus !== undefined ? targetStatus : 2
                        }).done(function () {
                            reloadCallback();
                            abp.notify.success(app.localize('Successfully'));
                        });
                    }
                }
            );
        }

        baseHelper.Approve = function (obj, service, reloadCallback, targetStatus) {
            abp.message.confirm(
                '',
                app.localize('ApproveMessageWarningTitle'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        service.updateStatus({
                            id: obj.id,
                            status: targetStatus !== undefined ? targetStatus : 3
                        }).done(function () {
                            reloadCallback();
                            abp.notify.success(app.localize('Successfully'));
                        });
                    }
                }
            );
        }

        baseHelper.CanApprove = function (havePermission, currentStatus, allowedStatus, allowEdit) {
            if (allowEdit !== null && allowEdit !== undefined && allowEdit === false)
                return false;
            if (currentStatus === null || currentStatus === undefined)
                return havePermission;
            let allowed = [2];
            if (allowedStatus === null || allowedStatus === undefined) {
                allowedStatus = allowed;
            }
            return havePermission && jQuery.inArray(parseInt(currentStatus), allowedStatus) !== -1;
        }

        baseHelper.CancelApprove = function (obj, service, reloadCallback, targetStatus) {
            abp.message.confirm(
                '',
                app.localize('ApproveCancelMessageWarningTitle'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        service.updateStatus({
                            id: obj.id,
                            status: targetStatus !== undefined ? targetStatus : 4
                        }).done(function () {
                            reloadCallback();
                            abp.notify.success(app.localize('Successfully'));
                        });
                    }
                }
            );
        }

        baseHelper.CanCancelApprove = function (havePermission, currentStatus, allowedStatus, allowEdit) {
            if (allowEdit !== null && allowEdit !== undefined && allowEdit === false)
                return false;
            if (currentStatus === null || currentStatus === undefined)
                return havePermission;
            let allowed = [3];
            if (allowedStatus === null || allowedStatus === undefined) {
                allowedStatus = allowed;
            }
            return havePermission && jQuery.inArray(parseInt(currentStatus), allowedStatus) !== -1;
        }

        baseHelper.Delete = function (obj, service, reloadCallback) {
            abp.message.confirm(
                '',
                app.localize('DeleteMessageWarningTitle'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        service.delete({
                            id: obj.id
                        }).done(function () {
                            reloadCallback();
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                        });
                    }
                }
            );
        }

        baseHelper.CanDelete = function (havePermission, currentStatus, allowedStatus, allowEdit) {
            if (allowEdit !== null && allowEdit !== undefined && allowEdit === false)
                return false;
            if (currentStatus === null || currentStatus === undefined)
                return havePermission;
            let allowed = [0, 1];
            if (allowedStatus === null || allowedStatus === undefined) {
                allowedStatus = allowed;
            }
            return havePermission && jQuery.inArray(parseInt(currentStatus), allowedStatus) !== -1;
        }

        baseHelper.ShowCheckInModal = function (record) {
            let obj = record[Object.keys(record)[0]];
            if (obj !== undefined) {
                let objectDto = obj;
                let chkInp = $("<input/>").addClass('modalSelectChecker');
                chkInp.attr('id', 'checkbox_' + objectDto.id);
                chkInp.attr('ModalSelectObjId', objectDto.id);
                chkInp.attr('type', 'checkbox');

                $.each(objectDto, function (key, val) {
                    if (key === 'id')
                        chkInp.attr('customId', val);
                    else if (key === 'type')
                        chkInp.attr('customType', val);
                    else
                        chkInp.attr(key, val);
                });

                if (record.selected) {
                    chkInp.attr('checked', 'checked');
                }

                return '<label for="checkbox_' + objectDto.id + '" class="kt-checkbox kt-checkbox--primary h-20 w-20 mt-0 pl-20">' +
                    chkInp[0].outerHTML +
                    '<span></span>' +
                    '</label>';
            }
            return '';
        }

        baseHelper.ShowNumber = function (input, floatCount = 0) {
            if (input !== undefined && parseFloat(input) !== parseFloat("0"))
                return $.number(input, floatCount, whatDecimalSeparator(), whatThousandSeparator());
            return '';
        }

        baseHelper.ShowEmailTemplateType = function (type) {
            switch (type) {
                case null:
                    return app.localize('EmailTemplateType_Default');
                case 2:
                    return app.localize('EmailTemplateType_UserActiveEmail');
                case 3:
                    return app.localize('EmailTemplateType_UserResetPassword');
                case 4:
                    return app.localize('EmailTemplateType_SecurityCode');
            }
        }

        baseHelper.ViewFile = function (fileUrl) {
            _globalViewFileModal.open({
                path: fileUrl
            });
        }

        baseHelper.NestedItemsHtml = function (items, showEditButton = false, showDeleteButton, handleIconClass = 'la la-bars', customLabel = 'customLabelClass', customEditClass = 'customSettingClass', customDeleteClass = 'customDeleteClass') {
            let output = '';
            $.each(items, function (index, item) {
                output += buildNestedItem(item, showEditButton, showDeleteButton, handleIconClass, customLabel, customEditClass, customDeleteClass);
            });

            return output;
        }

        baseHelper.NestedItemHtml = function (item, showSettingButton = false, showDeleteButton = false, handleIconClass = 'la la-bars', customLabel = 'customLabelClass', customEditClass = 'customSettingClass', customDeleteClass = 'customDeleteClass') {
            return buildNestedItem(item, showSettingButton, showDeleteButton, handleIconClass, customLabel, customEditClass, customDeleteClass);
        }

        baseHelper.ValidSelectors = function () {
            let res = true;
            $('body').find('select.requiredSelect2').each(function () {
                let selectId = $(this).attr('id');
                if ($(this).hasClass('requiredSelect2') && ($(this).val() === null || $(this).val() === undefined || $(this).val() === '')) {
                    $('body').find('#select2-' + selectId + '-container').parent().addClass('invalid');
                    res = false;
                } else {
                    $('body').find('#select2-' + selectId + '-container').parent().removeClass('invalid');
                }
            });
            return res;
        }

        $('.kt-select2').select2({
            width: '100%',
            dropdownAutoWidth: true,
            language: baseHelper.Select2Language()
        });

        $('.kt-select2-non-search').select2({
            width: '100%',
            dropdownAutoWidth: true,
            minimumResultsForSearch: -1,
            language: baseHelper.Select2Language()
        });

        $('.kt-select2-multi-select').select2({
            width: '100%',
            dropdownAutoWidth: true,
            multiple: true,
            closeOnSelect: false,
            language: baseHelper.Select2Language()
        });

        $('.date-picker').each(function () {
            $(this).datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            if ($(this).attr('init-value') !== undefined) {
                let initDate = moment($(this).attr('init-value'), 'DD/MM/YYYY').format('L');
                $(this).val(initDate);
            }
        });

        $('.datetime-picker').each(function () {
            $(this).datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L LT'
            });
            if ($(this).attr('init-value') !== undefined) {
                let initDate = moment($(this).attr('init-value'), 'DD/MM/YYYY hh:mm').format('L LT');
                $(this).val(initDate);
            }
        });

        $('.month-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'MM/YYYY'
        });

        $('.number').number(true, 0, whatDecimalSeparator(), whatThousandSeparator());
        $('.number1').number(true, 1, whatDecimalSeparator(), whatThousandSeparator());
        $('.number2').number(true, 2, whatDecimalSeparator(), whatThousandSeparator());
        $('.number3').number(true, 3, whatDecimalSeparator(), whatThousandSeparator());
        $('.numberOther').number(true, 0, '', '');

        $(".mScrollBar").mCustomScrollbar({
            theme: "minimal-dark"
        });

        $(document).on('hidden.bs.modal', '.modal', function () {
            $('.modal:visible').length && $(document.body).addClass('modal-open');
        });

        $('.make-slug').focusout(function () {
            let originalString = $(this).val();
            if (originalString === '')
                return; // Do nothing if nothing in the MakeSlug input
            let slugInput = $('.slug-input[data-target=' + $(this).attr('id') + ']');
            if (!slugInput) {
                console.log('Error: MakeSlug has invalid SlugInput');
                return;
            }
            if (slugInput.val() !== '')
                return; // SlugInput isn't empty. It already generated or entered by user before
            let slug = baseHelper.MakeSlug(originalString);
            slugInput.val(slug);
        });
        
        $('.frSimpleEditor[initEditor="false"]').each(function () {
            let id = $(this).attr('id');
            if (id !== undefined && id !== null) {
                new FroalaEditor($(this).get(0), frEditorConfigSimple)
                $(this).removeAttr('initEditor');
            }
        });
    });
})();