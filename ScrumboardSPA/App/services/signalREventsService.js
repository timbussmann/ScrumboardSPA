app.service('signalREventsService',
    ['$rootScope', function($rootScope) {

        // Proxy created on the fly          
        var hub = $.connection.storyHub;


        if (hub) {
            hub.client.updateStory = function(updatedStory) {
                // execute within apply because signalR does not run within angular scopes
                $rootScope.$apply(function() {
                    $rootScope.$broadcast('StoryChanged', updatedStory);
                });
            };

            hub.client.createStory = function(createdStory) {
                $rootScope.$broadcast('CreateSuccessful', createdStory);
            };
                    
            hub.client.deleteStory = function (deletedStoryId) {
                $rootScope.$apply(function() {
                    $rootScope.$broadcast('DeletedSuccessful', deletedStoryId);
                });
            };
            
            hub.client.updateOnlineUsers = function (usersCount) {
                $rootScope.$apply(function() {
                    $rootScope.$broadcast('OnlineUsersUpdatedSuccessful', usersCount);
                });
            };

            // Start the connection
            $.connection.hub.start();
        }
    }]);