﻿app.service('scrumboardService',
    ['$http', '$rootScope',
        function($http, $rootScope) {
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
                    .success(function(stories) {
                        localStorage.stories = JSON.stringify(stories);
                        callback(stories);
                    })
                    .error(function(error, statusCode) {
                        if (statusCode == 0) {
                            // server not reachable, fetch from localstorage:
                            var storageStories = JSON.parse(localStorage.stories);
                            $rootScope.$broadcast('UsingCachedData');
                            callback(storageStories);
                        }
                    });
            };
            this.getStory = function(storyId, callback) {
                $http.get('/api/story/' + storyId)
                    .success(callback)
                    .error(function(error, statusCode) {
                        if (statusCode == 0) {
                            var cachedStories = JSON.parse(localStorage.stories);
                            var cachedStory = _.findWhere(cachedStories, { Id: +storyId });
                            if (cachedStory) {
                                callback(cachedStory);
                            }
                        }
                    });
            };
            this.setStoryState = function(story, state) {
                $http.put('/api/story/' + story.Id + '/state/', {
                    Etag: story.Etag,
                    State: state
                }).success(function() {
                    $rootScope.$broadcast('UpdateSuccessful', story.Id);
                }).error(function(error, statusCode) {
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
            this.deleteStory = function(story) {
                $http.delete('/api/story/' + story.Id)
                    .success().error(function (error, statusCode) {
                        // Conflict:
                        if (statusCode == 409) {
                            $rootScope.$broadcast('UpdateConflicted', error);
                        } else {
                            $rootScope.$broadcast('UpdateFailed', statusCode);
                        }
                    });
            };
        }]);