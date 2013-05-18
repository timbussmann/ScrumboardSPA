app.service('notificationService', function () {
    
    return {
        notifySuccess: function (text, title) {
            toastr.success(text, title);
        },
        notifyError: function (text, title) {
            toastr.error(text, title);
        }
    };
});