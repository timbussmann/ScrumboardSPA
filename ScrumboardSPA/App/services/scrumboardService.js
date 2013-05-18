app.service('scrumboardService', function ($http) {

    return {
        getStates: function(callback) {
            $http.get('/api/stories/states', {
                cache:  true
            }).success(callback);
        },
        getStories: function(callback) {
            $http.get('/api/stories').success(callback);
        },
        getStory: function(storyId, callback) {
            $http.get('/api/story/' + storyId).success(callback);
        },
        setStoryState: function (story, state, successCallback, errorCallback) {
            $http.put('/api/story/' + story.Id + '/state/' + state,
                '"' + story.Etag + '"') // WebAPI requires single primitive datatypes as a string and not as json object
                .success(successCallback)
                .error(errorCallback);
        },
        createStory: function(story, successCallback, errorCallback) {
            $http.post('/api/story', story)
                .success(successCallback)
                .error(errorCallback);
        }
    };
});