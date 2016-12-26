var Packages = function () {

    handlePackages = function () {
        var grid = new Datatable();

        grid.init({
            src: $("#datatable_ajax"),
            onSuccess: function (grid) { },// execute some code afer table records loaded
            onError: function (grid) { },// execute some code on network or other general error  
            loadingMessage: 'Loading...',
            dataTable: {
                bStateSave: true, // save datatable state(pagination, sort, etc) in cookie. 
                lengthMenu: [[10, 20, 50, 100, 150, -1], [10, 20, 50, 100, 150, "All"]], // change per page values here
                pageLength: 10, // default record count per page
                ajax: { "url": "/package/list", },
                columns: [
                    {
                        data: "Name",
                        orderable: true
                    },
                    {
                        data: "PackageTypeName",
                        orderable: true
                    },
                    {
                        data: "ExpiresOn",
                        orderable: true,
                        render: function (data, type, row) {
                            return moment(row.Date).utc().format("MM/DD/YYYY");
                        }
                    },
                    {
                        data: "OriginalPrice",
                        orderable: true
                    },
                    {
                        data: "OfferedPrice",
                        orderable: true
                    },
                    {
                        data: "Active",
                        render: function (data, type, row) {
                            if (type === 'display') {
                                return data === true ? '<span class="label label-sm label-success">Active</span>' : '<span class="label label-sm label-info">Inactive</span>';
                            }
                            return data;
                        }
                    },
                    {
                        data: "Id",
                        orderable: false,
                        render: function (data, type, row) {
                            if (type === 'display') {
                                var actionHtml = '';

                                actionHtml = '<a title="Edit" class="btn btn-xs default" href="/package/edit/' + data + '"><i class="fa fa-edit"></i></a> \
                                        <a title="Delete" class="btn btn-xs red-haze lnk-delete" href="/package/delete/' + data + '"><i class="fa fa-trash-o"></i></a>';

                                return actionHtml;
                            }
                            return data;
                        }
                    }
                ],
                order: [[0, "asc"]]
            }
        });

        //search
        $('#btn-search').on('click', function (event) {
            event.preventDefault();
            var frm_array = $('#frm_package').serializeArray();
            $.each(frm_array, function (i, fd) { grid.setAjaxParam(fd.name, fd.value); });
            grid.getDataTable().ajax.reload();
        });
    }

    return {
        init: function () {
            handlePackages();
        }
    };

}(jQuery);