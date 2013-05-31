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
            this.createStoryErrorCallback = errorCallback;
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

    describe('when invalid user inputs', function () {
        beforeEach(function() {
            spyOn(scrumboardService, 'createStory');
            
            scope.newStoryForm = {
                $invalid: true
            };
            
            scope.CreateStory();
        });
        
        it('should show validation errors', function() {
            expect(scope.ShowErrors).toBe(true);
        });

        it('should not create a story', function() {
            expect(scrumboardService.createStory).wasNotCalled();
        });
    });

    describe('when valid user inputs', function () {
        
        var expectedStory = { Title: 'MyNewStory' };
        
        beforeEach(function() {
            scope.newStoryForm = {
                $invalid: false
            };
            
            scope.Story = expectedStory;

            scope.CreateStory();
        });

        it('should create user story', function() {
            expect(scrumboardService.createdStory).toBe(expectedStory);
        });

        describe('and user story was created', function() {
            beforeEach(function() {
                spyOn(location, 'url');
                
                scrumboardService.createStorySuccessCallback({});
            });

            it('should navigate to scrumboard', function() {
                expect(location.url).toHaveBeenCalledWith('/scrumboard');
            });
        });

        describe('and error occured on creation', function() {
            beforeEach(function() {

                scrumboardService.createStoryErrorCallback({ Message: 'an error' }, 42);
            });

            it('should show error message', function() {
                expect(scope.ServerError).toContain('an error');
                expect(scope.ServerError).toContain('42');
            });
        });
    });
});