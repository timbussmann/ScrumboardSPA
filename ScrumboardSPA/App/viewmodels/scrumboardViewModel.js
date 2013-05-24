app.controller('scrumboardViewModel',
    ['$scope', 'scrumboardService', '$location', 'notificationService', 'conflictService',
        function($scope, scrumboardService, $location, notificationService, conflictService) {

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

            $scope.$on('UpdateSuccessful', function(event, updatedStory) {
                // replace the current story in on the scope with the updated one
                var originalStory = _.findWhere($scope.Stories, { Id: updatedStory.Id });
                var storyIndex = _.indexOf($scope.Stories, originalStory);
                $scope.Stories[storyIndex] = updatedStory;
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