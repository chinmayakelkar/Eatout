var MessageTemplateEdit = function () {

    handleAjax = function () {

        //copy message template
        $('#message-template-copy').on('click', function (e) {
            e.preventDefault(); var form = this.closest('form');
            bootbox.confirm({
                title: 'Copy Confirmation',
                message: 'Are you sure you want to copy this message template?',
                callback: function (result) {
                    if (result) {
                        $("<input name='message-template-copy' type='text' value='message-template-copy' />").appendTo(form);
                        form.submit();
                    }
                }
            });
        });
    }

    return {
        init: function () {
            CKEDITOR.replace('Body');
            handleAjax();
        }
    };

}();