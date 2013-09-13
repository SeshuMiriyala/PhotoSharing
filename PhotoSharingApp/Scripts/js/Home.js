var loginCtrl = sharingApp.controller('loginCtrl', ['$scope', '$http', '$location', 'UserService', '$cookieStore', function (scope, $http, $location, User, $cookieStore) {
    scope.toggleSignIn = function() {
        scope.IsSignInVisible = !scope.IsSignInVisible;
    };
    scope.login = function() {
        var config = { method: 'POST', url: '/api/home/login', data: $.param({ userName: scope.username, password: scope.password }), withCredentials: true, headers: { 'Content-Type': 'application/x-www-form-urlencoded' } };

        $http(config)
        .success(function (data, status, headers, config) {
            if ('true' == data) {
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
        })
        .error(function (data, status, headers, config) {
            User.isLogged = false;
            User.username = '';
        });
    };
    scope.IsSignInVisible = false;

    scope.IsUserLoggedIn = function () {
        if (undefined == $cookieStore.get('user'))
            return false;
        return $cookieStore.get('user').isLogged;
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