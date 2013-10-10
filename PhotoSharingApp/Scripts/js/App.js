var sharingApp = angular.module("sharingApp", ["ngResource", "ngCookies", "firebase"], function ($httpProvider) {

    $httpProvider.defaults.transformRequest = function (data) {
        return btoa(JSON.stringify(data));
    };

    //$httpProvider.defaults.transformResponse = function(data, headersGetter) {
    //    return JSON.stringify(atob(data));
    //};
    //$locationProvider.html5Mode(true).hashPrefix('!');
}).
    config(function ($routeProvider) {
        $routeProvider.
            when('/main', { templateUrl: 'partials/home.html', controller: 'mainCtrl' }).
            when('/', { controller: 'loginCtrl', templateUrl: 'partials/home.html' }).
            when('/signup', { controller: 'registerCtrl', templateUrl: 'partials/signup.html' }).
            otherwise({ redirectTo: '/' });
    });
sharingApp.config(['$httpProvider', function($httpProvider) {
    var httpIntercepter = ['$cookieStore', '$rootScope', '$q', function ($cookieStore, $rootScope, $q) {
        //return function() {
        //    if (undefined != $cookieStore.get('user')) {
        //        var x = btoa($cookieStore.get('user'));
        //        var config1 = { method: 'POST', url: '/api/home/login', data: { userName: $cookieStore.get('user'), password: '' }, withCredentials: true, headers: { 'Content-Type': 'application/x-www-form-urlencoded, application/xml, application/json', 'Authorization': 'Basic ' + x, 'accept': "application/json" } };
        //        $http(config1)
        //            .success(function(data) {
        //                return data;
        //            })
        //            .error(function() {
        //                $rootScope.$broadcast('event:auth-loginRequired');
        //                return $q.reject(response);
        //            });
        //    }
        //};
        function success(response) {
            return response;
        }

        function error(response) {
            if (response.status === 401 && !response.config.ignoreAuthModule) {
                var deferred = $q.defer();
                $rootScope.$broadcast('event:auth-loginRequired');
                return deferred.promise;
            }
            // otherwise, default behaviour
            return $q.reject(response);
        }

        return function (promise) {
            return promise.then(success, error);
        };
    }];
    $httpProvider.responseInterceptors.push(httpIntercepter);
}]);
sharingApp.service('authService', ['$rootScope', function($rootScope) {
    return {
        loginConfirmed: function () {
            $rootScope.IsLogged = true;
            $rootScope.$broadcast('event:auth-loginConfirmed');
        }
    };
}]);
sharingApp.factory('dataService', ['$http', function($http) {
    var serviceBase = '/api/dataservice/',
        dataFactory = {};

    dataFactory.checkUniqueValue = function(property, value) {
        return $http.get(serviceBase + 'IsValid?property=' +
            property + '&value=' + escape(value)).then(
                function(results) {
                    return atob(results.data);
                });
    };
    return dataFactory;
}]);

sharingApp.directive('sharingContent', ['$cookieStore', function ($cookieStore) {
    return {
        //scope: true,   // optionally create a child scope
        link: function (scope, element) {
            var login = element.find('#login-holder');
            scope.$on('event:auth-loginRequired', function () {
                $cookieStore.remove('user');
                login.slideDown('slow', function () {
                });
            });
            scope.$on('event:auth-loginConfirmed', function () {
                login.slideUp();
            });
        }
    };
}]);