
var myApp = angular.module('myApp', []);

myApp.controller('biletController', function ($scope, $http) {
    $scope.v = false;

    $scope.go = function (seferId) {
        $scope.v = true;
        $scope.biletler = [];
        $http.get("/Home/GetBilet/"+seferId)
            .then(function (result) {
                $scope.biletler = result.data;
            });
    }

    $scope.rows = [];
    $scope.addRow = function(seferId, koltukNo) {
        $http.get("/Home/GetSeferBilgileri/"+seferId)
            .then(function (result) {
                result.data.MuavinID = koltukNo; //MuavinID = seçili koltuk numarası saklanıyor
                $scope.rows.push(result.data);
            });
    }
    
    
});
$scope.apply();