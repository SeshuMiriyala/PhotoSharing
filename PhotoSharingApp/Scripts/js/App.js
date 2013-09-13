var sharingApp = angular.module("sharingApp", ["ngResource", "ngCookies"], function($httpProvider) {

    $httpProvider.defaults.transformRequest = function(data, headersGetter) {
        return btoa(JSON.stringify(data));
    };

    //$httpProvider.defaults.transformResponse = function(data, headersGetter) {
    //    return JSON.parse(atob(data));
    //};
}).
    config(function ($routeProvider) {
        $routeProvider.
            when('/main', { templateUrl: 'partials/home.html', controller: 'mainCtrl' }).
            when('/', { controller: 'loginCtrl', templateUrl: 'partials/home.html' }).
            otherwise({ redirectTo: '/' });
    });
sharingApp.factory('UserService', [function () {
    var sdo = {
        isLogged: false,
        username: ''
    };
    return sdo;
}]);