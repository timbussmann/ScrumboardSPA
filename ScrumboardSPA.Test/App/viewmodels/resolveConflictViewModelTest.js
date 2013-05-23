/// <reference path="../../scripts/jasmine.js" />
/// <reference path="../../scripts/angular.js" />
/// <reference path="../../scripts/angular-mocks.js" />
/// <reference path="../../../scrumboardspa/scripts/underscore.js" />
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
    
    function createController() {
        inject(function ($rootScope, $controller) {
            scope = $rootScope.$new();
            $controller('resolveConflictViewModel', {
                $scope: scope,
                $routeParams: $routeParams,
                conflictService: conflictService,
                $location: location,
            });
        });
    }

    beforeEach(function () {
        module('appModule');
    });

    it('should show original and requested stories', function() {
        var expectedConflict = {
            original: { Equal: 'Text', Different: '123', Id: 1 },
            requested: { Equal: 'Text', Different: '456', Id: 1 }
        };
        conflictService.getConflict = function () {
            return expectedConflict;
        };

        // will be called upon initialization so we have to create a new controller:
        createController();

        expect(scope.Conflicts).toEqual([
            { key: 'Equal', original: 'Text', requested: 'Text', hasConflict: false },
            { key: 'Different', original: '123', requested: '456', hasConflict: true }
        ]);
    });
});