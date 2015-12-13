angular.module('Chat')
.controller("ChatCtrl", function ($scope, $http, $stateParams, UserContext, UserHub) {

    var chatId = undefined;
    var targetUserId = $stateParams.userId;
    $scope.initialized = false;
    $scope.messageModel = { text: '' };
    $scope.messages = [];

    $scope.sendMessage = function () {
        var loggedInUser = UserContext.getUser();
        if (!loggedInUser || !chatId || !$scope.messageModel.text) {
            return;
        }
        UserHub.addChatMessage(loggedInUser.Id, chatId, $scope.messageModel.text);
        $scope.messageModel.text = '';
    };

    var loggedInUser = UserContext.getUser();
    $scope.currentUserId = loggedInUser.Id;

    UserHub.initialized.then(function() {
        if (!$scope.currentUserId) {
            return;
        }
        UserHub.subscribe('chatMessageAdded', function (message) {
            if (chatId != message.ChatId) {
                return;
            }

            $scope.$apply(function () {
                $scope.messages.push(message);
            });
        });

        UserHub.getUserChat(loggedInUser.Id, targetUserId).done(function (result) {
            $scope.initialized = true;
            chatId = result;

            if (!result) {
                UserHub.createChat(loggedInUser.Id, targetUserId).done(function (result) {
                    chatId = result;
                });
            }
        });
    })

});