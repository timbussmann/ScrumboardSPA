app.service('scrumboardService', function($http) {
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
    this.setStoryState = function(story, state, successCallback, errorCallback) {
        $http.put('/api/story/' + story.Id + '/state/' + state,
            '"' + story.Etag + '"') // WebAPI requires single primitive datatypes as a string and not as json object
            .success(successCallback)
            .error(errorCallback);
    };
    this.createStory = function(story, successCallback, errorCallback) {
        $http.post('/api/story', story)
            .success(successCallback)
            .error(errorCallback);
    };
});