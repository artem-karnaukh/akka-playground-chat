angular.module('Chat')
.controller("UserSearchCtrl", function ($scope, $http, UserContext, UserHub) {


    $scope.users = [];

    $scope.searchModel = {
        login: ''
    };

    $scope.add = function (user) {
        var loggedInUser = UserContext.getUser();
        if (!loggedInUser) {
            return;
        }

        UserHub.addToContactList(loggedInUser.Id, user.Id).done(function (response) {
            $scope.$apply(function () {
                for (var i = 0; i < $scope.users.length; i++) {
                    var item = $scope.users[i];
                    if (item.Id == response.TargetUserId) {
                        item.IsAlreadyAdded = true;
                    }
                }
            });
        });
    };

    $scope.$watch('searchModel.login', function (newValue, oldValue) {
        if (!newValue) {
            $scope.users = [];
            return;
        }
        var loggedInUser = UserContext.getUser();
        UserHub.search(loggedInUser.Id, newValue).done(function (data) {
            $scope.$apply(function () {
                $scope.users = data;
            })
        });
    });

});