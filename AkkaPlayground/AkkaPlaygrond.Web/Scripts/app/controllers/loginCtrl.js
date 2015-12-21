angular.module('Chat')
.controller("LoginCtrl", function ($scope, $http, $state,  $ionicPopup, UserContext, UserHub) {

    $scope.model = { login : '' }
    
    $scope.goRegister = function () {
        $state.go('register');
    };

    $scope.submit = function () {
        UserHub.login($scope.model.login).done(function (result) {
            if (result && result.Id) {
                UserContext.setUser(result);
                $state.go('tab.contacts')
            }
        }).fail(function(result) {
            $ionicPopup.alert({
                title: 'Error',
                template: 'Wrong user login'
            });
        });
    }
});