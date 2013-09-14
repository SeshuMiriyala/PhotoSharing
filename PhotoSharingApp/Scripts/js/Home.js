var loginCtrl = sharingApp.controller('loginCtrl', ['$scope', '$http', '$location', 'UserService', '$cookieStore', function (scope, $http, $location, User, $cookieStore) {
    scope.toggleSignIn = function() {
        scope.IsSignInVisible = !scope.IsSignInVisible;
    };
    scope.login = function () {
        var x = btoa(scope.username + ':' + scope.password);
        var config = { method: 'POST', url: '/api/home/login', data: { userName: scope.username, password: scope.password }, withCredentials: true, headers: { 'Content-Type': 'application/x-www-form-urlencoded, application/xml, application/json', 'Authorization': 'Basic ' + x, 'accept': "application/json" } };
        AjaxCall(config
        ,function (data, status, headers, config) {
            if ('200' == status) {
                // succefull login
                User.isLogged = true;
                scope.IsSignInVisible = !scope.IsSignInVisible;
                User.username = scope.username;
                $cookieStore.put('user', User);
            }
            else {
                User.isLogged = false;
                User.username = '';
            }
        }
        ,function (data, status, headers, config) {
            User.isLogged = false;
            User.username = '';
        }
        , $cookieStore, $http, scope);
    };
    scope.IsSignInVisible = false;

    scope.IsUserLoggedIn = function () {
        if (undefined == $cookieStore.get('user'))
            return false;
        else {

            return $cookieStore.get('user').isLogged;
        }
    };
    scope.CheckLogin = function() {
        var x = btoa(scope.username + ':' + scope.password);
        var config = { method: 'POST', url: '/api/home', data: { userName: scope.username, password: scope.password }, withCredentials: true, headers: { 'Content-Type': 'application/x-www-form-urlencoded, application/xml, application/json', 'Authorization': 'Basic ' + x, 'accept': "application/json" } };
        window.AjaxCall(config
        ,function (data, status, headers, config) {
            if ('200' == status) {
                alert('LoggedIn');
            }
            else {
                alert('LoggedOut');
            }
        }
        ,function (data, status, headers, config) {
            alert('LoggedOut');
        }
        , $cookieStore, $http, scope);
    };
    scope.getUserName = function () {
        if (undefined == $cookieStore.get('user'))
            return '';
        return $cookieStore.get('user').username;
    };
    scope.logout = function () {
        User.isLogged = false;
        User.username = '';
        scope.username = '';
        scope.password = '';
        $cookieStore.remove('user');
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

function AjaxCall(config, successCallback, errorCallback, $cookieStore, $http, scope) {
    if (config['url'].indexOf('login') == -1) {
        if (undefined != $cookieStore.get('user')) {
            var x = btoa($cookieStore.get('user').username);
            var config1 = { method: 'POST', url: '/api/home/login', data: { userName: $cookieStore.get('user').username, password: '' }, withCredentials: true, headers: { 'Content-Type': 'application/x-www-form-urlencoded, application/xml, application/json', 'Authorization': 'Basic ' + x, 'accept': "application/json" } };
            $http(config1)
                .success(function(data, status, headers, config1) {
                    if ('200' == status) {
                        $http(config).success(successCallback).error(errorCallback);
                    } else {
                        scope.logout();
                        return;
                    }
                })
                .error(function(data, status, headers, config1) {
                    scope.logout();
                    return;
                });
        }
    }
    else
        $http(config).success(successCallback).error(errorCallback);
}
