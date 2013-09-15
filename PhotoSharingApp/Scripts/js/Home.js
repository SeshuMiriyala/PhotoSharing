﻿var loginCtrl = sharingApp.controller('loginCtrl', ['$scope', '$http', '$location', 'UserService', '$cookieStore', function (scope, $http, $location, user, $cookieStore) {
    scope.toggleSignIn = function() {
        scope.IsSignInVisible = !scope.IsSignInVisible;
    };
    scope.login = function () {
        var x = btoa(scope.username + ':' + scope.password);
        var config = { method: 'POST', url: '/api/home/login', data: { userName: scope.username, password: scope.password }, withCredentials: true, headers: { 'Content-Type': 'application/x-www-form-urlencoded, application/xml, application/json', 'Authorization': 'Basic ' + x, 'accept': "application/json" } };
        AjaxCall(config
        ,function (data, status, headers) {
            // succefull login
            var result = atob(data);
            if ("0" == result) {
                var username = scope.username;
                scope.IsLogged = true;
                $cookieStore.put('user', username);
            }
            else if ("1" == result) {
                alert('Session expired.');
                scope.logout();
            }
            else if ("2" == result) {
                alert("Login failed.");
                scope.logout();
            }
        }
        , function (data, status, headers) {
            scope.IsLogged = false;
            $cookieStore.remove('user');
        }
        , $http);
    };
    scope.IsSignInVisible = false;
    
    scope.init = function () {
        scope.IsUserLoggedIn();
    };

    scope.IsUserLoggedIn = function () {
        user.getStatus(function (data) {
            var result = atob(data);
            if ("0" == result) {
                scope.IsLogged = true;
                return true;
            }
            else {
                scope.username = '';
                scope.password = '';
                $cookieStore.remove('user');
                scope.IsLogged = false;
                return false;
            }
        }, function(data) {
            scope.username = '';
            scope.password = '';
            $cookieStore.remove('user');
            scope.IsLogged = false;
            return false;
        });
    };
    scope.CheckLogin = function () {
        user.getStatus(function(data) {
            var result = atob(data);
            if ("0" == result) {
                alert("Logged In");
            }
            else if ("1" == result) {
                alert('Session expired.');
            }
            else if ("2" == result) {
                alert("Login failed.");
            }
        }, function() {
            alert('LoggedOut');
        });
    };
    scope.getUserName = function () {
        if (undefined == $cookieStore.get('user'))
            return '';
        return $cookieStore.get('user');
    };
    scope.logout = function () {
        scope.username = '';
        scope.password = '';
        scope.IsLogged = false;
        scope.IsSignInVisible = false;
        $cookieStore.remove('user');
    };
    scope.signup = function () {
        $location.path('/signup');
    };
}]);
sharingApp.directive('focusMe', function ($timeout, $parse) {
    return {
        //scope: true,   // optionally create a child scope
        link: function (scope, element, attrs) {
            var model = $parse(attrs.focusMe);
            scope.$watch(model, function (value) {
                if (value === true) {
                    $timeout(function () {
                        element[0].focus();
                    });
                }
            });
        }
    };
});

function AjaxCall(config, successCallback, errorCallback, $http) {
    $http(config).success(successCallback).error(errorCallback);
}
