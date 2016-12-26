var MessageTemplates = function () {

    handleMessageTemplates = function () {

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
                ajax: { "url": "/messagetemplate/list", },
                columns: [
                    {
                        data: "Name",
                        orderable: true
                    },
                    {
                        data: "Subject",
                        orderable: false
                    },
                    {
                        data: "IsActive",
                        orderable: true,
                        render: function (data, type, row) {
                            if (type === 'display') {
                                return data === true ? 'Active' : 'Inactive';
                            }
                            return data;
                        }
                    },
                    {
                        data: "Id",
                        orderable: false,
                        render: function (data, type, row) {
                            if (type === 'display') {
                                return '<a class="btn btn-xs default" href="/messagetemplate/edit/' + row.Id + '"><i class="fa fa-edit"></i> Edit</a>\
                                        <a class="btn btn-xs red-haze lnk-delete" href="#" data-id="' + row.Id + '"><i class="fa fa-trash-o"></i> Delete</a>';;
                            }
                            return data;
                        }
                    }
                ],
                order: [[0, "asc"]]
            }
        });

        //delete
        $('#datatable_ajax').on('click', '.lnk-delete', function (event) {
            event.preventDefault();
            deleteEntity({
                data: { id: $(this).data('id') },
                message: 'Are you sure you want to delete this message template?',
                url: '/messagetemplate/delete',
                grid: grid
            });
        });
    }

    return {
        init: function () {
            handleMessageTemplates();
        }
    };

}(jQuery);