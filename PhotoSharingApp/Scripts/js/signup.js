var registerCtrl = sharingApp.controller('registerCtrl', ['$scope', '$http', '$location', 'UserService', '$cookieStore', '$route', function (scope, $http, $location, user, $cookieStore, $route) {
    scope.register = function() {
        if (scope.user.password == scope.user.confirmPassword) {
            var config = { method: 'POST', url: '/api/home/register', data: { userName: scope.user.username, password: scope.user.password, email: scope.user.email, firstName: scope.user.firstName, middleName: scope.user.middleName, lastName: scope.user.lastName }, withCredentials: true, headers: { 'Content-Type': 'application/x-www-form-urlencoded, application/xml, application/json', 'Authorization': 'Basic ', 'accept': "application/json" } };
            AjaxCall(config
            , function (data, status, headers) {
                // succefull login
                var username = scope.user.username;
                scope.IsLogged = true;
                $cookieStore.put('user', username);
                $location.path('/').ref();
            }
            , function (data, status, headers) {
                scope.IsLogged = false;
                $cookieStore.remove('user');
            }
            , $http);
        }
    };
}]);
sharingApp.directive('uniqueEmail', ['$http', function($http) {
    return {
        require: 'ngModel',
        restrict: 'A',
        link: function(scope, el, attrs, ctrl) {
            el.bind('blur', function() {
                var config = { method: 'POST', url: '/api/home/IsValidEmail', data: { key: ctrl.$viewValue }, withCredentials: true, headers: { 'Content-Type': 'application/x-www-form-urlencoded, application/xml, application/json', 'Authorization': 'Basic ', 'accept': "application/json" } };
                AjaxCall(config, function(data, status, headers) {
                    var result = atob(data);
                    if ("0" == result) {
                        ctrl.$setValidity('uniqueEmail', true);
                    } else if ("1" == result) {
                        ctrl.$setValidity('uniqueEmail', false);
                    }
                }, function(data, status, headers) {
                    ctrl.$setValidity('uniqueEmail', false);
                }, $http);
            });
        }
    };
}]);
sharingApp.directive('uniqueUsername', ['$http', function($http) {
    return {
        require: 'ngModel',
        restrict: 'A',
        link: function(scope, el, attrs, ctrl) {
            el.bind('blur', function() {
                var config = { method: 'POST', url: '/api/home/IsValidUserName', data: { key: ctrl.$viewValue }, withCredentials: true, headers: { 'Content-Type': 'application/x-www-form-urlencoded, application/xml, application/json', 'Authorization': 'Basic ', 'accept': "application/json" } };
                AjaxCall(config, function(data, status, headers) {
                    var result = atob(data);
                    if ("0" == result) {
                        ctrl.$setValidity('uniqueUsername', true);
                    } else if ("1" == result) {
                        ctrl.$setValidity('uniqueUsername', false);
                    }
                }, function(data, status, headers) {
                    ctrl.$setValidity('uniqueUsername', false);
                }, $http);
            });
        }
    };
}]);