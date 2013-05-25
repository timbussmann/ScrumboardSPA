app.controller('scrumboardViewModel',
    ['$scope', 'scrumboardService', '$location', 'notificationService', 'signalREventsService', 'conflictService',
        function($scope, scrumboardService, $location, notificationService, signalREventsService, conflictService) {

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

                // we assume the update will be successful, so we already update
                // the story for the ui. The story will be updated again after
                // the success or error events have been received.
                story.State = newState.State;

                scrumboardService.setStoryState(story, newState.State);
            };

            $scope.$on('CreateSuccessful', function(event, createdStory) {
                scrumboardService.getStory(createdStory.Id, function (receivedStory) {
                    
                    if (_.findWhere($scope.Stories, { Id: receivedStory.Id }) == undefined) {
                        $scope.Stories.push(receivedStory);
                    }                 

                    notificationService.notifySuccess(createdStory.Title + ' - <a href="/story/' + createdStory.Id + '">[click to see story]</a>', 'New Story created');
                });
            });
            
            $scope.$on('UpdateSuccessful', function(event, updatedStory) {
                scrumboardService.getStory(updatedStory.Id, function (receivedStory) {
                    var originalStory = _.findWhere($scope.Stories, { Id: receivedStory.Id });
                    var storyIndex = _.indexOf($scope.Stories, originalStory);
                    $scope.Stories[storyIndex] = receivedStory;
                    notificationService.notifySuccess('Updated story #"' + receivedStory.Id + '"');
                });
            });

            $scope.$on('UpdateConflicted', function(event, conflict) {
                notificationService.notifyError('Story ' + conflict.Requested.Id + ' has already been modified by another user', 'Concurrency conflict');

                var conflictNr = conflictService.addConflict(conflict.Original, conflict.Requested);
                $location.url('/conflict/' + conflictNr);
            });

            $scope.$on('UpdateFailed', function(event, statusCode) {
                notificationService.notifyError('The server responded with a Statuscode ' + statusCode, 'Update failed');
            });
        }]);