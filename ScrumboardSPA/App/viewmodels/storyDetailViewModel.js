app.controller('storyDetailViewModel',
    ['$scope', 'scrumboardService', '$routeParams', '$location',
        function($scope, scrumboardService, $routeParams, $location) {
            var storyId = $routeParams.storyId;

            scrumboardService.getStory(storyId, function(story) {
                $scope.Story = story;
            });

            scrumboardService.getStates(function(states) {
                $scope.States = states;
            });

            $scope.CloseDetailView = function() {
                $location.url('/scrumboard');
            };

        }]);