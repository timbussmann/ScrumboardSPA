var app = angular.module('appModule', [], function ($routeProvider, $locationProvider) {
    
    $routeProvider.when('/scrumboard', {
        templateUrl: '/views/ScrumboardView',
        controller: 'scrumboardViewModel'
    });

    $routeProvider.when('/story/new', {
        templateUrl: '/views/NewStoryView',
        controller: 'newStoryViewModel'
    });

    $routeProvider.when('/story/:storyId', {
        templateUrl: '/views/StoryDetailView',
        controller: 'storyDetailViewModel'
    });

    $routeProvider.when('/conflict/:conflictNumber', {
        templateUrl: '/views/ResolveConflictView',
        controller: 'resolveConflictViewModel'
    });

    // start with scrumboard view
    $routeProvider.otherwise({redirectTo: '/scrumboard'});

    // omit the hashtag in the url if possible
    $locationProvider.html5Mode(true);
});


// This directive allows to define a html5 drag & drop dropzone
// It will call the specified method on the current scope.
// Use the dropzone-parameter directive to add additional parameters from the
// scope to the function call.
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
            var rawData = e.dataTransfer.getData('text');
            var transferData = JSON.parse(rawData);
            scope[attrs['dropzone']](transferData, scope[attrs['dropzoneParameter']]);
        }, false);
    };
});

// This directive allows to define a scope variable which will be received
// by a html 5 drag & drop dropzone (see dropzone directive). The attribute
// will be transfered as json object.
app.directive('dropData', function() {
    return function(scope, elements, attrs) {
        var element = elements[0];

        element.addEventListener('dragstart', function(e) {
            //e.dataTransfer.effectAllowed = 'move';
            var transferData = JSON.stringify(scope[attrs['dropData']]);
            e.dataTransfer.setData('text', transferData);
        });
    };
});
