app.service('signalREventsService', function () {
    
    var updatedEventListener = [];
    var createdEventListener = [];
    
    // Proxy created on the fly          
    var hub = $.connection.storyHub;

    hub.client.updateStory = function (updatedStory) {
        for (var i = 0; i < updatedEventListener.length; i++) {
            updatedEventListener[i](updatedStory);
        }
    };
    
    hub.client.createStory = function (createdStory) {
        for (var i = 0; i < createdEventListener.length; i++) {
            createdEventListener[i](createdStory);
        }
    };

    // Start the connection
    $.connection.hub.start();
    
    this.registerUpdatedStoryEvent = function(callback) {
        updatedEventListener.push(callback);
    };
    
    this.registerCreatedStoryEvent = function(callback) {
        createdEventListener.push(callback);
    };
});