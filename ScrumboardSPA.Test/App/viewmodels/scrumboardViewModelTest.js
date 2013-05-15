/// <reference path="../../scripts/jasmine.js" />
/// <reference path="../../scripts/angular.js" />
/// <reference path="../../scripts/angular-mocks.js" />
/// <reference path="../../../scrumboardspa/app/appmodule.js" />
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
        }
    };

    var location = {
        url: function() {}
    };

    beforeEach(function() {
        module('appModule');
        inject(function($rootScope, $controller) {
            scope = $rootScope.$new();
            $controller('scrumboardViewModel', { $scope: scope, scrumboardService: scrumboardService, $location: location });
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
});

