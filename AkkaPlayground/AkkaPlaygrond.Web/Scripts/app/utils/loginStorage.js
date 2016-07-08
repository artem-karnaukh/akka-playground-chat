angular.module('Chat')
.factory('loginStorage', function () {

    var login = undefined;

    function getLogin() {
        return login;
    };

    function setLogin(argLogin) {
        login = argLogin;
    };

    function clearLogin() {
        login = undefined;
    };

    return {
        getLogin: getLogin,
        setLogin: setLogin,
        clearLogin: clearLogin
    }
});
