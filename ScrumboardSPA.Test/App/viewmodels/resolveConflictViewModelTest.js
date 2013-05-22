/// <reference path="../../scripts/jasmine.js" />
/// <reference path="../../scripts/angular.js" />
/// <reference path="../../scripts/angular-mocks.js" />
/// <reference path="../../../scrumboardspa/app/appmodule.js" />
/// <reference path="../../../scrumboardspa/app/viewmodels/resolveConflictViewModel.js" />
describe('resolve conflict Viewmodel', function () {
    var scope;
    
    var scrumboardService = {
    };
    
    var location = {
        url: function () { }
    };

    var conflictService = {
        getConflict: function() { }
    };

    var $routeParams = { conflictNumber: 42 };

    beforeEach(function () {
        module('appModule');
        inject(function ($rootScope, $controller) {
            scope = $rootScope.$new();
            $controller('resolveConflictViewModel', {
                $scope: scope,
                $routeParams : $routeParams,
                conflictService: conflictService,
                $location: location,
            });
        });
    });

    it('should show original and requested stories', function() {
        var expectedConflict = { original: 'original', requested: 'requested'};
        conflictService.getConflict = function () {
            return expectedConflict;
        };

        // will be called upon initialization so we have to create a new controller:
        inject(function ($controller) {
            $controller('resolveConflictViewModel', {
                $scope: scope,
                $routeParams: $routeParams,
                conflictService: conflictService,
                $location: location,
            });
        });

        expect(scope.Conflict).toBe(expectedConflict);
    });
});