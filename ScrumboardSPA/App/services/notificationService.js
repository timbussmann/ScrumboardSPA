app.service('notificationService', function () {
    
    return {
        notifySuccess: function (text, title) {
            toastr.success(text, title);
        }
    };
});