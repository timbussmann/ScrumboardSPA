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
        },
        getStates: function() {}
    };
    var location = {
        url: function (){}
    };

    beforeEach(function () {
        module('appModule');
        inject(function ($rootScope, $controller) {
            scope = $rootScope.$new();
            $controller('storyDetailViewModel', {
                $scope: scope,
                scrumboardService: scrumboardService,
                $routeParams: { storyId: StoryId },
                $location: location
            });
        });
    });

    describe('when closing detail view', function() {
        beforeEach(function() {
            spyOn(location, 'url');

            scope.CloseDetailView();
        });

        it('should navigate to scrumboard', function() {
            expect(location.url).toHaveBeenCalledWith('/scrumboard');
        });
    });

    describe('when deleting story', function () {
        var expectedStory = {
            Title: 'A Story'
        };
        
        beforeEach(function() {
            spyOn(location, 'url');
            scrumboardService.deleteStory = jasmine.createSpy('deleteStory');
            scope.Story = expectedStory;
            scope.DeleteStory();
        });

        it('should navigate to scrumboard', function () {
            expect(scrumboardService.deleteStory).toHaveBeenCalledWith(expectedStory);
        });

        it('should navigate to scrumboard', function () {
            expect(location.url).toHaveBeenCalledWith('/scrumboard');
        });
    });    
});