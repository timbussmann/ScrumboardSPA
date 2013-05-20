app.service('signalREventsService', function () {
    
    var eventListener = [];
    
    // Proxy created on the fly          
    var hub = $.connection.storyHub;

    hub.client.updateStory = function (updatedStory) {
        for (var i = 0; i < eventListener.length; i++) {
            eventListener[i](updatedStory);
        }
    };

    // Start the connection
    $.connection.hub.start();
    
    this.registerUpdatedStoryEvent = function(callback) {
        eventListener.push(callback);
    };
});