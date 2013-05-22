app.factory('conflictService', function () {

    var conflicts = new Array();

    return {
        addConflict: function (original, requested) {
            var conflict = {
                original: original,
                requested: requested
            };
            conflicts.push(conflict);
            return _.indexOf(conflicts, conflict);
        },
        getConflict: function(conflictNumber) {
            return conflicts[conflictNumber];
        }
    };
});