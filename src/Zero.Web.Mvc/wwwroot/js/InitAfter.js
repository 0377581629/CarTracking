var baseHelper = baseHelper || {};

var _globalViewFileModal = new app.ModalManager({
    viewUrl: abp.appPath + 'App/Common/ViewFileModal',
    scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Common/Modals/_ViewFileModal.js',
    modalClass: 'ViewFileModal'
});

var _fileManagerModal = new app.ModalManager({
    viewUrl: abp.appPath + 'App/FileManagerData/FileManagerModal',
    scriptUrl: abp.appPath + 'view-resources/Areas/App/FileManagerData/_FileManagerModal.js',
    modalClass: 'FileManagerModal'
});

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

function capitalizeFirstLetter(string){
    return string.charAt(0).toUpperCase() + string.slice(1);
}

function guid() {
    return 'a' + 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g,
        function(c)
        {
            let r = Math.random() * 16 | 0, v = c === 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
}

function buildNestedItem(item, showEditButton = false, showDeleteButton = false, handleIconClass='la la-bars', customLabel = 'customLabelClass', customEditClass = 'customSettingClass', customDeleteClass = 'customDeleteClass') {
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
            ol.append(buildNestedItem(sub, showEditButton, showDeleteButton, handleIconClass,customLabel, customEditClass, customDeleteClass));
        });
        ddItem.append(ol[0].outerHTML);
    }
    return ddItem[0].outerHTML;
}

(function () {
    $(function () {
        $.fn.exists = function () {
            return this.length !== 0;
        }
        baseHelper.UpFirstChar = function(str) {
            return capitalizeFirstLetter(str);
        }
        
        baseHelper.RefreshUI = function(element) {
            if (element) {
                let detailIndex = 1;
                element.find('.detailOrder').each(function() {
                    $(this).html(detailIndex);
                    detailIndex++;
                });
                element.find('.kt-select2').select2({
                    width: '100%',
                    dropdownAutoWidth: true
                });
                element.find('.kt-select2-non-search').select2({
                    width: '100%',
                    dropdownAutoWidth: true,
                    minimumResultsForSearch: -1
                });
                element.find('.touchSpin').TouchSpin({
                    verticalbuttons: true,
                    verticalupclass: 'btn-secondary',
                    verticaldownclass: 'btn-secondary'
                });

                element.find('.number').number(true, 0, whatDecimalSeparator(),whatThousandSeparator());
                element.find('.number1').number(true, 1, whatDecimalSeparator(),whatThousandSeparator());
                element.find('.number2').number(true, 2, whatDecimalSeparator(),whatThousandSeparator());
                element.find('.number3').number(true, 3, whatDecimalSeparator(),whatThousandSeparator());
                element.find('.numberOther').number(true, 0, '', '');
                element.find('.date-picker').datetimepicker({
                    locale: abp.localization.currentLanguage.name,
                    format: 'L'
                });

                element.find('.datetime-picker').datetimepicker({
                    locale: abp.localization.currentLanguage.name,
                    format: 'L LT'
                });

                element.find('.month-picker').datetimepicker({
                    locale: abp.localization.currentLanguage.name,
                    format: 'MM/YYYY'
                });
            }
        }
        
        baseHelper.MiniMenu = function () {
            if (!$('body').hasClass('aside-minimize'))
                $('body').addClass('aside-minimize');
        }

        baseHelper.SimpleTableIcon = function(funcName) {
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
            }
            return '';
        };
        
        baseHelper.SelectSingleFile = function(allow, selectFileButton, cancelFileButton, fileName, fileUrl, imgHolder, wrapper, customClassAfterChange) {
            
            if (allow === undefined || allow == null)
                allow = "*.jpg,*.png";
            if (selectFileButton) {
                selectFileButton.on('click', function(){
                    _fileManagerModal.open({
                        allowExtension: allow,
                        maxSelectCount: 1
                    }, function (selected) {
                        if (selected !== undefined && selected.length === 1) {
                            if (fileUrl) fileUrl.val(selected[0].path);
                            if (fileName) fileName.val(selected[0].name);
                            if (imgHolder) imgHolder.attr('src', selected[0].path);
                            if (wrapper && customClassAfterChange) wrapper.addClass(customClassAfterChange);
                        }
                    });
                });
            }
            if (cancelFileButton) {
                cancelFileButton.on('click', function () {
                    if (fileUrl) fileUrl.val('');
                    if (fileName) fileName.val('');
                    if (imgHolder) imgHolder.attr('src', imgHolder.attr('default-src'));
                    if (wrapper && customClassAfterChange)wrapper.removeClass(customClassAfterChange);
                });
            }
        }
        
        baseHelper.ShowCheckBox = function (id, customClass = '', checked = false) {
            let checkBoxLabel = $('<label>').addClass('checkbox checkbox-outline').css('display', 'inline-block');
            let checkBoxInput = $('<input>').attr('type', 'checkbox').attr('value', 'true').attr('customId', id).attr('id', guid()).addClass(customClass);
            let checkBoxSpan = $('<span>').addClass('mr-0');
            if (checked === true) {
                checkBoxInput.attr('checked','checked');
            }
            checkBoxLabel.append(checkBoxInput[0].outerHTML);
            checkBoxLabel.append(checkBoxSpan[0].outerHTML);
            return checkBoxLabel[0].outerHTML;
        }
        
        baseHelper.ShowAvatar = function (avatar) {
            let img = $("<img>");
            img.addClass('w-50 h-50 b-rd-50');
            img.attr('src', '../../Common/Images/default-profile-picture.jpg');
            img.attr('onerror', "src='../../Common/Images/default-profile-picture.jpg'");
            if (avatar !== undefined && avatar !== null && avatar.length !== 0) {
                img.attr('src', avatar);
            }
            return img[0].outerHTML;
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

        baseHelper.ShowTrainingPlanStatus = function (status) {
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
            } else if (status === 6) {
                $span.addClass("badge badge-info").text(app.localize('HaveResult'));
            }
            return $span[0].outerHTML;
        }

        baseHelper.ShowRecruitmentProcessStatus = function (status) {
            let $span = $("<span/>");
            if (status === 0) {
                $span.addClass("badge badge-light").text(app.localize('Recruitment_Process_Waiting'));
            } else if (status === 1) {
                $span.addClass("badge badge-info").text(app.localize('Recruitment_Process_Planned'));
            } else if (status === 2) {
                $span.addClass("badge badge-primary").text(app.localize('Recruitment_Process_Interviewing'));
            } else if (status === 3) {
                $span.addClass("badge badge-warning").text(app.localize('Recruitment_Process_Rejected'));
            } else if (status === 4) {
                $span.addClass("badge badge-success").text(app.localize('Recruitment_Process_Passed'));
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

        baseHelper.ShowColor = function (colorInHex) {
            console.log(colorInHex);
            let $span = $("<span/>");
            if (colorInHex !== undefined && colorInHex !== null && colorInHex.length > 0) {
                $span.addClass("badge").css('background-color', colorInHex);
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
        
        baseHelper.ShowFileTypeName = function(type) {
            switch (type) {
                case 1:
                    return app.localize('FileTemplateType_EmployeeCV');
                case 2:
                    return app.localize('FileTemplateType_EmployeeContract');
                case 3:
                    return app.localize('FileTemplateType_Reward');
                case 4:
                    return app.localize('FileTemplateType_Discipline');
                case 5:
                    return app.localize('FileTemplateType_ChangeOrganization');
                case 6:
                    return app.localize('FileTemplateType_ChangeBenefit');
                case 7:
                    return app.localize('FileTemplateType_GrantAssets');
            }
        }

        baseHelper.ShowSalaryComponentTypeScopeName = function(type) {
            switch (type) {
                case 1:
                    return app.localize('AllCompany');
                case 2:
                    return app.localize('WorkGroup');
                case 3:
                    return app.localize('WorkDepartment');
                case 4:
                    return app.localize('WorkParty');
                case 5:
                    return app.localize('Employee');
            }
        }

        baseHelper.CanEdit = function(havePermission, currentStatus, allowedStatus, allowEdit) {
            if (allowEdit !== null && allowEdit !== undefined && allowEdit === false)
                return false;
            if (currentStatus === null || currentStatus === undefined)
                return havePermission;
            let allowed = [0,1,4];
            if (allowedStatus === null || allowedStatus === undefined) {
                allowedStatus = allowed;
            }
            return havePermission && jQuery.inArray(parseInt(currentStatus), allowedStatus) !== -1;
        }
        
        baseHelper.CanRequestApprove = function(havePermission, currentStatus, allowedStatus, allowEdit) {
            if (allowEdit !== null && allowEdit !== undefined && allowEdit === false)
                return false;
            if (currentStatus === null || currentStatus === undefined)
                return havePermission;
            let allowed = [0,1];
            if (allowedStatus === null || allowedStatus === undefined) {
                allowedStatus = allowed;
            }
            return havePermission && jQuery.inArray(parseInt(currentStatus), allowedStatus) !== -1;
        }
        
        baseHelper.RequestApprove = function(obj, service, reloadCallback, targetStatus) {
            abp.message.confirm(
                '',
                app.localize('ApproveRequestMessageWarningTitle'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        service .updateStatus({
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
        
        baseHelper.Approve = function(obj, service, reloadCallback, targetStatus) {
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

        baseHelper.CanApprove = function(havePermission, currentStatus, allowedStatus, allowEdit) {
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
        
        baseHelper.CancelApprove = function(obj, service, reloadCallback, targetStatus) {
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

        baseHelper.CanCancelApprove = function(havePermission, currentStatus, allowedStatus, allowEdit) {
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

        baseHelper.Delete = function(obj, service, reloadCallback) {
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

        baseHelper.CanDelete = function(havePermission, currentStatus, allowedStatus, allowEdit) {
            if (allowEdit !== null && allowEdit !== undefined && allowEdit === false)
                return false;
            if (currentStatus === null || currentStatus === undefined)
                return havePermission;
            let allowed = [0,1];
            if (allowedStatus === null || allowedStatus === undefined) {
                allowedStatus = allowed;
            }
            return havePermission && jQuery.inArray(parseInt(currentStatus), allowedStatus) !== -1;
        }
        
        baseHelper.ShowCheckInModal = function(record) {
            let obj = record[Object.keys(record)[0]];
            
            
            if (obj !== undefined) {
                let objectDto = obj;
                let chkInp = $("<input/>").addClass('modalSelectChecker');
                chkInp.attr('id','checkbox_' + objectDto.id);
                chkInp.attr('ModalSelectObjId', objectDto.id);
                chkInp.attr('type','checkbox');

                $.each(objectDto, function (key, val) {
                    if (key === 'id')
                        chkInp.attr('customId', val);
                    else if (key === 'type')
                        chkInp.attr('customType', val);
                    else
                        chkInp.attr(key, val);
                });

                if (record.selected) {
                    chkInp.attr('checked','checked');
                }

                return '<label for="checkbox_' + objectDto.id + '" class="kt-checkbox kt-checkbox--primary h-20 w-20 mt-0 pl-20">' +
                    chkInp[0].outerHTML +
                    '<span></span>' +
                    '</label>';
            }
            return '';
        }
        
        baseHelper.ShowNumber = function(input, floatCount = 0) {
            if (input !== undefined && parseFloat(input) !== parseFloat("0")) 
                return $.number(input,floatCount, whatDecimalSeparator(),whatThousandSeparator());
            return '';
        }

        baseHelper.PayFormTypeName = function(input = 0) {
            if (input === 1)
            {
                return app.localize('PayFormType_ByTime');
            } else if (input === 2) {
                return app.localize('PayFormType_ByProduct');
            } else if (input === 3) {
                return app.localize('PayFormType_Fixed');
            }
            return '';
        }
        
        baseHelper.ViewFile = function(fileUrl) {
            _globalViewFileModal.open({
                path: fileUrl
            });
        }
        
        baseHelper.ShowPatientInfo = function(patient) {
            // depend on IHavePatientInfoDto
            let container = $('<div>');
            let name = $('<p>');
            let code = $('<span>');
            let phone = $('<span>');
            let email = $('<span>');
            code.add($('<i>').addClass('icon fa fa-sharp')).text(patient.patientCode);
            phone.add($('<i>').addClass('icon fa fa-phone')).text(patient.patientPhone);
            email.add($('<i>').addClass('icon fa fa-email')).text(patient.patientEmail);
            name.text(patient.patientFirstName + ' ' + patient.patientLastName);
            container.add(name);
            container.add(code);
            container.add(phone);
            container.add(email);
            return container.outerHTML();
        }
        
        baseHelper.ShowDoctorInfo = function(doctor) {
            // depend on IHaveDoctorInfoDto
        }
        
        baseHelper.NestedItemsHtml = function(items, showEditButton = false, showDeleteButton, handleIconClass='la la-bars', customLabel = 'customLabelClass', customEditClass = 'customSettingClass', customDeleteClass = 'customDeleteClass') {
            let output = '';
            $.each(items, function (index, item) {
                output += buildNestedItem(item, showEditButton, showDeleteButton, handleIconClass,customLabel, customEditClass, customDeleteClass);
            });
            
            return output;
        }
        
        baseHelper.NestedItemHtml = function(item, showSettingButton = false, handleIconClass='la la-bars', customLabel = 'customLabelClass', customEditClass = 'customSettingClass', customDeleteClass = 'customDeleteClass') {
            return buildNestedItem(item, showSettingButton, handleIconClass,customLabel, customEditClass, customDeleteClass);
        }

        baseHelper.ValidSelectors = function(){
            let res = true;
            $('body').find('select.requiredSelect2').each(function() {
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
            dropdownAutoWidth: true
        });

        $('.kt-select2-non-search').select2({
            width: '100%',
            dropdownAutoWidth: true,
            minimumResultsForSearch: -1
        });

        $('.kt-select2-multi-select').select2({
            width: '100%',
            dropdownAutoWidth: true,
            multiple: true,
            closeOnSelect: false
        });

        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        $('.datetime-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L LT'
        });

        $('.month-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'MM/YYYY'
        });

        $('.number').number(true, 0, whatDecimalSeparator(),whatThousandSeparator());
        $('.number1').number(true, 1, whatDecimalSeparator(),whatThousandSeparator());
        $('.number2').number(true, 2, whatDecimalSeparator(),whatThousandSeparator());
        $('.number3').number(true, 3, whatDecimalSeparator(),whatThousandSeparator());
        $('.numberOther').number(true, 0, '', '');

        $(".mScrollBar").mCustomScrollbar({
            theme: "minimal-dark"
        });

        $(document).on('hidden.bs.modal', '.modal', function () {
            $('.modal:visible').length && $(document.body).addClass('modal-open');
        });
        
    });
})();