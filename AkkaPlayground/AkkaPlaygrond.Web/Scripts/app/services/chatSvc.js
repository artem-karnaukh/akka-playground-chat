angular.module('Chat')
.service('ChatService', function ($q, $http) {
    function create(userId, targetUserId) {
        var url = 'api/chatapi/Create';
        var model = { UserId: userId, TargetUserId: targetUserId };

        return $http({
            method: 'POST', url: url, data: JSON.stringify(model)
        });
    };

    function addMessage(chatId, userId, message) {
        var url = 'api/chatapi/addMessage';
        var data = { chatId: chatId, author: userId, message: message };

        return $http({
            method: 'POST', url: url, data: JSON.stringify(data)
        });
    };

    function getByUser(userId, targetUserId) {
        var url = 'api/chatapi/GetByUser';
        url += "?userId=" + userId;
        url += "&targetUserId=" + targetUserId;

        return $http({
            method: 'GET', url: url
        });
    };

    return {
        getByUser: getByUser,
        create: create,
        addMessage: addMessage
    };
});