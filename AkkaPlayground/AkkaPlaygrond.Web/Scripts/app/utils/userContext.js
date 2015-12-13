angular.module('Chat')
.factory('UserContext', function () {

    var userStorageKey = 'user';

    function getUser() {
        var json = window.localStorage.getItem(userStorageKey);
        var user = JSON.parse(json);
        return user;
    };

    function setUser(user) {
        window.localStorage.setItem(userStorageKey, JSON.stringify(user));
    };

    function clearUser() {
        window.localStorage.removeItem(userStorageKey)
    };

    return {
        getUser: getUser,
        setUser: setUser,
        clearUser: clearUser
    }
});
