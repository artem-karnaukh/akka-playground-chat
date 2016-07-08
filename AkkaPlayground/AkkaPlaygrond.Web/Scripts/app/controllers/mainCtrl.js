angular.module('Chat')
.controller("MainCtrl", function ($rootScope, $scope, UserContext, rootRef, $firebaseObject) {
    

    
    function init() {
        var loggedInUser = UserContext.getUser();
        if (loggedInUser) {
            var ref = rootRef.child('user-badges').child(loggedInUser.Id).child('UnReadCount');
            $rootScope.chatBadges = $firebaseObject(ref);
        }
    }

    init();
});