/// <reference path="../../scripts/jasmine.js" />
/// <reference path="../../scripts/angular.js" />
/// <reference path="../../scripts/angular-mocks.js" />
/// <reference path="../../../scrumboardspa/app/appmodule.js" />
/// <reference path="../../../scrumboardspa/scripts/underscore.js" />
/// <reference path="../../../scrumboardspa/app/viewmodels/scrumboardviewmodel.js" />
describe('Scrumboard Viewmodel', function () {

    $ = { connection: { storyHub: { client: {} }, hub:{start: function() {}} } };
    
    var scope;
    var scrumboardService = {
        getStates: function(callback) {
            this.statesCallback = callback;
        },
        getStories: function (callback) {
            this.storiesCallback = callback;
        },
        setStoryState: function(storyId, newState, callback) {
            this.setStoryStateCallback = callback;
            this.setStoryStateParameter = { StoryId: storyId, NewState: newState };
        }
    };

    var location = {
        url: function() {}
    };

    var notificationService = {
        notifySuccess: function () { }
    };

    beforeEach(function() {
        module('appModule');
        inject(function($rootScope, $controller) {
            scope = $rootScope.$new();
            $controller('scrumboardViewModel', {
                $scope: scope,
                scrumboardService: scrumboardService,
                $location: location,
                notificationService: notificationService
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

    it('should update story when updateStory is called on hub', function () {
        var oldStory = { Id: 1, Title: 'old' };
        var newStory = { Id: 1, Title: 'new' };
        scope.Stories = [oldStory];
        _ = {
            findWhere: function () { return oldStory; },
            indexOf: function () { return 0; }
        };
        $.connection.storyHub.client.updateStory(newStory);

        expect(scope.Stories[0]).toBe(newStory);
    });

    it('should set story state when UpdateStoryState called', function () {
        spyOn(notificationService, 'notifySuccess');
        var stories = [{ Title: 'story1', State: 'done', Id: 42 }];
        scrumboardService.storiesCallback(stories);

        scope.UpdateStoryState(stories[0], { State: 'ToVerify' });

        expect(scrumboardService.setStoryStateParameter.StoryId).toBe(stories[0]);
        expect(scrumboardService.setStoryStateParameter.NewState).toBe('ToVerify');
        scrumboardService.setStoryStateCallback({ State: 'TestState', Id: 42 });
        expect(scope.Stories[0].State).toBe('TestState');
        expect(notificationService.notifySuccess).toHaveBeenCalled();
    });
});

