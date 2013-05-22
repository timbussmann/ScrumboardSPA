app.controller('resolveConflictViewModel',
    ['$scope', '$routeParams', 'conflictService', '$location',
        function($scope, $routeParams, conflictService, $location) {

            var conflict = conflictService.getConflict($routeParams.conflictNumber);

            $scope.Conflict = conflict;
        }]);