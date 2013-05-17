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
        setStoryState: function (storyId, state, callback) {
            $http.put('/api/story/' + storyId + '/state/' + state).success(callback);
        },
        createStory: function(story, successCallback, errorCallback) {
            $http.post('/api/story', story)
                .success(successCallback)
                .error(errorCallback);
        }
    };
});