$(function () {

    let fileManagerSelector = $('#fileManagerSelector');

    fileManagerSelector.kendoFileManager({
        height: 750,
        previewPane: {
            singleFileTemplate: kendo.template($("#preview-template").html())
        },
        dataSource: {
            schema: kendo.data.schemas.filemanager,
            transport: {
                read: {
                    url: "/Base/FileManagerData/Read",
                    method: "POST"
                },
                create: {
                    url: "/Base/FileManagerData/Create",
                    method: "POST"
                },
                update: {
                    url: "/Base/FileManagerData/Update",
                    method: "POST"
                },
                destroy: {
                    url: "/Base/FileManagerData/Destroy",
                    method: "POST"
                }
            }
        },
        uploadUrl: "/Base/FileManagerData/Upload",
        toolbar: {
            items: [
                { name: "createFolder" },
                { name: "upload" },
                { name: "sortField" },
                { name: "changeView" },
                { name: "spacer" },
                { name: "details" },
                { name: "search" }
            ]
        },
        contextMenu: {
            items: [
                { name: "rename" },
                { name: "delete" }
            ]
        },
        draggable: false,
        resizable: true,
        select: FileManagerOnSelect
        
    });
    function FileManagerOnSelect(e) {
        console.log(e);
    }
});
