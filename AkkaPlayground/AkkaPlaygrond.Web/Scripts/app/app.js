angular.module('Chat', ['ionic', 'SignalR'])
.run(function ($ionicPlatform) {
    $ionicPlatform.ready(function () {
        if (window.StatusBar) {
            StatusBar.styleLightContent();
        }
    });
})

.config(function ($stateProvider, $urlRouterProvider) {
    $stateProvider

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

   .state('tab.chat', {
       url: '/chat?userId',
       views: {
           'tab-contacts': {
               templateUrl: 'chat',
               controller: 'ChatCtrl'
           }
       }
   })

    .state('tab.login', {
        url: '/login',
        views: {
            'tab-login': {
                templateUrl: 'account/login',
                controller: 'LoginCtrl'
            }
        }
    })

    .state('tab.register', {
        url: '/register',
        views: {
            'tab-register': {
                templateUrl: 'account/register',
                controller: 'RegisterCtrl'
            }
        }
    })

    $urlRouterProvider.otherwise('/tab/user-search');
});

$(function () {
    angular.bootstrap(document, ['Chat']);
});
