app.service('scrumboardService', function ($http) {

    return {
        getStates: function(callback) {
            $http.get('/api/stories/states').success(callback);
        },
        getStories: function(callback) {
            $http.get('/api/stories').success(callback);
        },
        getStory: function(storyId, callback) {
            $http.get('/api/story/' + storyId).success(callback);
        }
    };
});