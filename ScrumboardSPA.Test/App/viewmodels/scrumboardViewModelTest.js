/// <reference path="../../scripts/jasmine.js" />
/// <reference path="../../scripts/angular.js" />
/// <reference path="../../scripts/angular-mocks.js" />
/// <reference path="../../../scrumboardspa/app/appmodule.js" />
/// <reference path="../../../scrumboardspa/scripts/underscore.js" />
/// <reference path="../../../scrumboardspa/app/viewmodels/scrumboardviewmodel.js" />
describe('Scrumboard Viewmodel', function () {

    var scope, rootScope;
    var scrumboardService = {
        getStates: function(callback) {
            this.statesCallback = callback;
        },
        getStories: function (callback) {
            this.storiesCallback = callback;
        },
        setStoryState: function(storyId, newState, successCallback, errorCallback) {
            this.setStoryStateSuccessCallback = successCallback;
            this.setStoryStateErrorCallback = errorCallback;
            this.setStoryStateParameter = { StoryId: storyId, NewState: newState };
        }
    };

    var location = {
        url: function() {}
    };

    var notificationService = {
        notifySuccess: function () { },
        notifyError: function () { }
    };

    var conflictService = {};

    beforeEach(function() {
        module('appModule');
        inject(function ($rootScope, $controller) {
            rootScope = $rootScope;
            scope = $rootScope.$new();
            $controller('scrumboardViewModel', {
                $scope: scope,
                scrumboardService: scrumboardService,
                $location: location,
                notificationService: notificationService,
                conflictService: conflictService
            });
        });
    });

    it('should show states from scrumboard service', function () {
        scrumboardService.statesCallback(['state1', 'state2']);
        
        expect(scope.States.length).toBe(2);
        expect(scope.States[1]).toBe('state2');
    });

    it('should show all retrieved stories', function() {
        var stories = ['story1', 'story2', 'story3'];
        scrumboardService.storiesCallback(stories);

        expect(scope.Stories.length).toBe(3);
        angular.forEach(stories, function(story) {
            expect(scope.Stories).toContain(story);
        });
    });

    it('should navigate to story detail page when ShowStoryDetail clicked', function () {
        var storyId = 11;
        spyOn(location, 'url');
        
        scope.ShowStoryDetail(storyId);

        expect(location.url).toHaveBeenCalledWith('/story/' + storyId);
    });

    it('should set story state when UpdateStoryState called', function () {
        spyOn(notificationService, 'notifySuccess');
        var stories = [{ Title: 'story1', State: 'done', Id: 42 }];
        scrumboardService.storiesCallback(stories);

        scope.UpdateStoryState(stories[0], { State: 'ToVerify' });

        expect(stories[0].State).toBe('ToVerify');
    });
    
    it('should update story when update successful', function () {
        spyOn(notificationService, 'notifySuccess');
        var stories = [{ Title: 'story1', State: 'done', Id: 42 }];
        scrumboardService.storiesCallback(stories);

        rootScope.$broadcast('UpdateSuccessful', { State: 'TestState', Id: 42 });

        expect(scope.Stories[0].State).toBe('TestState');
        expect(notificationService.notifySuccess).toHaveBeenCalled();
    });

    it('should add merge conflict to conflicts and navigate to resolve view', function() {
        var conflict = {
            Original: 'The original',
            Requested: 'The requested'
        };
        conflictService.addConflict = jasmine.createSpy('addConflict').andReturn(42);
        spyOn(location, 'url');

        rootScope.$broadcast('UpdateConflicted', conflict);

        expect(conflictService.addConflict).toHaveBeenCalledWith(conflict.Original, conflict.Requested);
        expect(location.url).toHaveBeenCalledWith('/conflict/42');
    });
});

