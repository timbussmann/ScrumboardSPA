﻿app.controller('scrumboardViewModel',
    ['$scope', 'scrumboardService', '$location', 'notificationService',
        function($scope, scrumboardService, $location, notificationService) {

            // Proxy created on the fly          
            var hub = $.connection.storyHub;

            // Declare a function on the chat hub so the server can invoke it          
            hub.client.updateStory = function (updatedStory) {
                var originalStory = _.findWhere($scope.Stories, { Id: updatedStory.Id });
                var storyIndex = _.indexOf($scope.Stories, originalStory);
                $scope.Stories[storyIndex] = updatedStory;
                $scope.$apply();
            };

            // Start the connection
            $.connection.hub.start();


            scrumboardService.getStates(function(states) {
                $scope.States = states;
            });
            scrumboardService.getStories(function(stories) {
                $scope.Stories = stories;
            });

            $scope.ShowStoryDetail = function(storyId) {
                $location.url('/story/' + storyId);
            };

            $scope.UpdateStoryState = function (story, newState) {
                // don't update story if the state hasn't changed.
                if (story.State === newState.State) {
                    return;
                }

                scrumboardService.setStoryState(story, newState.State, function (updatedStory) {
                    // replace the current story in on the scope with the updated one
                    var originalStory = _.findWhere($scope.Stories, { Id: story.Id });
                    var storyIndex = _.indexOf($scope.Stories, originalStory);
                    $scope.Stories[storyIndex] = updatedStory;
                    notificationService.notifySuccess('Moved story to "' + newState.Name + '"');
                }, function (error, statusCode) {
                    // Conflict
                    if (statusCode = 409) {
                        notificationService.notifyError('Story ' + story.Id + ' has already been modified by another user', 'Concurrency conflict');
                    } else {
                        notificationService.notifyError('The server responded with a Statuscode ' + statusCode, 'Update failed');
                    }
                });
            };
        }]);