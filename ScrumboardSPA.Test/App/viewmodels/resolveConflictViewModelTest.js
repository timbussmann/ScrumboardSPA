/// <reference path="../../scripts/jasmine.js" />
/// <reference path="../../scripts/angular.js" />
/// <reference path="../../scripts/angular-mocks.js" />
/// <reference path="../../../scrumboardspa/scripts/underscore.js" />
/// <reference path="../../../scrumboardspa/app/appmodule.js" />
/// <reference path="../../../scrumboardspa/app/viewmodels/resolveConflictViewModel.js" />
describe('resolve conflict Viewmodel', function() {
    var scope;
    var location = {};
    var conflictService = {};
    var $routeParams = { conflictNumber: 42 };
    var notificationService = {};

    function createController() {
        inject(function($rootScope, $controller) {
            scope = $rootScope.$new();
            $controller('resolveConflictViewModel', {
                $scope: scope,
                $routeParams: $routeParams,
                conflictService: conflictService,
                $location: location,
                notificationService: notificationService
            });
        });
    }

    beforeEach(function() {
        location.url = jasmine.createSpy('url');
        notificationService.notifyWarning = jasmine.createSpy('notifyWarning');

        module('appModule');
    });

    describe('on initialization', function() {
        describe('with existing conflict', function() {
            it('should show original and requested stories', function() {
                var expectedConflict = {
                    original: { Equal: 'Text', Different: '123' },
                    requested: { Equal: 'Text', Different: '456' }
                };
                conflictService.getConflict = function() {
                    return expectedConflict;
                };

                // create controller because the logic will be executed at initialization:
                createController();

                expect(scope.Conflicts).toEqual([
                    { key: 'Equal', original: 'Text', requested: 'Text', hasConflict: false },
                    { key: 'Different', original: '123', requested: '456', hasConflict: true }
                ]);
            });

            it('should hide Id and Etag property', function() {
                var expectedConflict = {
                    original: { Equal: 'Text', Etag: '123', Id: 1 },
                    requested: { Equal: 'Text', Etag: '456', Id: 1 }
                };
                conflictService.getConflict = function() {
                    return expectedConflict;
                };

                // create controller because the logic will be executed at initialization:
                createController();

                expect(scope.Conflicts).toEqual([
                    { key: 'Equal', original: 'Text', requested: 'Text', hasConflict: false }
                ]);
            });
        });
        describe('with undefined conflict', function() {
            beforeEach(function() {
                conflictService.getConflict = function() {
                    return undefined;
                };

                createController();
            });

            it('should notify with warning', function() {
                expect(notificationService.notifyWarning).toHaveBeenCalled();
            });

            it('should navigate to scrumboard', function() {
                expect(location.url).toHaveBeenCalledWith('/scrumboard');
            });
        });
    });

    describe('after initialization', function () {
        var conflict = {
            original: { Equal: 'Text', Different: '123', Id: 1, Etag: '1' },
            requested: { Equal: 'Text', Different: '456', Id: 1, Etag: '1' }
        };
        
        beforeEach(function () {
            conflictService.getConflict = function () {
                return conflict;
            };
            
            createController();
        });

        describe('when taking server version', function () {
            beforeEach(function() {
                spyOn(scope, '$broadcast');
                conflictService.resolveConflict = jasmine.createSpy('resolveConflict');

                scope.TakeOriginal();
            });

            it('should publish StoryChanged event with original version', function() {
                expect(scope.$broadcast.mostRecentCall.args[0]).toBe('StoryChanged');
                expect(scope.$broadcast.mostRecentCall.args[1]).toEqual(conflict.original);             
            });

            it('should mark the conflict as resolved', function() {
                expect(conflictService.resolveConflict).toHaveBeenCalledWith($routeParams.conflictNumber);
            });

            it('should navigate to scrumboard', function() {
                expect(location.url).toHaveBeenCalledWith('/scrumboard');
            });
        });
    });
});