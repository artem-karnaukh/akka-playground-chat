angular.module('Chat')
.service('ChatService', function ($q, $http) {
    function create(userId, targetUserId) {
        var url = 'api/chatapi/Create';
        var model = { UserId: userId, TargetUserId: targetUserId };

        return $http({
            method: 'POST', url: url, data: JSON.stringify(model)
        });
    };

    return {
        create: create,
    };
});