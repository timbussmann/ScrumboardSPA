app.controller('ScrumboardViewModel', ['$scope', 'scrumboardService', function($scope, scrumboardService) {

    var model = {};
    
    scrumboardService.getStates(function(states) {
        model.States = states;
    });
    scrumboardService.getStories(function(stories) {
        model.Stories = stories;
    });

    $scope.model = model;
}]);