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
    return function (scope, elements, attrs) {
        var element = elements[0];
        
        element.addEventListener('dragover', function(e) {
            e.preventDefault && e.preventDefault();

            e.dataTransfer.dropEffect = 'move';

            return false;
        }, false);

        element.addEventListener('dragenter', function(e) {
            //element.classList.add('dropzoneDragOver');
        }, false);
        element.addEventListener('dragleave', function(e) {
            //element.classList.remove('dropzoneDragOver');
        }, false);
        element.addEventListener('drop', function(e) {
            var rawData = e.dataTransfer.getData('application/json');
            var transferData = JSON.parse(rawData);
            scope[attrs['dropzone']](transferData, scope[attrs['dropzoneParameter']]);
        }, false);
    };
});

app.directive('dropData', function() {
    return function(scope, elements, attrs) {
        var element = elements[0];

        element.addEventListener('dragstart', function(e) {
            //e.dataTransfer.effectAllowed = 'move';
            var transferData = JSON.stringify(scope[attrs['dropData']]);
            e.dataTransfer.setData('application/json', transferData);
        });
    };
});
