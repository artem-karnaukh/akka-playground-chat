angular.module('Chat')
.controller("ContactsCtrl", function ($scope, $state, UserContext, UserService) {

    $scope.users = [];

    $scope.doRefresh = function () {
        getUsersContacts();
    };

    function getUsersContacts() {
        var loggedInUser = UserContext.getUser();
        if (!loggedInUser) {
            return;
        }
        UserService.getUserContacts(loggedInUser.Id).then(function (response) {
            $scope.users = response.data;
        }).finally(function() {
            $scope.$broadcast('scroll.refreshComplete');
        });
    }

    $scope.openChat = function (user) {
        $state.go('tab.user-chat-details', { userId: user.Id });
    }
    

    getUsersContacts();

});