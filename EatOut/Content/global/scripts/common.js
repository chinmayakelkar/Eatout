//set location
function setLocation(url) {
    window.location.href = url;
}

//open window in center of screen
function OpenWindow(query, w, h, scroll) {
    var l = (screen.width - w) / 2;
    var t = (screen.height - h) / 2;

    winprops = 'resizable=1, height=' + h + ',width=' + w + ',top=' + t + ',left=' + l + 'w';
    if (scroll) winprops += ',scrollbars=1';
    var f = window.open(query, "_blank", winprops);
}

//delete entity
function deleteEntity(data) {
    bootbox.confirm({
        title: data.title ? data.title : 'Delete Confirmation',
        message: data.message ? data.message : '',
        callback: function (result) {
            if (result) {
                $.ajax({
                    cache: false,
                    type: data.verb ? data.verb : 'POST',
                    url: data.url ? data.url : '',
                    data: data.data ? data.data : null,
                    dataType: data.type ? data.type : 'json',
                    success: function (result) {
                        if (data.callback && !data.grid) {
                            data.callback(result);  //call callback when grid refresh is not required or no more grid
                        } else {
                            if (result.success) {
                                toastr.success(result.message);
                                if (data.grid) {   //if grid passed
                                    data.grid.getDataTable().ajax.reload();

                                    if (data.callback) { //call callback after grid is refreshed
                                        data.callback();
                                    }
                                }
                            } else {
                                toastr.error(result.message);
                            }
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        toastr.error(thrownError);
                    }
                });
            }
        }
    });
}

//show modal
function showModal(data) {
    $.ajax({
        cache: false,
        type: data.verb ? data.verb : 'GET',
        url: data.url ? data.url : '',
        data: data.data ? data.data : null,
        dataType: data.type ? data.type : 'html',
        success: function (result) {
            if (data.vm) {
                data.vm.find('.modal-body').html(result);
                data.vm.modal('show');
                data.vm.data('data', data.data);

                if (data.callback) { //call back after modal is shown
                    data.callback(result);
                }
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            toastr.error(thrownError);
        }
    });
}

function displayNotification(message, messagetype, modal) {
    var htmlcode = '';
    if ((typeof message) == 'string') {
        htmlcode = '<p>' + message + '</p>';
    } else {
        for (var i = 0; i < message.length; i++) {
            htmlcode = htmlcode + '<p>' + message[i] + '</p>';
        }
    }

    var is_modal = (modal ? true : false);
    if (is_modal) {
        bootbox.alert(htmlcode);
    } else {
        if (messagetype == 'success') {
            toastr.success(htmlcode);
        } else if (messagetype == 'error') {
            toastr.error(htmlcode);
        } else {
            toastr.success(htmlcode);
        }
    }
}

//ck editor
function update_ckeditor_elemnent(element) {
    if (element) {
        CKEDITOR.instances[element].updateElement();
    } else {
        for (instance in CKEDITOR.instances) {
            CKEDITOR.instances[instance].updateElement();
        }
    }
}

$(document).ready(function () {
    //restart application
    $('#restart-application').on('click', function () {
        Metronic.blockUI({ boxed: true, message: 'Please wait...' });
    });

    //clear cache
    $('#clear-cache').on('click', function () {
        Metronic.blockUI({ boxed: true, message: 'Please wait...' });
    });

    //ckeditor issue fix for bootstrap modal
    $.fn.modal.Constructor.prototype.enforceFocus = function () {
        var $modalElement = this.$element;
        $(document).on('focusin.modal', function (e) {
            var $parent = $(e.target.parentNode);
            if ($modalElement[0] !== e.target && !$modalElement.has(e.target).length && $(e.target).parentsUntil('*[role="dialog"]').length === 0) {
                    $modalElement.focus();
            }
        });
    }
});

//ajax busy show/hide
$(document).ajaxStart(function () {
    $('#ajaxBusy').show();
}).ajaxStop(function () {
    $('#ajaxBusy').hide();
});