angular.module('Chat')
.controller("UserSearchCtrl", function ($scope, $http, UserContext, UserHub, UserService, $ionicPopup) {

    $scope.users = [];

    $scope.searchModel = {
        login: ''
    };

    UserHub.initialized.then(function () {
        UserHub.subscribe('userAddedToContactList', function (userid, contactId, contactUserName) {
            for (var i = 0; i < $scope.users.length; i++) {
                var item = $scope.users[i];
                if (item.Id == contactId) {
                    item.IsAlreadyAdded = true;
                }
            }
            $scope.$digest();

            $ionicPopup.alert({
                title: 'Success',
                template: 'User ' + contactUserName + ' was registered.'
            });
        });
    })

    $scope.add = function (user) {
        var loggedInUser = UserContext.getUser();
        if (!loggedInUser) {
            return;
        }
        UserService.addToContactList(loggedInUser.Id, user.Id);
    };

    $scope.$watch('searchModel.login', function (newValue, oldValue) {
        if (!newValue) {
            $scope.users = [];
            return;
        }

        var loggedInUser = UserContext.getUser();
        UserService.search(loggedInUser.Id, newValue).then(function (response) {
            $scope.users = response.data;
        });
    });

});