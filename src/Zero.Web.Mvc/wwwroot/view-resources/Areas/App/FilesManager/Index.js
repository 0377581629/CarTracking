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
                    url: "/Base/FilesManager/Read",
                    method: "POST"
                },
                create: {
                    url: "/Base/FilesManager/Create",
                    method: "POST"
                },
                update: {
                    url: "/Base/FilesManager/Update",
                    method: "POST"
                },
                destroy: {
                    url: "/Base/FilesManager/Destroy",
                    method: "POST"
                }
            }
        },
        uploadUrl: "/Base/FilesManager/Upload",
        toolbar: {
            items: [
                { name: "createFolder" },
                { name: "upload" },
                { name: "sortField" },
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
