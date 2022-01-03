(function () {
    $(function () {
        baseHelper.MiniMenu();

        let _pageLayoutService = abp.services.app.pageLayout;

        let pageBlockWrapper = $('#PageBlockWrapper');
        
        let bodyWrapper = $('#mainContentWrapper');
        let bodyContentSortable = $('#mainContentSortable');
        
        pageBlockWrapper.on('click', '.btnDeleteBlock', function () {
            let blockUniqueId = $(this).attr('blockUniqueId');
            pageBlockWrapper.find('.pageBlockDetail[blockUniqueId="' + blockUniqueId + '"]').remove();
        });

        function dropBlockOnEnter(e) {
            if (e.dropTarget !== undefined && e.dropTarget !== null && e.dropTarget.length > 0) {
                e.dropTarget.addClass('border border-success');
                e.dropTarget.removeClass('border-dashed');
            }
        }
        
        function dropBlockOnLeave(e) {
            if (e.dropTarget !== undefined && e.dropTarget !== null && e.dropTarget.length > 0) {
                e.dropTarget.removeClass('border border-success');
                e.dropTarget.addClass('border-dashed');
            }
        }
        
        function dropBlockOnDrop(e) {
            if (e.dropTarget !== undefined && e.dropTarget !== null && e.dropTarget.length > 0) {
                e.dropTarget.removeClass('border border-success');
                e.dropTarget.addClass('border-dashed');
            }

            if (e.draggable.currentTarget[0].id.indexOf('blockDraggable_') > -1) {
                let columnCount = e.draggable.currentTarget[0].id.replace('blockDraggable_', '');
                let parentUniqueId = e.dropTarget.attr('blockUniqueId');
                if (parentUniqueId === undefined)
                    parentUniqueId = '';
                let parentColumnUniqueId = e.dropTarget.attr('columnUniqueId');
                if (parentColumnUniqueId === undefined)
                    parentColumnUniqueId = '';
                $.get(abp.appPath + 'Cms/PageLayout/BlockConfigDetail?columnCount=' + columnCount + '&parentUniqueId=' + parentUniqueId + '&parentColumnUniqueId=' + parentColumnUniqueId).then(function (res) {
                    e.dropTarget.append(res);
                    baseHelper.RefreshUI(e.dropTarget);
                    InitUI();
                });

            }
        }

        bodyContentSortable.sortable({
            exclude: '.unsortable',
            handle: '.sortableHandle',
            isValidTarget: function ($item, container) {
                let containerId = container.el[0].id;
                if (containerId === undefined || containerId.length <= 0)
                    return false;
                return !$("#" + containerId).hasClass('unsortable');
            }
        });

        $(".blockDraggable").each(function () {
            $(this).kendoDraggable({
                group: "blockGroup",
                hint: function (element) {
                    return element.clone();
                }
            });
        });

        pageBlockWrapper.kendoDropTargetArea({
            group: "blockGroup",
            filter: ".blockDroppable",
            drop: dropBlockOnDrop,
            dragenter: dropBlockOnEnter,
            dragleave: dropBlockOnLeave
        });
        
        pageBlockWrapper.on('click', '.fr-view', function () {
            $(this).focus();
        });
        
        pageBlockWrapper.on('click','.btnConfigBlock', function(){
            let blockUniqueId = $(this).attr('blockUniqueId');
            if (!$(this).hasClass('active')) {
                $(this).addClass('active');
                pageBlockWrapper.find('.blockConfigGroup[blockUniqueId="' + blockUniqueId + '"]').removeClass('hidden');
            } else {
                $(this).removeClass('active');
                pageBlockWrapper.find('.blockConfigGroup[blockUniqueId="' + blockUniqueId + '"]').addClass('hidden');
            }
        });
        
        pageBlockWrapper.on('change','.blockName', function(){
            let blockUniqueId = $(this).attr('blockUniqueId');
            pageBlockWrapper.find('.blockLabel[blockUniqueId="' + blockUniqueId + '"]').html($(this).val());
        });
        
        function InitUI() {
            $('.blockDroppable[initDroppable="false"]').each(function () {
                $(this).kendoDropTargetArea({
                    group: "blockGroup",
                    filter: ".blockDroppable",
                    drop: dropBlockOnDrop,
                    dragenter: dropBlockOnEnter,
                    dragleave: dropBlockOnLeave
                });
                $(this).attr('initDroppable', true);
            });
        }
        
        InitUI();
        
        function BlockDetails(wrapper, order) {
            let res= [];
            if (wrapper) {
                wrapper.find('.pageBlockDetail').each(function () {

                    let blockUniqueId = $(this).attr('blockUniqueId');
                    let col1Id = $('.col1Id[blockUniqueId="' + blockUniqueId + '"]').val();
                    if (col1Id === undefined)
                        col1Id = '';
                    let col1UniqueId = $('.col1UniqueId[blockUniqueId="' + blockUniqueId + '"]').val();
                    if (col1UniqueId === undefined)
                        col1UniqueId = '';
                    let col1Class = $('.col1Class[blockUniqueId="' + blockUniqueId + '"]').val();
                    if (col1Class === undefined)
                        col1Class = '';

                    let col2Id = $('.col2Id[blockUniqueId="' + blockUniqueId + '"]').val();
                    if (col2Id === undefined)
                        col2Id = '';
                    let col2UniqueId = $('.col2UniqueId[blockUniqueId="' + blockUniqueId + '"]').val();
                    if (col2UniqueId === undefined)
                        col2UniqueId = '';
                    let col2Class = $('.col2Class[blockUniqueId="' + blockUniqueId + '"]').val();
                    if (col2Class === undefined)
                        col2Class = '';

                    let col3Id = $('.col3Id[blockUniqueId="' + blockUniqueId + '"]').val();
                    if (col3Id === undefined)
                        col3Id = '';
                    let col3UniqueId = $('.col3UniqueId[blockUniqueId="' + blockUniqueId + '"]').val();
                    if (col3UniqueId === undefined)
                        col3UniqueId = '';
                    let col3Class = $('.col3Class[blockUniqueId="' + blockUniqueId + '"]').val();
                    if (col3Class === undefined)
                        col3Class = '';

                    let col4Id = $('.col4Id[blockUniqueId="' + blockUniqueId + '"]').val();
                    if (col4Id === undefined)
                        col4Id = '';
                    let col4UniqueId = $('.col4UniqueId[blockUniqueId="' + blockUniqueId + '"]').val();
                    if (col4UniqueId === undefined)
                        col4UniqueId = '';
                    let col4Class = $('.col4Class[blockUniqueId="' + blockUniqueId + '"]').val();
                    if (col4Class === undefined)
                        col4Class = '';

                    let wrapInRow = $('.wrapInRow[blockUniqueId="' + blockUniqueId + '"]').prop('checked');
                    if (wrapInRow === undefined)
                        wrapInRow = false;
                    
                    let obj = {
                        uniqueId: blockUniqueId,
                        name: $('.blockName[blockUniqueId="' + blockUniqueId + '"]').val(),
                        columnCount: $(this).attr('blockColumnCount'),
                        wrapInRow: wrapInRow,
                        order: order,
                        
                        parentBlockUniqueId: $(this).attr('parentBlockUniqueId'),
                        parentColumnUniqueId: $(this).attr('parentColumnUniqueId'),

                        col1Id: col1Id,
                        col1UniqueId: col1UniqueId,
                        col1Class: col1Class,

                        col2Id: col2Id,
                        col2UniqueId: col2UniqueId,
                        col2Class: col2Class,

                        col3Id: col3Id,
                        col3UniqueId: col3UniqueId,
                        col3Class: col3Class,

                        col4Id: col4Id,
                        col4UniqueId: col4UniqueId,
                        col4Class: col4Class
                    };

                    if ($(this).attr('detailId') !== undefined)
                        obj.id = $(this).attr('detailId');
                    res.push(obj);
                    order++;
                });
            }
            return res;
        }
        
        function GetBlockDetail() {
            let res = [];
            let order = 0;
            $.merge(res,BlockDetails(bodyWrapper, order));
            return res;
        }
        
        let saveButton = $('#SaveButton');

        if (saveButton) {
            saveButton.on('click', function () {
                let pageLayout = {
                    id: $('#PageLayout_Id').val(),
                    blocks: GetBlockDetail(),
                }
                abp.ui.setBusy(pageBlockWrapper);
                _pageLayoutService.updateConfig(
                    pageLayout
                ).done(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                }).always(function () {
                    abp.ui.clearBusy(pageBlockWrapper);
                });
            });
        }

        let btnBackToList = $('#BackToListingPageButton');

        if (btnBackToList) {
            btnBackToList.on('click', function () {
                window.location = "/Cms/PageLayout/";
            })
        }
    });
})();