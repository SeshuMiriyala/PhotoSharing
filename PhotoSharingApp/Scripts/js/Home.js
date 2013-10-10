sharingApp.controller('loginCtrl', ['$scope', '$http', '$location', '$cookieStore', 'authService', '$rootScope', '$q', function (scope, $http, $location, $cookieStore, authService, $rootScope, $q) {
    scope.toggleSignIn = function() {
        scope.IsSignInVisible = !scope.IsSignInVisible;
    };
    scope.login = function() {
        if (undefined != $cookieStore.get('user'))
            $cookieStore.remove('user');
        var config = { method: 'POST', url: '/api/home/login', data: { userName: scope.username, password: scope.password }, withCredentials: true, headers: { 'Content-Type': 'application/x-www-form-urlencoded, application/xml, application/json', 'Authorization': 'Basic ', 'accept': "application/json" } };
        AjaxCall(config,
            function (data) {
                var result = atob(data);
                if ("0" == result) {
                    $cookieStore.put('user', scope.username);
                    authService.loginConfirmed();
                } else if ("1" == result) {
                    $rootScope.$broadcast('event:auth-loginRequired');
                    return $q.reject(data);
                }
            }
            ,function() {
                $rootScope.$broadcast('event:auth-loginRequired');
                return $q.reject(response);
            }, $http);
    };
    $rootScope.IsLogged = (undefined != $cookieStore.get('user'));
    scope.IsSignInVisible = false;
    //scope.init = function () {
    //    scope.IsUserLoggedIn();
    //};

    //scope.IsUserLoggedIn = function () {
    //    user.getStatus(function (data) {
    //        var result = atob(data);
    //        if ("0" == result) {
    //            scope.IsLogged = true;
    //            return true;
    //        }
    //        else {
    //            scope.username = '';
    //            scope.password = '';
    //            $cookieStore.remove('user');
    //            scope.IsLogged = false;
    //            return false;
    //        }
    //    }, function() {
    //        scope.username = '';
    //        scope.password = '';
    //        $cookieStore.remove('user');
    //        scope.IsLogged = false;
    //        return false;
    //    });
    //};
    //scope.CheckLogin = function () {
    //    user.getStatus(function(data) {
    //        var result = atob(data);
    //        if ("0" == result) {
    //            alert("Logged In");
    //        }
    //        else if ("1" == result) {
    //            alert('Session expired.');
    //        }
    //        else if ("2" == result) {
    //            alert("Login failed.");
    //        }
    //    }, function() {
    //        alert('LoggedOut');
    //    });
    //};
    scope.getUserName = function () {
        if (undefined == $cookieStore.get('user'))
            return '';
        return $cookieStore.get('user');
    };
    scope.reset = function() {
        var login = $('#login-holder');
        login.slideUp();
    };
    scope.logout = function () {
        scope.username = '';
        scope.password = '';
        $rootScope.IsLogged = false;
        scope.IsSignInVisible = false;
        if (undefined != $cookieStore.get('user')) {
            var config = { method: 'POST', url: '/api/home/logout', data: { userName: $cookieStore.get('user'), password: '' }, withCredentials: true, headers: { 'Content-Type': 'application/x-www-form-urlencoded, application/xml, application/json', 'Authorization': 'Basic ', 'accept': "application/json" } };
            AjaxCall(config,
                function(data) {
                    $rootScope.$broadcast('event:auth-loginRequired');
                },function() {
                    $rootScope.$broadcast('event:auth-loginRequired');
                    return $q.reject(response);
                }, $http);
            $cookieStore.remove('user');
        }
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

sharingApp.controller('homeCtrl', ['$scope', '$cookieStore', '$rootScope', 'angularFire', function ($scope, $cookieStore, $rootScope, angularFire) {
    var url = 'https://photosharingapp.firebaseio.com/posts';
    $scope.posts = [];
    var myDataRef = new Firebase('https://photosharingapp.firebaseio.com/posts');
    angularFire(myDataRef, $scope, "posts").then(function () {
        $scope.postMsg = function () {
            if (undefined == $cookieStore.get('user'))
                $rootScope.$broadcast('event:auth-loginRequired');
            else {
                $scope.newPost.username = $cookieStore.get('user');
                $scope.posts.push({ username: $scope.newPost.username, message: $scope.newPost.message });
                $scope.newPost.message = '';
            }
        };
        //myDataRef.on('child_added', function (snapshot) {
        //    var post = snapshot.val();
        //    posts.push(post);
        //});
    });
}]);