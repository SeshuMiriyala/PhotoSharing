var sharingApp = angular.module("sharingApp", ["ngResource", "ngCookies"], function ($httpProvider, $locationProvider, $routeProvider) {

    $httpProvider.defaults.transformRequest = function (data, headersGetter) {
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
sharingApp.service('UserService', function ($cookieStore, $http) {
    return {
        getStatus: function(successCallback, errorCallback) {
            if (undefined != $cookieStore.get('user')) {
                var x = btoa($cookieStore.get('user'));
                var config1 = { method: 'POST', url: '/api/home/login', data: { userName: $cookieStore.get('user'), password: '' }, withCredentials: true, headers: { 'Content-Type': 'application/x-www-form-urlencoded, application/xml, application/json', 'Authorization': 'Basic ' + x, 'accept': "application/json" } };
                $http(config1)
                    .success(function(data, status, headers, config1) {
                        successCallback(data);
                    })
                    .error(function(data, status, headers, config1) {
                        errorCallback(data);
                    });
            }
        }  
    };
});
sharingApp.service('sharedProperties', function () {
        var isLogged = false;

        return {
            GetIsLogged: function () {
                return isLogged;
            },
            SetIsLogged: function (value) {
                isLogged = value;
            }
        };
    });