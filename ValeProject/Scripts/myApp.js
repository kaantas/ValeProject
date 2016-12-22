
var myApp = angular.module('myApp', []);

myApp.controller('biletController', function ($scope, $http) {
    $scope.biletler = [];
    $http.get("/Home/GetBilet")
        .then(function(result) {
            $scope.biletler = result.data;
        });
});
