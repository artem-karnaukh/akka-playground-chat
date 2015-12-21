angular.module('Chat')
.controller("ContactsCtrl", function ($scope, $http, $state, UserContext, UserHub) {

    $scope.users = [];

    $scope.doRefresh = function () {
        getUsersContacts();
    };

    function getUsersContacts() {
        var loggedInUser = UserContext.getUser();
        if (!loggedInUser) {
            return;
        }

        UserHub.getUsersContacts(loggedInUser.Id).done(function (data) {
            $scope.$apply(function () {
                $scope.users = data;
                $scope.$broadcast('scroll.refreshComplete');
            })
        });
    }

    $scope.openChat = function (user) {
        $state.go('tab.user-chat-details', { userId: user.Id });
    }

    UserHub.initialized.then(function () {
        getUsersContacts();
    });

});