app.controller('resolveConflictViewModel',
    ['$scope', '$routeParams',
        function($scope, $routeParams) {

            $scope.ConflictNr = $routeParams.conflictNumber;

        }]);