﻿app.controller('scrumboardViewModel',
    ['$scope', 'scrumboardService', '$location', 'notificationService',
        function($scope, scrumboardService, $location, notificationService) {

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

                scrumboardService.setStoryState(story.Id, newState.State, function (updatedStory) {
                    // replace the current story in on the scope with the updated one
                    var originalStory = _.findWhere($scope.Stories, { Id: story.Id });
                    var storyIndex = _.indexOf($scope.Stories, originalStory);
                    $scope.Stories[storyIndex] = updatedStory;
                    notificationService.notifySuccess('Moved story to "' + newState.Name + '"');
                });
            };
        }]);