angular.module('Chat')
.service('UserService', function ($q, $http) {

    function login(login) {
        var url = 'api/user/login';
        var model = { login: login };

        return $http({
            method: 'POST', url: url, data: JSON.stringify(model)
        });
    };

    function register(data) {
        var url = 'api/user/register';
        return $http({
            method: 'POST', url: url, data: JSON.stringify(data)
        });
    }

    function search(currentUserId, value) {
        var url = 'api/user/search';
        var data = {
            currentUserId: currentUserId,
            searchString: value
        };
        return $http({
            method: 'POST', url: url, data: JSON.stringify(data)
        });
    }


    function addToContactList(userId, targetUserId) {
        var url = 'api/user/AddToContactList';
        var data = {
            userId: userId,
            targetUserId: targetUserId
        };
        return $http({
            method: 'POST', url: url, data: JSON.stringify(data)
        });
    }

    return {
        login: login,
        register: register,
        search: search,
        addToContactList: addToContactList
    };
});