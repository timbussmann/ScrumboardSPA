app.controller('newStoryViewModel',
    ['$scope', 'scrumboardService', '$location', 'notificationService',
        function ($scope, scrumboardService, $location, notificationService) {

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
                        notificationService.notifySuccess(createdStory.Title + ' - <a href="/story/' + createdStory.Id + '">[click to see story]</a>', 'New Story created');
                        $location.url('/scrumboard');
                    }, function (error) {
                        $scope.ServerError = error;
                    });
                }
            };
        }]);