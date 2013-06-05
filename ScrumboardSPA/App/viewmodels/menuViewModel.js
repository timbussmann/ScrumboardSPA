app.controller('menuViewModel',
    ['$scope',
        function ($scope) {

            $scope.OnlineUsers = 0;
            
            $scope.$on('OnlineUsersUpdatedSuccessful', function (event, onlineUsersCount) {
                $scope.OnlineUsers = onlineUsersCount;
            });
        }]);