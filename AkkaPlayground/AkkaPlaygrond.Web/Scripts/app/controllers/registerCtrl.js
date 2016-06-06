angular.module('Chat')
.controller("RegisterCtrl", function ($scope,  $ionicPopup, UserHub, UserService) {

    var registeringUserId = '';
    $scope.registerModel = { login: "", email: "" };

    UserHub.initialized.then(function () {
        UserHub.subscribe('userJoined', function (id, userName, email) {
            if (id == registeringUserId) {
                var alertPopup = $ionicPopup.alert({
                    title: 'Success',
                    template: 'User ' + userName + ' was registered.'
                }).then(function (res) {
                    clearRegisterModel();
                });

            };
        });
    })
    

    function clearRegisterModel() {
        $scope.registerModel.login = '';
        $scope.registerModel.email = '';
    };

    $scope.register = function () {
        var data = { UserName: $scope.registerModel.login, Email: $scope.registerModel.email };
        UserService.register(data).then(function (result) {
            registeringUserId = result.data;
        });
    }
});



