/// <reference path="../../scripts/jasmine.js" />
/// <reference path="../../scripts/angular.js" />
/// <reference path="../../scripts/angular-mocks.js" />
/// <reference path="../../../scrumboardspa/app/appmodule.js" />
/// <reference path="../../../scrumboardspa/app/viewmodels/menuviewmodel.js" />
describe('Menu Viewmodel', function () {
    var scope;

    beforeEach(function () {
        module('appModule');
        inject(function ($rootScope, $controller) {
            scope = $rootScope.$new();
            $controller('menuViewModel', {
                $scope: scope
            });
        });
    });

    describe('when updated successful event called', function () {
        var result;
        beforeEach(function () {
            scope.$broadcast('OnlineUsersUpdatedSuccessful', 2);
            result = scope.OnlineUsers;
        });

        it('should return 2', function() {
            expect(result).toEqual(2);
        });
    });
});