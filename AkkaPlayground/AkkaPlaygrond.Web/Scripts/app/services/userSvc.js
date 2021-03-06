﻿angular.module('Chat')
.service('UserService', function ($q, $http) {

    function login(login) {
        var url = 'api/user/login';
        var model = { login: login };

        return $http({
            method: 'POST', url: url, data: JSON.stringify(model)
        });
    };

    function register(data) {
        debugger
        var url = 'api/user/register';
        return $http({
            method: 'POST', url: url, data: JSON.stringify(data)
        });
    }

    function search(currentUserId, value) {
        var url = 'api/user/search';
        var data = {
            currentUserId: currentUserId,
            searchString: value
        };
        return $http({
            method: 'POST', url: url, data: JSON.stringify(data)
        });
    }

    function getUserContacts(currentUserId) {
        var url = 'api/user/GetUserContacts?userId=' + currentUserId;
        var data = {
            userId: currentUserId,
        };
        return $http({
            method: 'GET', url: url
        });
    }

    function addToContactList(userId, targetUserId) {
        var url = 'api/user/AddToContactList';
        var data = {
            userId: userId,
            targetUserId: targetUserId
        };
        return $http({
            method: 'POST', url: url, data: JSON.stringify(data)
        });
    }

    function markChatMessagesRead(userId, chatId) {
        var url = 'api/user/MarkChatMessagesRead';
        var data = {
            userId: userId,
            chatId: chatId
        };
        return $http({
            method: 'POST', url: url, data: JSON.stringify(data)
        });
    }

    return {
        login: login,
        register: register,
        search: search,
        addToContactList: addToContactList,
        getUserContacts: getUserContacts,
        markChatMessagesRead: markChatMessagesRead
    };
});