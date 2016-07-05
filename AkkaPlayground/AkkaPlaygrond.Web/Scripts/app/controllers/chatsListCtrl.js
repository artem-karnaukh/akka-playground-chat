angular.module('Chat')
.controller("ChatsListCtrl", function ($scope, UserContext, $firebaseArray) {
    var ref = firebase.database().ref().child('chats').child("2747cb7e-da07-4312-9083-46588a29dbe7");
    var query = ref.orderByChild("ChatName");
    $scope.chats = $firebaseArray(ref);

    //$scope.doRefresh = function () {
    //    getUsersChats();
    //};

    //function getUsersChats() {
    //    var loggedInUser = UserContext.getUser();
    //    if (!loggedInUser) {
    //        return;
    //    }

    //    UserHub.getUserChats(loggedInUser.Id).done(function (data) {
    //        $scope.$apply(function () {
    //            var chatModels = [];
    //            for (var i = 0; i < data.Chats.length; i++) {
    //                var dto = data.Chats[i];
    //                var itemModel = mapDto(dto);
    //                chatModels.push(itemModel);
    //            }
    //            $scope.chats = chatModels;

    //            $scope.$broadcast('scroll.refreshComplete');
    //        })
    //    });
    //}

    //function mapDto(dto) {
    //    var chatModel = {
    //        ChatId : dto.ChatId,
    //        LastMessage : dto.LastMessage,
    //        LastMessageDate : moment(dto.LastMessageDate).fromNow()
    //    };
    //    var loggedInUser = UserContext.getUser();
    //    for (var i = 0; i < dto.Participants.length; i++) {
    //        if (dto.Participants[i].UserId != loggedInUser.Id) {
    //            chatModel.TargetUserName = dto.Participants[i].Name;
    //        }
    //    }
    //    return chatModel;
    //}

    //$scope.openChat = function (chat) {
    //    $state.go('tab.chat-details', { chatId: chat.ChatId });
    //}

    //$rootScope.$on('chatMessageAdded', function (e, message) {
    //    debugger
    //    for (var i = 0; i < $scope.chats.length; i++) {
    //        var item = $scope.chats[i];
    //        if (item.ChatId == message.ChatId) {
    //            item.LastMessage = message.Message;
    //            item.LastMessageDate = moment(message.Date).fromNow();
    //        }
    //    }
    //    $scope.$digest();
    //});

    //UserHub.initialized.then(function () {
    //    getUsersChats();
    //});

});