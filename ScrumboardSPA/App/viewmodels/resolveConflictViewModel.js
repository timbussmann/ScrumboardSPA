app.controller('resolveConflictViewModel',
    ['$scope', '$routeParams', 'conflictService', '$location', 'notificationService',
        function($scope, $routeParams, conflictService, $location, notificationService) {

            var conflictNr = $routeParams.conflictNumber;
            var conflict = conflictService.getConflict(conflictNr);

            if (conflict === undefined) {
                notificationService.notifyWarning('Conflict not found');
                $location.url('/scrumboard');
                return;
            }

            var mappedConflicts = _.map(_.omit(conflict.original, 'Id', 'Etag'), function(value, key) {
                return {
                    key: key,
                    original: value,
                    requested: conflict.requested[key],
                    hasConflict: value !== conflict.requested[key]
                };
            });

            $scope.Conflicts = mappedConflicts;

            $scope.TakeOriginal = function () {
                conflictService.resolveConflict(conflictNr);

                $scope.$broadcast('StoryChanged', conflict.original);

                $location.url('/scrumboard');
            };
        }]);