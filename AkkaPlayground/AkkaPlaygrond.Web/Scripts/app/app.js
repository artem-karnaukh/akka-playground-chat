angular.module('Chat', ['ionic', 'SignalR'])
.run(function ($ionicPlatform, $rootScope, UserContext, $state, UserHub) {
    $ionicPlatform.ready(function () {
        if (window.StatusBar) {
            StatusBar.styleLightContent();
        }
    });

    var appStarted = false;

    $rootScope.$on('$locationChangeStart', function (event, toState, toParams, fromState, fromParams) {
        var user = UserContext.getUser();
        if (appStarted) {
            if (!user) {
                event.preventDefault();
                $state.go('login');
            }
        } else {
            event.preventDefault();
            appStarted = true;
            if (user) {
                $state.go('tab.chats');
                UserHub.initialized.then(function () {
                    UserHub.joinSignalRChatGroups(user.Id);
                });
            } else {
                $state.go('login');
            }
        }

    });
})

.config(function ($stateProvider, $urlRouterProvider) {
    $stateProvider

      .state('login', {
          url: "/login",
          templateUrl: "account/login",
          controller: 'LoginCtrl'
      })

      .state('register', {
          templateUrl: 'account/register',
          controller: "RegisterCtrl"
      })

      .state('tab', {
          url: "/tab",
          abstract: true,
          templateUrl: "home/tabs"
      })

    .state('tab.user-search', {
        url: '/user-search',
        views: {
            'tab-user-search': {
                templateUrl: 'contacts',
                controller: 'UserSearchCtrl'
            }
        }
    })

    .state('tab.contacts', {
        url: '/contacts',
        views: {
            'tab-contacts': {
                templateUrl: 'contacts/added',
                controller: 'ContactsCtrl'
            }
        }
    })

   .state('tab.user-chat-details', {
       url: '/user-chat-details?userId',
       cache:false,
       views: {
           'tab-contacts': {
               templateUrl: 'chat',
               controller: 'ChatCtrl'
           }
       }
   })

   .state('tab.chats', {
       url: '/chats-list',
       views: {
           'tab-chats': {
               templateUrl: 'chat/list',
               controller: 'ChatsListCtrl'
           }
       }
   })

   .state('tab.chat-details', {
       url: '/chat-details?chatId',
       views: {
           'tab-chats': {
               templateUrl: 'chat',
               controller: 'ChatCtrl'
           }
       }
   })

    $urlRouterProvider.otherwise('/login');
});

$(function () {
    angular.bootstrap(document, ['Chat']);
});
