app.controller('resolveConflictViewModel',
    ['$scope', '$routeParams', 'conflictService', '$location',
        function($scope, $routeParams, conflictService, $location) {

            var conflict = conflictService.getConflict($routeParams.conflictNumber);

            var mappedConflicts = _.map(_.omit(conflict.original, 'Id', 'Etag'), function(value, key) {
                return {
                    key: key,
                    original: value,
                    requested: conflict.requested[key],
                    hasConflict: value !== conflict.requested[key]
                };
            });

            $scope.Conflicts = mappedConflicts;
        }]);