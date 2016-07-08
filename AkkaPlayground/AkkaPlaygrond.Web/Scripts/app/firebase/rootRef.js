angular.module('Chat')
.constant('FirebaseUrl', 'https://akkaplayground.firebaseio.com')
.service('rootRef', function (FirebaseUrl) {
    return new Firebase(FirebaseUrl);
});