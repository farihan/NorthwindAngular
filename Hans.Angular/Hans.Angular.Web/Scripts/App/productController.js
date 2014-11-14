'use strict';
angular.module('Northwind', ['ui.bootstrap']);

angular.module('Northwind').controller('ProductController', function ($scope, $http, $modal) {
    $scope.pagingInfo = {
        page: 1,
        pageSize: 10,
        sortBy: 'productid',
        isAsc: true,
        totalItems: 0
    };

    $scope.gridIndex = function (index) {
        return (index + 1) + (($scope.pagingInfo.page - 1) * $scope.pagingInfo.pageSize);
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

    $scope.openModal = function (id, html) {
        var modalInstance = $modal.open({
            templateUrl: html,
            controller: 'ModalController',
            size: '',
            resolve: {
                selectedID: function () {
                    return id;
                }
            }
        });

        modalInstance.result.then(function (selectedItem) {
            $scope.selected = selectedItem;
        }, function () {
            //$log.info('Modal dismissed at: ' + new Date());
        });
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
        $http.get('/api/product/getallby', { params: $scope.pagingInfo }).success(function (data) {
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

angular.module('Northwind').controller('ModalController', function ($scope, $modalInstance, $http, selectedID) {
    $scope.selectedID = selectedID;
    $scope.ok = function () {
        $modalInstance.close();
    };

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };
    
    function getAvailableSuppliers() {
        $scope.availableSuppliers = [];
        $http.get('/api/supplier/getall').success(function (data) {
            $.each(data, function (index, item) {
                $scope.availableSuppliers.push({ ID: item.SupplierID, Name: item.ContactName + ' (' + item.CompanyName + ')' });
            });
        })
        .error(function () {
            $scope.error = "Error has occured!";
        });
    }

    function getAvailableCategories() {
        $scope.availableCategories = [];
        $http.get('/api/category/getall').success(function (data) {
            $.each(data, function (index, item) {
                $scope.availableCategories.push({ ID: item.CategoryID, Name: item.CategoryName });
            });
        })
        .error(function () {
            $scope.error = "Error has occured!";
        });
    }

    function loadProduct() {
        $scope.product = null;
        $http.get('/api/product/get', { params: { id: $scope.selectedID } }).success(function (data) {
            $scope.product = data;
        })
        .error(function () {
            $scope.error = "Error has occured!";
        });
    }

    function pageInit() {
        getAvailableSuppliers();
        getAvailableCategories();

        if ($scope.selectedID != null)
            loadProduct();
    }

    pageInit();
});