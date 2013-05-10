app.controller('storyDetailViewModel', ['$scope', 'scrumboardService', '$routeParams',
    function($scope, scrumboardService, $routeParams) {
        var storyId = $routeParams.storyId;

        scrumboardService.getStory(storyId, function(story) {
            $scope.Story = story;
        });

    }]);