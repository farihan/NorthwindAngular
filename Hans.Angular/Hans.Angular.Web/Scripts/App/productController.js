'use strict';
angular.module('Northwind', ['ui.bootstrap']);

angular.module('Northwind').controller('ProductController', function ($scope, $http) {
    $scope.pagingInfo = {
        page: 1,
        pageSize: 10,
        sortBy: 'productid',
        isAsc: true,
        totalItems: 0
    };
    $scope.setPage = function (pageNo) {
        $scope.pagingInfo.page = pageNo;
    };
    $scope.pageChanged = function () {
        pageInit();
    };

    $scope.sort = function (sortBy) {
        if (sortBy === $scope.pagingInfo.sortBy) {
            $scope.pagingInfo.isAsc = !$scope.pagingInfo.isAsc;
        } else {
            $scope.pagingInfo.sortBy = sortBy;
            $scope.pagingInfo.isAsc = false;
        }
        pageInit();
    };

    function loadTotalItems() {
        $http.get('/api/product/getsize').success(function (data) {
            $scope.pagingInfo.totalItems = data;
        })
        .error(function () {
            $scope.error = "Error has occured!";
        });
    }

    function loadProducts() {
        $scope.products = null;
        $http.get('/api/product/getallby', {params: $scope.pagingInfo }).success(function (data) {
            $scope.products = data;
        })
        .error(function () {
            $scope.error = "Error has occured!";
        });
    }    

    function pageInit() {
        loadTotalItems();
        loadProducts();
    }

    pageInit();
});