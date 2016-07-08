angular.module('Chat')
.controller("LoginCtrl", function ($scope, $state, $ionicPopup, UserContext, UserService, loginStorage) {

    $scope.model = { login : '' }
    
    $scope.submit = function () {
        UserService.login($scope.model.login).then(function (result) {
            if (result.data && result.data.Id) {
                UserContext.setUser(result.data);
                $state.go('tab.contacts')
            }
        }, function (result) {
            loginStorage.setLogin($scope.model.login);
            $state.go('register');
        });
    }
});