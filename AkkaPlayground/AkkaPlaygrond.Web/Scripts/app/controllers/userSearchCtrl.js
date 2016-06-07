angular.module('Chat')
.controller("UserSearchCtrl", function ($scope, $http, UserContext, UserHub, UserService) {


    $scope.users = [];

    $scope.searchModel = {
        login: ''
    };

    $scope.add = function (user) {
        var loggedInUser = UserContext.getUser();
        if (!loggedInUser) {
            return;
        }

        UserService.addToContactList(loggedInUser.Id, user.Id);
        //    .then(function (response) {
        //    $scope.$apply(function () {
        //        for (var i = 0; i < $scope.users.length; i++) {
        //            var item = $scope.users[i];
        //            if (item.Id == response.TargetUserId) {
        //                item.IsAlreadyAdded = true;
        //            }
        //        }
        //    });
        //});
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