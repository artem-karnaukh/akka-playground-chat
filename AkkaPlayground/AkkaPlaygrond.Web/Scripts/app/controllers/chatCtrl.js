angular.module('Chat')
.controller("ChatCtrl", function ($rootScope, $scope, $stateParams, $q, UserContext, UserHub, ChatService) {
    var chatId = $stateParams.chatId;
    var targetUserId = $stateParams.userId;

    //$scope.messageModel = { text: '' };
    //$scope.messages = [];

    //$scope.sendMessage = function () {
    //    var loggedInUser = UserContext.getUser();
    //    if (!loggedInUser || !chatId || !$scope.messageModel.text) {
    //        return;
    //    }
    //    UserHub.addChatMessage(loggedInUser.Id, chatId, $scope.messageModel.text);
    //    $scope.messageModel.text = '';
    //};

    //var loggedInUser = UserContext.getUser();
    //$scope.currentUserId = loggedInUser.Id;

    //var ready = $q.defer();

    //ready.promise.then(function () {
    //    $rootScope.$on('chatMessageAdded', function (e, message) {
    //        if (chatId != message.ChatId) {
    //            return;
    //        }

    //        $scope.$apply(function () {
    //            $scope.messages.push(message);
    //        });
    //    });

    //    UserHub.getChatLog(chatId).then(function (response) {
    //        $scope.messages = $scope.messages.concat(response.Log);
    //        $scope.$digest();
    //    })
    //});
    var loggedInUser = UserContext.getUser();

    ChatService.create(loggedInUser.Id, targetUserId).success(function (result) {
        chatId = result;
    });

    //UserHub.initialized.then(function() {
    //    if (!$scope.currentUserId) {
    //        ready.resolve();
    //        return;
    //    }
    //    if (!chatId) {
    //        UserHub.getUserChat(loggedInUser.Id, targetUserId).done(function (result) {
    //            chatId = result;

    //            if (!result) {
    //                UserHub.createChat(loggedInUser.Id, targetUserId).done(function (result) {
    //                    chatId = result;
    //                    ready.resolve()
    //                });
    //            } else {
    //                ready.resolve()
    //            }
    //        });
    //    } else {
    //        ready.resolve();
    //    }
    //})

});