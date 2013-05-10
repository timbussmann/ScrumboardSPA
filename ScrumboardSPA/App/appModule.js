var app = angular.module('appModule', [], function ($routeProvider, $locationProvider) {
    
    $routeProvider.when('/scrumboard', {
        templateUrl: '/views/ScrumboardView',
        controller: 'scrumboardViewModel'
    });

    $routeProvider.when('/story/:storyId', {
        templateUrl: '/views/StoryDetailView',
        controller: 'storyDetailViewModel'
    });

    $routeProvider.otherwise({redirectTo: '/scrumboard'});

    $locationProvider.html5Mode(true);
});

app.directive('dropzone', function () {
    return function (scope, element, attrs) {
        element[0].addEventListener('dragover', function(e) {
            e.preventDefault && e.preventDefault();

            e.dataTransfer.dropEffect = 'move';

            return false;
        }, false);

        element[0].addEventListener('dragenter', function(e) {
            element[0].classList.add('dropzoneDragOver');
        }, false);
        element[0].addEventListener('dragleave', function(e) {
            this.classList.remove('dropzoneDragOver');
        }, false);
    };
});
