angular.module('Chat')
.controller("LoginCtrl", function ($scope, $http, $ionicPopup, UserContext, UserHub) {

    $scope.model = { login : '' }
    
    $scope.submit = function () {
        UserHub.login($scope.model.login).done(function (result) {
            if (result && result.Id) {
                UserContext.setUser(result);
                $ionicPopup.alert({
                    title: 'Success',
                    template: 'You have been logged in'
                });
            }
        }).fail(function(result) {
            $ionicPopup.alert({
                title: 'Error',
                template: 'Wrong user login'
            });
        });
    }
});