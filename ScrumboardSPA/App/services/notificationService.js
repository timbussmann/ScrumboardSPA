﻿app.service('notificationService', function () {

    this.notifySuccess = function(text, title) {
        toastr.success(text, title);
    };

    this.notifyError = function(text, title) {
        toastr.error(text, title);
    };

    this.notifyWarning = function(text, title) {
        toastr.warning(text, title);
    };
});