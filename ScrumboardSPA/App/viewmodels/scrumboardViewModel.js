app.controller('ScrumboardViewModel', function($scope) {
    var states = ['ToDo', 'WIP', 'To Verify', 'Done'];
    $scope.States = states;
    $scope.Stories = [
        new Story('Story1', 'Description for the first story.'),
        new Story('Story2', 'Yet another story description', states[1]),
        new Story('Story3', 'This is the description for story 3. This description is also very long and does not fit on a single line')];

    function Story(title, description, state) {
        this.title = title;
        this.description = description;
        this.state = state || states[0];
    }
});