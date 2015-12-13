angular.module('Chat')
.controller("ContactsCtrl", function ($scope, $http, $state, UserContext, UserHub) {

    $scope.users = [];

    function getUsersContacts() {
        var loggedInUser = UserContext.getUser();
        if (!loggedInUser) {
            return;
        }

        UserHub.getUsersContacts(loggedInUser.Id).done(function (data) {
            $scope.$apply(function () {
                $scope.users = data;
            })
        });
    }

    $scope.openChat = function (user) {
        $state.go('tab.chat', { userId: user.Id });
    }

    UserHub.initialized.then(function () {
        getUsersContacts();
    });

});