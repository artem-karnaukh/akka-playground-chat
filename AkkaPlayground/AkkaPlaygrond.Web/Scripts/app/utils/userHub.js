angular.module('Chat')
.factory('UserHub', function ($q) {

    var userHub = null;

    var initializedTask = $q.defer()

    function initialize () {
        userHub = $.connection.userHub;
        userHub.on('onTest', function (text) { } );
        userHub.connection.start().done(function() {
            initializedTask.resolve();
        });
    };
    initialize();

    function subscribe(eventName, callback) {
        userHub.on(eventName, callback);
    }

    function unSubscribe(eventName) {
        userHub.off(eventName, callback);
    }

    function getUserChat(userId, targetUserId) {
        return userHub.server.getUserChat(userId, targetUserId)
    }

    function createChat(userId, targetUserId) {
        return userHub.server.createChat(userId, targetUserId);
    }

    function addChatMessage(userId, chatId, text) {
        return userHub.server.addChatMessage(userId, chatId, text);
    }

    function getUsersContacts(userId) {
        return userHub.server.getUsersContacts(userId);
    }

    function register(data) {
        return userHub.server.register(data);
    }

    function login(value) {
        return userHub.server.login(value);
    }

    function addToContactList(userId, targetUserId) {
        return userHub.server.addToContactList(userId, targetUserId);
    }

    function search(userId, value) {
        return userHub.server.search(userId, value);
    }

    return {
        subscribe: subscribe,
        unSubscribe: unSubscribe,
        initialized: initializedTask.promise,

        getUserChat: getUserChat,
        createChat: createChat,
        addChatMessage: addChatMessage,
        getUsersContacts: getUsersContacts,
        register: register,
        login: login,
        addToContactList: addToContactList,
        search: search
    };

});
