/// <reference path="../../scripts/jasmine.js" />
/// <reference path="../../scripts/angular.js" />
/// <reference path="../../scripts/angular-mocks.js" />
/// <reference path="../../../scrumboardspa/app/appmodule.js" />
/// <reference path="../../../scrumboardspa/app/viewmodels/scrumboardviewmodel.js" />
describe('Scrumboard Viewmodel', function () {

    var scope;
    var scrumboardService = {
        states: [], // reference to the return value of getStates
        stories: [], // reference to the return value of getStories
        getStates: function() {
            return this.states;
        },
        getStories: function () {
            return this.stories;
        }
    };

    beforeEach(function() {
        module('appModule');
        inject(function($rootScope, $controller) {
            scope = $rootScope.$new();
            $controller('ScrumboardViewModel', { $scope: scope, scrumboardService: scrumboardService });
        });
    });

    it('should show states from scrumboard service', function () {
        scrumboardService.states.push('state1');
        scrumboardService.states.push('state2');
        
        expect(scope.States.length).toBe(2);
        expect(scope.States[1]).toBe(scrumboardService.states[1]);
    });

    it('should show all retrieved stories', function() {
        scrumboardService.stories.push('story1');
        scrumboardService.stories.push('story2');
        scrumboardService.stories.push('story3');

        expect(scope.Stories.length).toBe(3);
        angular.forEach(scrumboardService.stories, function(story) {
            expect(scope.Stories).toContain(story);
        });
    });
});

