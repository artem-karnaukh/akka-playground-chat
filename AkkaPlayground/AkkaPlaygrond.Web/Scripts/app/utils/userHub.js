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


    function getUserChats(userId) {
        return self.userHub.server.getUserChats(userId);
    }

    function joinSignalRGroup(groupId) {
        return self.userHub.server.joinSignalRGroup(groupId);
    };

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
        getUserChats: getUserChats,
        joinSignalRGroup: joinSignalRGroup,
        joinSignalRChatGroups: joinSignalRChatGroups
    };

});
