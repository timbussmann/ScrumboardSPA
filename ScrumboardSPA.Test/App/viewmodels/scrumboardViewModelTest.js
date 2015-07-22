/// <reference path="../../scripts/jasmine.js" />
/// <reference path="../../scripts/angular.js" />
/// <reference path="../../scripts/angular-route.js" />
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
        getStory: function (id, callback) {
            this.storyCallback = callback;
        },
        setStoryState: function(storyId, newState, successCallback, errorCallback) {
            this.setStoryStateSuccessCallback = successCallback;
            this.setStoryStateErrorCallback = errorCallback;
        },
        deleteStory: function() {}
    };

    var location = {
        url: function() {}
    };

    var notificationService = {
        notifySuccess: function () { },
        notifyError: function () { },
        notifyInfo: function () { }
    };

    var signalREventsService = {};
    var conflictService = {};

    function createController() {
        inject(function ($rootScope, $controller) {
            scope = $rootScope.$new();
            rootScope = $rootScope;
            $controller('scrumboardViewModel', {
                $scope: scope,
                scrumboardService: scrumboardService,
                $location: location,
                notificationService: notificationService,
                signalREventsService: signalREventsService,
                conflictService: conflictService
            });
        });
    }

    beforeEach(function() {
        module('appModule', 'ngRoute');
        
        spyOn(notificationService, 'notifySuccess');
        spyOn(notificationService, 'notifyInfo');

        spyOn(location, 'url');
    });

    describe('on initialization', function () {
        var states = ['state1', 'state2'];
        var stories = ['story1', 'story2', 'story3'];
        
        beforeEach(function () {
            spyOn(scrumboardService, 'getStates').and.callFake(function(callback) {
                callback(states);
            });
            spyOn(scrumboardService, 'getStories').and.callFake(function(callback) {
                callback(stories);
            });
            
            createController();
        });

        it('should show states from scrumboard service', function () {
            expect(scrumboardService.getStories).toHaveBeenCalled();
            expect(scope.States.length).toBe(2);
            expect(scope.States).toEqual(states);
        });

        it('should show all retrieved stories', function () {
            expect(scope.Stories.length).toBe(3);
            expect(scope.Stories).toEqual(stories);
        });
    });

    describe('after initialization', function () {
        beforeEach(function() {
            createController();
        });
        
        describe('when story selected', function () {
            var storyId = 11;

            beforeEach(function () {
                scope.ShowStoryDetail(storyId);
            });

            it('should navigate to story detail page', function () {
                expect(location.url).toHaveBeenCalledWith('/story/' + storyId);
            });
        });

        describe('on story changed event', function () {
            var oldStory = { Id: 1, Title: 'old', State: 'Ready', Etag: 1 };
            var newStory = { Id: 1, Title: 'new', State: 'To Verify',  Etag: 2 };
            beforeEach(function () {
                spyOn(scrumboardService, 'getStory').and.callFake(function (id, callback) {
                    callback(newStory);
                });
                scope.Stories = [oldStory];

                scope.$broadcast('StoryChanged', newStory);
            });

            it('should update properties', function () {
                expect(scope.Stories[0]).toEqual(newStory);
            });

            it('should notify success', function () {
                expect(notificationService.notifySuccess).toHaveBeenCalledWith("Updated story #" + newStory.Id);
            });
        });

        describe('on story deleted event', function () {
            var story = { Id: 1, Title: 'old' };

            beforeEach(function () {
                scope.Stories = [story];

                scope.$broadcast('DeletedSuccessful', story.Id);
            });

            it('should remove the story from the board', function () {
                expect(scope.Stories.length).toBe(0);
            });

            it('should add a notification', function () {
                expect(notificationService.notifyInfo).toHaveBeenCalledWith('Story #' + story.Id + ' deleted');
            });
        });

        describe('when story has been moved to new state', function () {
            var stories = [{ Title: 'story1', State: 'done', Id: 42 }];
            var newState = { State: 'ToVerify' };

            beforeEach(function () {
                scope.Stories = stories;

                spyOn(scrumboardService, 'setStoryState');
            });

            describe('when new state is not equal to current state', function () {
                beforeEach(function () {
                    scope.UpdateStoryState(stories[0], newState);
                });

                it('should update the state', function () {
                    expect(scrumboardService.setStoryState).toHaveBeenCalledWith(stories[0], newState.State);
                });
            });

            describe('when new state is equal to current state', function () {
                beforeEach(function () {
                    newState = { State: 'done' };
                    scope.UpdateStoryState(stories[0], newState);
                });

                it('should not update the state', function () {
                    expect(scrumboardService.setStoryState).not.toHaveBeenCalled();
                });
            });
        });

        describe('on new story event', function () {
            var newStory = { Id: 1, Title: 'new', State: 'To Verify' };
            beforeEach(function () {
                spyOn(scrumboardService, 'getStory').and.callFake(function (id, callback) {
                    callback(newStory);
                });
                scope.Stories = [];

                scope.$broadcast('CreateSuccessful', newStory);
            });

            it('should create story', function () {
                expect(scope.Stories[0]).toBe(newStory);
            });

            it('should notify creation', function () {
                expect(notificationService.notifySuccess).toHaveBeenCalledWith(newStory.Title + ' - <a href="/story/' + newStory.Id + '">[click to see story]</a>', 'New Story created');
            });
        });

        describe('when story has been moved to trash', function() {
            var story = { Id: 1 };
            beforeEach(function () {
                spyOn(scrumboardService, 'deleteStory');

                scope.DeleteStory(story);
            });

            it('should call delete on scrumboardService', function() {
                expect(scrumboardService.deleteStory).toHaveBeenCalled();
            });
        });

        describe('on merge conflict', function () {
            var conflict = {
                Original: 'The original',
                Requested: 'The requested'
            };

            beforeEach(function () {
                conflictService.addConflict = jasmine.createSpy('addConflict').and.returnValue(42);

                rootScope.$broadcast('UpdateConflicted', conflict);
            });

            it('should add conflict to conflicts', function () {
                expect(conflictService.addConflict).toHaveBeenCalledWith(conflict.Original, conflict.Requested);
            });

            it('should navigate to resolve conflict view', function () {
                expect(location.url).toHaveBeenCalledWith('/conflict/42');
            });
        });
    });
});

