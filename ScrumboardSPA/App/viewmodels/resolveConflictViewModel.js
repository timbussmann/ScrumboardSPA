app.controller('resolveConflictViewModel',
    ['$scope', '$routeParams', 'conflictService', '$location', 'notificationService', 'scrumboardService',
        function($scope, $routeParams, conflictService, $location, notificationService, scrumboardService) {

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

            $scope.TakeRequested = function () {
                scrumboardService.setStoryState(conflict.original, conflict.requested.State);
                conflictService.resolveConflict(conflictNr);

                $scope.$broadcast('StoryChanged', conflict.requested);
                $location.url('/scrumboard');
            };
        }]);