
var myApp = angular.module('myApp', []);

myApp.controller('biletController', function ($scope, $http) {
    console.log("function içinde");
    $scope.biletler = {};
    $http.get("/Home/GetBilet")
        .then(function(result) {
            $scope.biletler = result;
            console.log("error yok");
        });
});
