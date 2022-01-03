(function () {
    $(function () {
        baseHelper.MiniMenu();

        let pageId = $('#PageId').val();
        let _PagesService = abp.services.app.page;

        let pageWidgetWrapper = $('#PageWidgetWrapper');

        function dropWidgetOnEnter(e) {
            if (e.dropTarget !== undefined && e.dropTarget !== null && e.dropTarget.length > 0) {
                e.dropTarget.addClass('border border-success');
                e.dropTarget.removeClass('border-dashed');
            }
        }

        function dropWidgetOnLeave(e) {
            if (e.dropTarget !== undefined && e.dropTarget !== null && e.dropTarget.length > 0) {
                e.dropTarget.removeClass('border border-success');
                e.dropTarget.addClass('border-dashed');
            }
        }

        function dropWidgetOnDrop(e) {
            if (e.dropTarget !== undefined && e.dropTarget !== null && e.dropTarget.length > 0) {
                e.dropTarget.removeClass('border border-success');
                e.dropTarget.addClass('border-dashed');
            }

            if (e.dropTarget !== undefined && e.dropTarget !== null && e.dropTarget.length > 0) {
                e.dropTarget.removeClass('border-success');
            }

            if (e.draggable.currentTarget[0].id.indexOf('widgetDraggable_') > -1) {
                let widgetId = e.draggable.currentTarget[0].id.replace('widgetDraggable_', '');
                let blockColumnId = e.dropTarget.attr('columnUniqueId');
                
                $.get(abp.appPath + 'Cms/Page/WidgetConfigDetail?widgetId=' + widgetId + '&blockColumnId=' + blockColumnId).then(function (res) {
                    e.dropTarget.append(res);
                    baseHelper.RefreshUI(e.dropTarget);
                    InitSelector();
                });
            }
        }
        
        $(".widgetDraggable").each(function () {
            $(this).kendoDraggable({
                group: "widgetGroup",
                hint: function (element) {
                    return element.clone();
                }
            });
        });

        $('.widgetSortable').each(function () {
            $(this).kendoSortable({
                container: pageWidgetWrapper,
                filter: ".pageWidgetDetail",
                handler: ".pageWidgetDetailHeader",
                ignore: ".fr-view,.inputGroup,textarea"
            });
        });
        
        $('.widgetDroppable[initDroppable="false"]').each(function () {
            $(this).kendoDropTargetArea({
                group: "widgetGroup",
                filter: ".widgetDroppable",
                drop: dropWidgetOnDrop,
                dragenter: dropWidgetOnEnter,
                dragleave: dropWidgetOnLeave
            });
            $(this).attr('initDroppable', true);
        });

        // pageWidgetWrapper.on('click', '.fr-view', function () {
        //     $(this).focus();
        // });

        function InitSelector() {
            if (pageWidgetWrapper) {
                pageWidgetWrapper.on('click', '.btnDeleteWidget', function () {
                    let pageWidgetUniqueId = $(this).attr('pageWidgetUniqueId');
                    pageWidgetWrapper.find('.pageWidgetDetail[pageWidgetUniqueId="' + pageWidgetUniqueId + '"]').remove();
                });
                pageWidgetWrapper.find('.pageWidgetDetailWrapper').each(function () {
                    let rowId = $(this).attr('rowId');

                    let serviceTypeId = pageWidgetWrapper.find('.serviceTypeId[rowId="' + rowId + '"][initSelector="false"]');
                    let serviceCategoryId = pageWidgetWrapper.find('.serviceCategoryId[rowId="' + rowId + '"][initSelector="false"]');
                    let serviceArticleId = pageWidgetWrapper.find('.serviceArticleId[rowId="' + rowId + '"][initSelector="false"]');
                    let serviceVendorId = pageWidgetWrapper.find('.serviceVendorId[rowId="' + rowId + '"][initSelector="false"]');
                    let serviceBrandId = pageWidgetWrapper.find('.serviceBrandId[rowId="' + rowId + '"][initSelector="false"]');
                    let serviceId = pageWidgetWrapper.find('.serviceId[rowId="' + rowId + '"][initSelector="false"]');
                    let servicePropertyGroupId = pageWidgetWrapper.find('.servicePropertyGroupId[rowId="' + rowId + '"][initSelector="false"]');
                    let reviewPostId = pageWidgetWrapper.find('.reviewPostId[rowId="' + rowId + '"][initSelector="false"]');
                    let imageBlockGroupId = pageWidgetWrapper.find('.imageBlockGroupId[rowId="' + rowId + '"][initSelector="false"]');
                    let menuGroupId = pageWidgetWrapper.find('.menuGroupId[rowId="' + rowId + '"][initSelector="false"]');

                    if (serviceTypeId) {
                        baseHelper.SimpleRequiredSelector(serviceTypeId, app.localize('PleaseSelect'), "/ServiceWorld/GetPagedServiceTypes");
                        serviceTypeId.removeAttr('initSelector');
                        serviceTypeId.on('change', function () {
                            if (serviceCategoryId)
                                serviceCategoryId.val(null).trigger('change');
                            if (serviceArticleId)
                                serviceArticleId.val(null).trigger('change');
                            if (reviewPostId)
                                reviewPostId.val(null).trigger('change');
                        });
                    }

                    if (serviceCategoryId) {
                        serviceCategoryId.select2({
                            placeholder: app.localize('NoneSelect'),
                            allowClear: true,
                            width: '100%',
                            language: baseHelper.Select2Language(),
                            ajax: {
                                url: abp.appPath + "api/services/app/ServiceWorld/GetPagedServiceCategories",
                                dataType: 'json',
                                delay: 50,
                                data: function (params) {
                                    return {
                                        filter: params.term,
                                        skipCount: ((params.page || 1) - 1) * 10,
                                        serviceTypeId: serviceTypeId !== undefined ? serviceTypeId.val() : null
                                    };
                                },
                                processResults: function (data, params) {
                                    params.page = params.page || 1;

                                    let res = $.map(data.result.items, function (item) {
                                        return {
                                            text: item.name,
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
                        serviceCategoryId.removeAttr('initSelector');
                        serviceCategoryId.on('change', function () {
                            if (serviceId)
                                serviceId.val(null).trigger('change');
                            if (reviewPostId)
                                reviewPostId.val(null).trigger('change');
                        });
                    }

                    if (serviceArticleId) {
                        serviceArticleId.select2({
                            placeholder: app.localize('NoneSelect'),
                            allowClear: true,
                            width: '100%',
                            language: baseHelper.Select2Language(),
                            ajax: {
                                url: abp.appPath + "api/services/app/ServiceWorld/GetPagedServiceArticles",
                                dataType: 'json',
                                delay: 50,
                                data: function (params) {
                                    return {
                                        filter: params.term,
                                        skipCount: ((params.page || 1) - 1) * 10,
                                        serviceTypeId: serviceTypeId !== undefined ? serviceTypeId.val() : null
                                    };
                                },
                                processResults: function (data, params) {
                                    params.page = params.page || 1;

                                    let res = $.map(data.result.items, function (item) {
                                        return {
                                            text: item.name,
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
                        serviceArticleId.removeAttr('initSelector');
                        serviceArticleId.on('change', function () {
                            if (serviceId)
                                serviceId.val(null).trigger('change');
                            if (reviewPostId)
                                reviewPostId.val(null).trigger('change');
                        });
                    }

                    if (serviceVendorId) {
                        serviceVendorId.select2({
                            placeholder: app.localize('NoneSelect'),
                            allowClear: true,
                            width: '100%',
                            language: baseHelper.Select2Language(),
                            ajax: {
                                url: abp.appPath + "api/services/app/ServiceWorld/GetPagedServiceVendors",
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
                                            text: item.name,
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
                        serviceVendorId.removeAttr('initSelector');
                        serviceVendorId.on('change', function () {
                            if (serviceId)
                                serviceId.val(null).trigger('change');
                            if (reviewPostId)
                                reviewPostId.val(null).trigger('change');
                        });
                    }

                    if (serviceBrandId) {
                        serviceBrandId.select2({
                            placeholder: app.localize('NoneSelect'),
                            allowClear: true,
                            width: '100%',
                            language: baseHelper.Select2Language(),
                            ajax: {
                                url: abp.appPath + "api/services/app/ServiceWorld/GetPagedServiceBrands",
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
                                            text: item.name,
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
                        serviceBrandId.removeAttr('initSelector');
                        serviceBrandId.on('change', function () {
                            if (serviceId)
                                serviceId.val(null).trigger('change');
                            if (reviewPostId)
                                reviewPostId.val(null).trigger('change');
                        });
                    }
                    
                    if (serviceId) {
                        serviceId.select2({
                            placeholder: app.localize('NoneSelect'),
                            allowClear: true,
                            width: '100%',
                            language: baseHelper.Select2Language(),
                            ajax: {
                                url: abp.appPath + "api/services/app/ServiceWorld/GetPagedServices",
                                dataType: 'json',
                                delay: 50,
                                data: function (params) {
                                    return {
                                        filter: params.term,
                                        skipCount: ((params.page || 1) - 1) * 10,
                                        serviceTypeId: serviceTypeId !== undefined ? serviceTypeId.val() : null,
                                        serviceCategoryId: serviceCategoryId !== undefined ? serviceCategoryId.val() : null,
                                        serviceArticleId: serviceArticleId !== undefined ? serviceArticleId.val() : null
                                    };
                                },
                                processResults: function (data, params) {
                                    params.page = params.page || 1;

                                    let res = $.map(data.result.items, function (item) {
                                        return {
                                            text: item.name,
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
                        serviceId.removeAttr('initSelector');
                        serviceId.on('change', function () {
                            if (reviewPostId)
                                reviewPostId.val(null).trigger('change');
                        });
                    }

                    if (servicePropertyGroupId) {
                        baseHelper.SimpleRequiredSelector(servicePropertyGroupId, app.localize('PleaseSelect'), "/ServiceWorld/GetPagedServicePropertyGroups");
                        servicePropertyGroupId.removeAttr('initSelector');
                    }
                    
                    if (reviewPostId) {
                        reviewPostId.select2({
                            placeholder: app.localize('NoneSelect'),
                            allowClear: true,
                            width: '100%',
                            language: baseHelper.Select2Language(),
                            ajax: {
                                url: abp.appPath + "api/services/app/ServiceWorld/GetPagedReviewPosts",
                                dataType: 'json',
                                delay: 50,
                                data: function (params) {
                                    return {
                                        filter: params.term,
                                        skipCount: ((params.page || 1) - 1) * 10,
                                        serviceTypeId: serviceTypeId !== undefined ? serviceTypeId.val() : null,
                                        serviceCategoryId: serviceCategoryId !== undefined ? serviceCategoryId.val() : null,
                                        serviceArticleId: serviceArticleId !== undefined ? serviceArticleId.val() : null,
                                        serviceId: serviceId !== undefined ? serviceId.val() : null,
                                        reviewPostStatus: 3
                                    };
                                },
                                processResults: function (data, params) {
                                    params.page = params.page || 1;

                                    let res = $.map(data.result.items, function (item) {
                                        return {
                                            text: item.title,
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
                        reviewPostId.removeAttr('initSelector');
                    }

                    if (imageBlockGroupId) {
                        baseHelper.SimpleRequiredSelector(imageBlockGroupId, app.localize('PleaseSelect'), "/Cms/GetPagedImageBlockGroups");
                        imageBlockGroupId.removeAttr('initSelector');
                    }

                    if (menuGroupId) {
                        baseHelper.SimpleRequiredSelector(menuGroupId, app.localize('PleaseSelect'), "/Cms/GetPagedMenuGroups");
                        menuGroupId.removeAttr('initSelector');
                    }
                });
            }
        }
        
        InitSelector();
        
        function GetWidgetDetail() {
            let res = [];
            if (pageWidgetWrapper) {
                let order = 1;
                pageWidgetWrapper.find('.pageWidgetDetail').each(function () {
                    let obj = {
                        widgetId: $(this).attr('widgetId'),
                        pageBlockColumnId: $(this).attr('blockColumnId'),
                        order: order,
                        details: []
                    };
                    let pageWidgetDetailConfigs = $(this).find('.pageWidgetDetailConfig');
                    if (pageWidgetDetailConfigs !== undefined && pageWidgetDetailConfigs.length > 0) {
                        pageWidgetDetailConfigs.each(function () {
                            let detailConfig = {};
                            let rowId = $(this).attr('rowId');

                            let serviceTypeId = $(this).find('.serviceTypeId[rowId="' + rowId + '"]');
                            if (serviceTypeId && serviceTypeId.val() !== undefined)
                                detailConfig.serviceTypeId = serviceTypeId.val();

                            let serviceCategoryId = $(this).find('.serviceCategoryId[rowId="' + rowId + '"]');
                            if (serviceCategoryId && serviceCategoryId.val() !== undefined)
                                detailConfig.serviceCategoryId = serviceCategoryId.val();

                            let serviceArticleId = $(this).find('.serviceArticleId[rowId="' + rowId + '"]');
                            if (serviceArticleId && serviceArticleId.val() !== undefined)
                                detailConfig.serviceArticleId = serviceArticleId.val();

                            let serviceVendorId = $(this).find('.serviceVendorId[rowId="' + rowId + '"]');
                            if (serviceVendorId && serviceVendorId.val() !== undefined)
                                detailConfig.serviceVendorId = serviceVendorId.val();

                            let serviceBrandId = $(this).find('.serviceBrandId[rowId="' + rowId + '"]');
                            if (serviceBrandId && serviceBrandId.val() !== undefined)
                                detailConfig.serviceBrandId = serviceBrandId.val();
                            
                            let serviceId = $(this).find('.serviceId[rowId="' + rowId + '"]');
                            if (serviceId && serviceId.val() !== undefined)
                                detailConfig.serviceId = serviceId.val();

                            let servicePropertyGroupId = $(this).find('.servicePropertyGroupId[rowId="' + rowId + '"]');
                            if (servicePropertyGroupId && servicePropertyGroupId.val() !== undefined)
                                detailConfig.servicePropertyGroupId = servicePropertyGroupId.val();

                            let reviewPostId = $(this).find('.reviewPostId[rowId="' + rowId + '"]');
                            if (reviewPostId && reviewPostId.val() !== undefined)
                                detailConfig.reviewPostId = reviewPostId.val();

                            let imageBlockGroupId = $(this).find('.imageBlockGroupId[rowId="' + rowId + '"]');
                            if (imageBlockGroupId && imageBlockGroupId.val() !== undefined)
                                detailConfig.imageBlockGroupId = imageBlockGroupId.val();

                            let menuGroupId = $(this).find('.menuGroupId[rowId="' + rowId + '"]');
                            if (menuGroupId && menuGroupId.val() !== undefined)
                                detailConfig.menuGroupId = menuGroupId.val();

                            let customContent = $(this).find('.customContent[rowId="' + rowId + '"]');
                            if (customContent && customContent.val() !== undefined)
                                detailConfig.customContent = customContent.val();
                            obj.details.push(detailConfig);
                        })
                    }
                    res.push(obj);
                    order++;
                });
            }
            return res;
        }

        let saveButton = $('#SaveButton');

        if (saveButton) {
            saveButton.on('click', function () {
                let Page = {
                    id: pageId,
                    widgets: GetWidgetDetail()
                }
                abp.ui.setBusy(pageWidgetWrapper);
                _PagesService.updatePageDetails(
                    Page
                ).done(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                }).always(function () {
                    abp.ui.clearBusy(pageWidgetWrapper);
                });
            });
        }

        let btnBackToList = $('#BackToListingPageButton');

        if (btnBackToList) {
            btnBackToList.on('click', function () {
                window.location = "/Cms/Page/";
            })
        }
    });
})();