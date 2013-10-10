sharingApp.controller('registerCtrl', ['$scope', '$http', '$location', '$cookieStore', function (scope, $http, $location, $cookieStore) {
    scope.register = function () {
        if (scope.user.password == scope.user.confirmPassword) {
            var config = { method: 'POST', url: '/api/home/register', data: { userName: scope.user.username, password: scope.user.password, email: scope.user.email, firstName: scope.user.firstName, middleName: scope.user.middleName, lastName: scope.user.lastName }, withCredentials: true, headers: { 'Content-Type': 'application/x-www-form-urlencoded, application/xml, application/json', 'Authorization': 'Basic ', 'accept': "application/json" } };
            AjaxCall(config
            , function () {
                // succefull login
                $cookieStore.put('user', scope.user.username);
                $location.path('/');
                //scope.IsUserLoggedIn();
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
sharingApp.directive('wcUnique', ['dataService', function (dataService) {
    return {
        restrict: 'A',
        require: 'ngModel',
        link: function(scope, element, attrs, ctrl) {
            element.bind('blur', function() {
                if (!ctrl || !element.val()) return;
                var keyProperty = scope.$eval(attrs.wcUnique);
                var currentValue = element.val();
                dataService.checkUniqueValue(keyProperty.property, currentValue)
                    .then(function(result) {
                        if ("Username" == keyProperty.property) {
                            if ("0" == result) {
                                ctrl.$setValidity('uniqueUsername', true);
                                scope.isUsernameValid = true;
                                scope.usernameInvalidMsg = '';
                            } else if ("1" == result) {
                                ctrl.$setValidity('uniqueUsername', false);
                                scope.isUsernameValid = (false || ctrl.$error.required);
                                scope.usernameInvalidMsg = 'Username already exists';
                            }
                        } else if ("Email" == keyProperty.property) {
                            if ("0" == result) {
                                ctrl.$setValidity('uniqueEmail', true);
                                scope.isEmailValid = true;
                                scope.emailInvalidMsg = '';
                            } else if ("1" == result) {
                                ctrl.$setValidity('uniqueEmail', false);
                                scope.isEmailValid = (false || ctrl.$error.required);
                                scope.emailInvalidMsg = 'Email already exists';
                            }
                        }
                    }, function() {
                        ctrl.$setValidity('unique' + keyProperty.property, false);
                        if ("Username" == keyProperty.property) {
                            scope.isUsernameValid = (false || ctrl.$error.required);
                            scope.usernameInvalidMsg = 'Error in validating username. Please try again';
                        } else if ("Email" == keyProperty.property) {
                            scope.isUsernameValid = (false || ctrl.$error.required);
                            scope.usernameInvalidMsg = 'Error in validating email. Please try again';
                        }
                    });
            });
        }
    };
}])