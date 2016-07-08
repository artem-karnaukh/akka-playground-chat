angular.module('Chat')
.controller("RegisterCtrl", function ($scope, $state, $ionicPopup, UserHub, UserService, UserContext, loginStorage) {

    var registeringUserId = '';
    $scope.registerModel = { userName: "" };
    var login = loginStorage.getLogin();

    UserHub.initialized.then(function () {
        UserHub.subscribe('userJoined', function (id, login, userName) {
            if (id == registeringUserId) {
                $ionicPopup.alert({
                    title: 'Success',
                    template: 'User ' + login + ' was registered.'
                }).then(function (res) {
                    clearRegisterModel();
                }).then(function () {
                    UserContext.setUser({ Id: id, Login: login, Name: userName});
                    $state.go('tab.contacts')
                });

            };
        });
    });
    

    function clearRegisterModel() {
        $scope.registerModel.userName = '';
    };

    $scope.register = function () {
        var data = { Login: login, UserName: $scope.registerModel.userName };
        UserService.register(data).then(function (result) {
            registeringUserId = result.data;
        });
    }
});



