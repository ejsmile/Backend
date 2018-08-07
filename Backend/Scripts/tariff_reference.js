var app = angular.module('tariffModule', []);

app.controller('tariffCtrl', function ($scope, $http, TariffsService) {
    $scope.tariffsData = null;
    TariffsService.GetAllRecords().then(function (d) {
        $scope.tariffsData = d.data; // Success
    }, function () {
        alert('Error Occured !!!'); // Failed
    });

    $scope.total = function () {
        var total = 0;
        angular.forEach($scope.tariffsData, function (item) {
            total += item.Price;
        })
        return total;
    }

    $scope.Tariff = {
        Id: '',
        Name: '',
        Price: '',
        Category: '',
        Deleted: ''
    };

    // Reset Tariff details
    $scope.clear = function () {
        $scope.Tariff.Id = '';
        $scope.Tariff.Name = '';
        $scope.Tariff.Price = '';
        $scope.Tariff.Category = '';
        $scope.Tariff.Deleted = '';
    }

    //Add New Item
    $scope.save = function () {
        if ($scope.Tariff.Name != "" &&
            $scope.Tariff.Price != "" && $scope.Tariff.Category != "") {
            $http({
                method: 'POST',
                url: 'api/Tariff/',
                data: $scope.Tariff
            }).then(function successCallback(response) {
                // this callback will be called asynchronously
                // when the response is available
                $scope.tariffsData.push(response.data);
                $scope.clear();
                alert("Tariff Added Successfully !!!");
            }, function errorCallback(response) {
                alert("Error : " + response.data.ExceptionMessage);
            });
        }
        else {
            alert('Please Enter All the Values !!');
        }
    };

    // Edit Tariff details
    $scope.edit = function (data) {
        $scope.Tariff = { Id: data.Id, Name: data.Name, Price: data.Price, Category: data.Category, Deleted: data.Deleted };
    }

    // Cancel Tariff details
    $scope.cancel = function () {
        $scope.clear();
    }

    // Update Tariff details
    $scope.update = function () {
        if ($scope.Tariff.Name != "" &&
            $scope.Tariff.Price != "" && $scope.Tariff.Category != "") {
            $http({
                method: 'PUT',
                url: 'api/Tariff/' + $scope.Tariff.Id,
                data: $scope.Tariff
            }).then(function successCallback(response) {
                $scope.tariffsData = response.data;
                $scope.clear();
                alert("Tariff Updated Successfully !!!");
            }, function errorCallback(response) {
                alert("Error : " + response.data.ExceptionMessage);
            });
        }
        else {
            alert('Please Enter All the Values !!');
        }
    };

    // Delete Tariff details
    $scope.delete = function (index) {
        $http({
            method: 'DELETE',
            url: 'api/Tariff/' + $scope.tariffsData[index].Id,
        }).then(function successCallback(response) {
            $scope.tariffsData.splice(index, 1);
            alert("Tariff Deleted Successfully !!!");
        }, function errorCallback(response) {
            alert("Error : " + response.data.ExceptionMessage);
        });
    };
});

app.factory('TariffsService', function ($http) {
    var fac = {};
    fac.GetAllRecords = function () {
        return $http.get('api/Tariff');
    }
    return fac;
});