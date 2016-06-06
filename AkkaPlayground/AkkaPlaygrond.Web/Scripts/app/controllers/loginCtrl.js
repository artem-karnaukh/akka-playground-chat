angular.module('Chat')
.controller("LoginCtrl", function ($scope, $state, $ionicPopup, UserContext, UserService) {

    $scope.model = { login : '' }
    
    $scope.goRegister = function () {
        $state.go('register');
    };

    $scope.submit = function () {
        UserService.login($scope.model.login).then(function (result) {
            if (result.data && result.data.Id) {
                UserContext.setUser(result.data);
                $state.go('tab.contacts')
            }
        },function (result) {
            $ionicPopup.alert({
                title: 'Error',
                template: 'Wrong user login'
            });
        });
    }
});