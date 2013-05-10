/// <reference path="../../scripts/jasmine.js" />
/// <reference path="../../scripts/angular.js" />
/// <reference path="../../scripts/angular-mocks.js" />
/// <reference path="../../../scrumboardspa/app/appmodule.js" />
/// <reference path="../../../scrumboardspa/app/viewmodels/storydetailviewmodel.js" />
describe('Story Detail Viewmodel', function () {
    var StoryId = 42;
    var scope;
    var scrumboardService = {
        getStory: function(storyId, callback) {
            if (storyId == StoryId) {
                this.storyCallback = callback;
            }
        }
    };

    beforeEach(function () {
        module('appModule');
        inject(function ($rootScope, $controller) {
            scope = $rootScope.$new();
            $controller('storyDetailViewModel', {
                $scope: scope,
                scrumboardService: scrumboardService,
                $routeParams: {storyId: StoryId}
            });
        });
    });

    it('should load selected story', function () {
        var expectedStory = {
            Title: 'A Story'
        };

        scrumboardService.storyCallback(expectedStory);

        expect(scope.Story).toBe(expectedStory);
    });
});