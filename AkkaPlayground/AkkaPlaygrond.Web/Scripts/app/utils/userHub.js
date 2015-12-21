angular.module('Chat')
.service('UserHub', function ($q, $rootScope) {

    var self = this;
    self.userHub = null;

    var initializedTask = $q.defer()

    function initialize () {
        self.userHub = $.connection.userHub;
        self.userHub.client.chatMessageAdded = function (message) {
            $rootScope.$emit('chatMessageAdded', message);
        };

        self.userHub.connection.start().done(function() {
            initializedTask.resolve();
        });
    };
    initialize();

    function subscribe(eventName, callback) {
        self.userHub.on(eventName, callback);
    }

    function unSubscribe(eventName) {
        self.userHub.off(eventName, callback);
    }

    function joinSignalRGroup(chatId) {
        return self.userHub.server.joinSignalRGroup(chatId);
    };

    function getChatLog(chatId) {
        return self.userHub.server.getChatLog(chatId);
    }

    function getUserChat(userId, targetUserId) {
        return self.userHub.server.getUserChat(userId, targetUserId)
    }

    function createChat(userId, targetUserId) {
        return self.userHub.server.createChat(userId, targetUserId);
    }

    function addChatMessage(userId, chatId, text) {
        return self.userHub.server.addChatMessage(userId, chatId, text);
    }

    function getUsersContacts(userId) {
        return self.userHub.server.getUsersContacts(userId);
    }

    function register(data) {
        return self.userHub.server.register(data);
    }

    function login(value) {
        return self.userHub.server.login(value);
    }

    function addToContactList(userId, targetUserId) {
        return self.userHub.server.addToContactList(userId, targetUserId);
    }

    function search(userId, value) {
        return self.userHub.server.search(userId, value);
    }

    function getUserChats(userId) {
        return self.userHub.server.getUserChats(userId);
    }

    function joinSignalRChatGroups(userId) {
        return self.userHub.server.joinSignalRChatGroups(userId);
    }

    return {
        subscribe: subscribe,
        unSubscribe: unSubscribe,
        initialized: initializedTask.promise,
        getChatLog: getChatLog,
        getUserChat: getUserChat,
        createChat: createChat,
        addChatMessage: addChatMessage,
        getUsersContacts: getUsersContacts,
        register: register,
        login: login,
        addToContactList: addToContactList,
        search: search,
        getUserChats: getUserChats,
        joinSignalRGroup: joinSignalRGroup,
        joinSignalRChatGroups: joinSignalRChatGroups
    };

});
