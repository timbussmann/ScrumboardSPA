/// <reference path="../../scripts/jasmine.js" />
/// <reference path="../../scripts/angular.js" />
/// <reference path="../../scripts/angular-mocks.js" />
/// <reference path="../../../scrumboardspa/app/appmodule.js" />
/// <reference path="../../../scrumboardspa/app/viewmodels/newstoryviewmodel.js" />
describe('new Story Viewmodel', function () {
    var scope;
    var scrumboardService = {
        createdStory: {},
        createStory: function (story, successCallback, errorCallback) {
            scrumboardService.createdStory = story;
            this.createStorySuccessCallback = successCallback;
        }
    };
    var location = {
        url: function () { }
    };

    var notificationService = {
        notifySuccess: function () { }
    };

    beforeEach(function () {
        module('appModule');
        inject(function ($rootScope, $controller) {
            scope = $rootScope.$new();
            $controller('newStoryViewModel', {
                $scope: scope,
                scrumboardService: scrumboardService,
                $location: location,
                notificationService: notificationService
            });
        });
    });

    it('should show validation errors when form is not valid', function () {
        spyOn(scrumboardService, 'createStory');
        scope.newStoryForm = {
            $invalid: true
        };

        scope.CreateStory();

        expect(scope.ShowErrors).toBe(true);
        expect(scrumboardService.createStory).wasNotCalled();
    });

    it('should create story when inputs are valid', function () {
        scope.newStoryForm = {
            $invalid: false
        };
        var expectedStory = { Title: 'MyNewStory' };
        scope.Story = expectedStory;

        scope.CreateStory();

        expect(scrumboardService.createdStory).toBe(expectedStory);
    });

    it('should navigate to scrumboard on successful creation', function () {
        spyOn(location, 'url');
        spyOn(notificationService, 'notifySuccess');
        scope.newStoryForm = { $invalid: false };
        var expectedStory = { Title: 'MyNewStory' };
        scope.Story = expectedStory;
        scope.CreateStory();

        scrumboardService.createStorySuccessCallback({});

        expect(location.url).toHaveBeenCalledWith('/scrumboard');
    });
});