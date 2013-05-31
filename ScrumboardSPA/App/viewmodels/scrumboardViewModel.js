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
                // receive the story from the scope. The parameter is a json deserialized story
                // which will not trigger repaint of the original story
                //var originalStory = _.findWhere($scope.Stories, { Id: story.Id });
                //originalStory.State = newState.State;

                scrumboardService.setStoryState(story, newState.State);
            };

            $scope.$on('CreateSuccessful', function(event, createdStory) {
                scrumboardService.getStory(createdStory.Id, function (receivedStory) {
                    
                    if (_.findWhere($scope.Stories, { Id: receivedStory.Id }) === undefined) {
                        $scope.Stories.push(receivedStory);
                    }                 

                    notificationService.notifySuccess(createdStory.Title + ' - <a href="/story/' + createdStory.Id + '">[click to see story]</a>', 'New Story created');
                });
            });

            $scope.$on('DeletedSuccessful', function(event, deletedStoryId) {

                var story = _.findWhere($scope.Stories, { Id: deletedStoryId });
                if (story !== undefined) {
                    notificationService.notifySuccess('Story ' + story.Title + ' deleted');
                    var storyIndex = _.indexOf($scope.Stories, story);
                    $scope.Stories.splice(storyIndex, 1);
                }
            });

            $scope.$on('StoryChanged', function(event, changedStory) {
                var originalStory = _.findWhere($scope.Stories, { Id: changedStory.Id });
                
                // only update story if we have an older version
                if (originalStory.Etag !== changedStory.Etag) {
                    var storyIndex = _.indexOf($scope.Stories, originalStory);
                    $scope.Stories[storyIndex] = changedStory;
                    notificationService.notifySuccess('Updated story #' + changedStory.Id);
                }
            });

            $scope.$on('UpdateConflicted', function(event, conflict) {
                notificationService.notifyError('Story ' + conflict.Requested.Id + ' has already been modified by another user', 'Concurrency conflict');

                var conflictNr = conflictService.addConflict(conflict.Original, conflict.Requested);
                $location.url('/conflict/' + conflictNr);
            });

            $scope.$on('UpdateFailed', function(event, statusCode) {
                notificationService.notifyError('The server responded with a Statuscode ' + statusCode, 'Update failed');
            });

            $scope.$on('UsingCachedData', function(event) {
                $scope.Offline = true;
            });
        }]);