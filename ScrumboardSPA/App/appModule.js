var app = angular.module('appModule', [], function ($routeProvider, $locationProvider) {
    
    $routeProvider.when('/scrumboard', {
        templateUrl: '/views/ScrumboardView',
        controller: 'scrumboardViewModel'
    });

    $routeProvider.otherwise({redirectTo: '/scrumboard'});

    $locationProvider.html5Mode(true);
});
