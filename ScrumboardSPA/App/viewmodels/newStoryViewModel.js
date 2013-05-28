app.controller('newStoryViewModel',
    ['$scope', 'scrumboardService', '$location',
        function ($scope, scrumboardService, $location) {

            $scope.Story = {};
            $scope.ShowErrors = false;

            $scope.CreateStory = function () {
                if ($scope.newStoryForm.$invalid) {
                    $scope.ShowErrors = true;
                    return;
                } else {
                    $scope.ShowErrors = false;
                    var story = $scope.Story;
                    $scope.Story = {};
                    scrumboardService.createStory(story, function (createdStory) {
                        $location.url('/scrumboard');
                    }, function (error, statusCode) {
                        if (error && error.Message) {
                            // Web API error:
                            $scope.ServerError = statusCode + ': ' + error.Message;
                        } else {
                            $scope.ServerError = 'HTTP Code ' + statusCode;
                        }
                    });
                }
            };
        }]);