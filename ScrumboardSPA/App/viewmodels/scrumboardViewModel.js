app.controller('ScrumboardViewModel', ['$scope', 'scrumboardService', function($scope, scrumboardService) {

    var states = scrumboardService.getStates();
    var stories = scrumboardService.getStories();
    
    $scope.States = states;
    $scope.Stories = stories;
}]);