app.controller('scrumboardViewModel',
    ['$scope', 'scrumboardService', '$location',
        function($scope, scrumboardService, $location) {

            scrumboardService.getStates(function(states) {
                $scope.States = states;
            });
            scrumboardService.getStories(function(stories) {
                $scope.Stories = stories;
            });

            $scope.ShowStoryDetail = function(storyId) {
                $location.url('/story/' + storyId);
            };

            $scope.UpdateStoryState = function(story, newState) {
                scrumboardService.setStoryState(story.Id, newState.State);
            };
        }]);