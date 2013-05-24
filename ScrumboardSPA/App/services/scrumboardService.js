app.service('scrumboardService', ['$http', '$rootScope', function($http, $rootScope) {
    // angular services expect a constructor function. for a module pattern style way, look at angular factory
    
    this.getStates = function(callback) {
        $http.get('/api/stories/states', {
            cache: true
        }).success(callback);
    };
    this.getStories = function(callback) {
        $http.get('/api/stories').success(callback);
    };
    this.getStory = function(storyId, callback) {
        $http.get('/api/story/' + storyId).success(callback);
    };
    this.setStoryState = function(story, state) {
        $http.put('/api/story/' + story.Id + '/state/' + state,
            '"' + story.Etag + '"') // WebAPI requires single primitive datatypes as a string and not as json object
            .success(function(data) {
                $rootScope.$broadcast('UpdateSuccessful', data);
            })
            .error(function (error, statusCode) {
                // Conflict:
                if (statusCode == 409) {
                    $rootScope.$broadcast('UpdateConflicted', error);
                } else {
                    $rootScope.$broadcast('UpdateFailed', statusCode);
                }
            });
    };
    this.createStory = function(story, successCallback, errorCallback) {
        $http.post('/api/story', story)
            .success(successCallback)
            .error(errorCallback);
    };
}]);