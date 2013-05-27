app.controller('storyDetailViewModel',
    ['$scope', 'scrumboardService', '$routeParams', '$location', 'scrumboardService',
        function($scope, scrumboardService, $routeParams, $location, $scrumboardService) {
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
            
            $scope.DeleteStory = function() {
                $scrumboardService.deleteStory($scope.Story);
                $location.url('/scrumboard');
            };
        }]);