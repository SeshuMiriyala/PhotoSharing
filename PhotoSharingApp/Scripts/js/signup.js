sharingApp.controller('registerCtrl', ['$scope', '$http', '$location', 'UserService', '$cookieStore', function (scope, $http, $location, user, $cookieStore) {
    scope.register = function () {
        if (scope.user.password == scope.user.confirmPassword) {
            var config = { method: 'POST', url: '/api/home/register', data: { userName: scope.user.username, password: scope.user.password, email: scope.user.email, firstName: scope.user.firstName, middleName: scope.user.middleName, lastName: scope.user.lastName }, withCredentials: true, headers: { 'Content-Type': 'application/x-www-form-urlencoded, application/xml, application/json', 'Authorization': 'Basic ', 'accept': "application/json" } };
            AjaxCall(config
            , function () {
                // succefull login
                $cookieStore.put('user', scope.user.username);
                $location.path('/');
                scope.IsUserLoggedIn();
            }
            , function () {
                $cookieStore.remove('user');
            }
            , $http);
        } else {
            scope.isPasswordValid = false;
        }
    };
    scope.isUsernameValid = true;
    scope.usernameInvalidMsg = '';
    scope.isEmailValid = true;
    scope.emailInvalidMsg = '';
    scope.isPasswordValid = true;
    scope.home = function () {
        $location.path('/');
    };
    
}]);
sharingApp.directive('uniqueEmail', ['$http', function ($http) {
    return {
        require: 'ngModel',
        restrict: 'A',
        link: function (scope, el, attrs, ctrl) {
            el.bind('blur', function () {
                var config = { method: 'POST', url: '/api/home/IsValidEmail', data: { key: ctrl.$viewValue }, withCredentials: true, headers: { 'Content-Type': 'application/x-www-form-urlencoded, application/xml, application/json', 'Authorization': 'Basic ', 'accept': "application/json" } };
                AjaxCall(config, function (data) {
                    var result = atob(data);
                    if ("0" == result) {
                        ctrl.$setValidity('uniqueEmail', true);
                        scope.isEmailValid = true;
                        scope.emailInvalidMsg = '';
                    } else if ("1" == result) {
                        ctrl.$setValidity('uniqueEmail', false);
                        scope.isEmailValid = (false || ctrl.$error.required || ctrl.$error.email);
                        scope.emailInvalidMsg = 'Email already used';
                    }
                }, function () {
                    ctrl.$setValidity('uniqueEmail', false);
                    scope.isEmailValid = (false || ctrl.$error.required || ctrl.$error.email);
                    scope.emailInvalidMsg = 'Error in validating email. Please try again';
                }, $http);
            });
        }
    };
}]);
sharingApp.directive('uniqueUsername', ['$http', function ($http) {
    return {
        require: 'ngModel',
        restrict: 'A',
        link: function ($scope, el, attrs, ctrl) {
            el.bind('blur', function () {
                var config = { method: 'POST', url: '/api/home/IsValidUserName', data: { key: ctrl.$viewValue }, withCredentials: true, headers: { 'Content-Type': 'application/x-www-form-urlencoded, application/xml, application/json', 'Authorization': 'Basic ', 'accept': "application/json" } };
                AjaxCall(config, function (data) {
                    var result = atob(data);
                    if ("0" == result) {
                        ctrl.$setValidity('uniqueUsername', true);
                        $scope.isUsernameValid = true;
                        $scope.usernameInvalidMsg = '';
                    } else if ("1" == result) {
                        ctrl.$setValidity('uniqueUsername', false);
                        $scope.isUsernameValid = (false || ctrl.$error.required);
                        $scope.usernameInvalidMsg = 'Username already exists';
                    }
                }, function () {
                    ctrl.$setValidity('uniqueUsername', false);
                    $scope.isUsernameValid = (false || ctrl.$error.required);
                    $scope.usernameInvalidMsg = 'Error in validating username. Please try again';
                }, $http);
            });
        }
    };
}]);