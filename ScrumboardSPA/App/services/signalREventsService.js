app.service('signalREventsService',
    ['$rootScope', function ($rootScope) {
    
    // Proxy created on the fly          
    var hub = $.connection.storyHub;

    hub.client.updateStory = function (updatedStory) {
        $rootScope.$broadcast('StoryChanged', updatedStory);
    };
    
    hub.client.createStory = function (createdStory) {
        $rootScope.$broadcast('CreateSuccessful', createdStory);
    };

    // Start the connection
    $.connection.hub.start();
}]);