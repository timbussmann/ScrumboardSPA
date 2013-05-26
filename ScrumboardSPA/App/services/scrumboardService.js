app.service('scrumboardService',
    ['$http', '$rootScope', 'notificationService',
    function ($http, $rootScope, notificationService) {
    // angular services expect a constructor function. for a module pattern style way, look at angular factory
    
    this.getStates = function(callback) {
        $http.get('/api/stories/states', {
            cache: true
        }).success(function(states) {
            localStorage.states = JSON.stringify(states);
            callback(states);
        }).error(function(error, statusCode) {
            if (statusCode == 0) {
                // server not reachable, fetch from localstorage:
                callback(JSON.parse(localStorage.states));
            }
        });
    };
    this.getStories = function(callback) {
        $http.get('/api/stories')
            .success(function (stories) {
                localStorage.stories = JSON.stringify(stories);
                callback(stories);
            })
            .error(function(error, statusCode) {
                if (statusCode == 0) {
                    // server not reachable, fetch from localstorage:
                    var storageStories = JSON.parse(localStorage.stories);
                    callback(storageStories);
                }
            });
    };
    this.getStory = function(storyId, callback) {
        $http.get('/api/story/' + storyId).success(callback);
    };
    this.setStoryState = function(story, state) {
        $http.put('/api/story/' + story.Id + '/state/' + state,
            '"' + story.Etag + '"') // WebAPI requires single primitive datatypes as a string and not as json object
            .success(function (data) {
                notificationService.notifySuccess('Story #' + story.Id + ' updated');
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