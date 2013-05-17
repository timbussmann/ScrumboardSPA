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
                        toastr.success(createdStory.Title + ' - <a href="/story/' + createdStory.Id + '">[click to see story]</a>', 'New Story created');
                        $location.url('/scrumboard');
                    }, function (error) {
                        $scope.ServerError = error;
                    });
                }
            };
        }]);