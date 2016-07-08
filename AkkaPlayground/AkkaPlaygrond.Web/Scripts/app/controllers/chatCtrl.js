angular.module('Chat')
.controller("ChatCtrl", function ($rootScope, $scope, $stateParams, $q, rootRef, $firebaseArray, UserContext, $ionicLoading, UserService, ChatService, MessageFactory) {
    var chatId = $stateParams.chatId;
    var targetUserId = $stateParams.userId;

    var loggedInUser = UserContext.getUser();

    $scope.currentUserId = loggedInUser.Id;

    $scope.messages = undefined;

    $scope.messageModel = { text: '' };
    

    $scope.sendMessage = function () {
        var loggedInUser = UserContext.getUser();
        if (!loggedInUser || !chatId || !$scope.messageModel.text) {
            return;
        }
        ChatService.addMessage(chatId, loggedInUser.Id, $scope.messageModel.text);
        $scope.messageModel.text = '';
    };
    
    function init() {
        $ionicLoading.show({ template: 'Loading...' })
        if (targetUserId) {
            ChatService.getByUser(loggedInUser.Id, targetUserId).success(function (result) {
                if (!result) {
                    ChatService.create(loggedInUser.Id, targetUserId).success(function (result) {
                        chatId = result;
                        initChat(chatId);
                    });
                } else {
                    chatId = result.ChatId;
                    initChat(chatId); initChat
                }
            });
        } else if (chatId) {
            initChat(chatId);
        }
    }

    var loading = false;

    function onChatLoaded(e) {
        loading = false;
        $ionicLoading.hide();
    };

    function onChatAdded(e) {
        if (!loading) {
            UserService.markChatMessagesRead(loggedInUser.Id, chatId);
        }
    }

    var onChatAddedOff;

    function initChat(chatId) {
        loading = true;
        var ref = rootRef.child('chats').child(chatId).child('messages');

        $scope.messages = new MessageFactory(ref);
        $scope.messages.$loaded(onChatLoaded)
        onChatAddedOff = $scope.messages.$watch(onChatAdded);


        UserService.markChatMessagesRead(loggedInUser.Id, chatId);
    }
    $scope.$on('$destroy', function () {
        onChatAddedOff();
    });


    init();
});