app.controller('scrumboardViewModel', ['$scope', 'scrumboardService', function($scope, scrumboardService) {

    scrumboardService.getStates(function (states) {
        $scope.States = states;
    });
    scrumboardService.getStories(function(stories) {
        $scope.Stories = stories;
    });
}]);