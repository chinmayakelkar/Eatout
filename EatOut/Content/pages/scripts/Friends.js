var Friends = function () {

    handleFriends = function () {
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
                ajax: {
                    "url": "/home/UsersList",
                },
                columns: [
                    {
                        data: "FirstName",
                        orderable: true
                    },

                     {
                         data: "LastName",
                         orderable: true
                     },
                     {
                         data: "Email",
                         orderable: true
                     },

                      {
                          data: "isFriend",
                          render: function (data, type, row) {
                              if (type === 'display') {
                                  console.log(data);
                                  return data === true ? '<span class="label label-sm label-success">Friend</span>' : '<span class="label label-sm label-info">Not a Friend</span>';
                              }
                              return data;
                          }
                      },
                    {
                        data: "UserID",
                        orderable: false,
                        render: function (data, type, row) {
                            if (type === 'display') {
                                var actionHtml = '';

                                actionHtml = '<a title="Friend" class="btn btn-xs default" href="/home/AddFriend/' + data + '"><i class="fa fa-edit"></i></a>';

                                return actionHtml;
                            }
                            return data;
                        }
                    }
                ],
                order: [[0, "asc"]],
                bFilter: true
            }
        });
        //search
        $('#btn-search').on('click', function (event) {
            event.preventDefault();
            var frm_array = $('#frm_rest').serializeArray();
            $.each(frm_array, function (i, fd) { grid.setAjaxParam(fd.name, fd.value); });
            grid.getDataTable().ajax.reload();
        });

    }

    return {
        init: function () {
            handleFriends();
        }
    };

}(jQuery);