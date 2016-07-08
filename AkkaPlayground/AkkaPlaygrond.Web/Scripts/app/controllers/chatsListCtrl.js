angular.module('Chat')
.controller("ChatsListCtrl", function ($scope, $state, UserContext, rootRef, $firebaseArray, ChatFactory) {

    var loggedInUser = UserContext.getUser();

    var ref = rootRef.child('user-chats').child(loggedInUser.Id);
    $scope.chats = new ChatFactory(ref);


    $scope.openChat = function (chat) {
        $state.go('tab.chat-details', { chatId: chat.ChatId });
    };

});