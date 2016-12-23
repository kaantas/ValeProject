
var myApp = angular.module('myApp', []);

myApp.controller('biletController', function ($scope, $http) {
    $scope.v = false;

    $scope.go = function () {
        $scope.v = true;
        $scope.biletler = [];
        $http.get("/Home/GetBilet")
            .then(function (result) {
                $scope.biletler = result.data;
            });
    }

});

$scope.$apply();