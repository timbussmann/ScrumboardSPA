/// <reference path="../../scripts/jasmine.js" />
/// <reference path="../../scripts/angular.js" />
/// <reference path="../../scripts/angular-mocks.js" />
/// <reference path="../../../scrumboardspa/app/appmodule.js" />
/// <reference path="../../../scrumboardspa/scripts/underscore.js" />
/// <reference path="../../../scrumboardspa/app/viewmodels/scrumboardviewmodel.js" />
describe('Scrumboard Viewmodel', function () {

    $ = { connection: { storyHub: { client: {} }, hub:{start: function() {}} } };
    
    var scope, rootScope;
    var scrumboardService = {
        getStates: function(callback) {
            this.statesCallback = callback;
        },
        getStories: function (callback) {
            this.storiesCallback = callback;
        },
        getStory: function (id, callback) {
            this.storyCallback = callback;
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

    var signalREventsService = {};
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
                signalREventsService: signalREventsService,
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

    describe('updateStory is called on hub', function() {
        var newStory;
        beforeEach(function () {
            spyOn(notificationService, 'notifySuccess');
            var oldStory = { Id: 1, Title: 'old' };
            newStory = { Id: 1, Title: 'new', State: 'To Verify' };
            scope.Stories = [oldStory];
            _ = {
                findWhere: function () { return oldStory; },
                indexOf: function () { return 0; }
            };

            scope.$broadcast('UpdateSuccessful', newStory);
            scrumboardService.storyCallback(newStory);
        });
        
        it('should update story', function () {
            expect(scope.Stories[0]).toBe(newStory);
        });
        
        it('should notify success', function () {
            expect(notificationService.notifySuccess).toHaveBeenCalledWith('Updated story #"' + newStory.Id + '"');
        });
    });
    
    describe('deleteStory is called on hub', function() {
        beforeEach(function () {
            spyOn(notificationService, 'notifySuccess');
            var story = { Id: 1, Title: 'old' };
            scope.Stories = [story];
            _ = {
                findWhere: function () { return story; },
                indexOf: function () { return 0; }
            };

            scope.$broadcast('DeletedSuccessful', 1);
        });
        
        it('should update story', function () {
            expect(scope.Stories.length).toBe(0);
        });
        
        it('should notify success', function () {
            expect(notificationService.notifySuccess).toHaveBeenCalledWith('Story old deleted');
        });
    });

    describe('UpdateStoryState is called', function () {
        var stories;
        
        beforeEach(function() {
            spyOn(notificationService, 'notifySuccess');
            stories = [{ Title: 'story1', State: 'done', Id: 42 }];
            scrumboardService.storiesCallback(stories);

            scope.UpdateStoryState(stories[0], { State: 'ToVerify' });
        });
        
        it('should set story Id', function () {
            expect(scrumboardService.setStoryStateParameter.StoryId).toBe(stories[0]);
        });
        
        it('should set story state to ToVerify', function () {
            expect(scrumboardService.setStoryStateParameter.NewState).toBe('ToVerify');
        });    
    });

    describe('createStory is called on hub', function () {
        var newStory;
        beforeEach(function () {
            spyOn(notificationService, 'notifySuccess');
            newStory = { Id: 1, Title: 'new', State: 'To Verify' };
            scope.Stories = [];

            scope.$broadcast('CreateSuccessful', newStory);
            scrumboardService.storyCallback(newStory);
        });

        it('should create story', function () {
            expect(scope.Stories[0]).toBe(newStory);
        });       

        it('should notify success', function () {
            expect(notificationService.notifySuccess).toHaveBeenCalledWith(newStory.Title + ' - <a href="/story/' + newStory.Id + '">[click to see story]</a>', 'New Story created');
        });
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

