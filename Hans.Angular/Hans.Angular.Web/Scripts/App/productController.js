//'use strict';
angular.module('Northwind', ['ui.bootstrap']);

angular.module('Northwind').controller('ProductController', function ($scope, $http, $modal) {
    $scope.pagingInfo = {
        page: 1,
        pageSize: 10,
        sortBy: 'productid',
        isAsc: true,
        totalItems: 0
    };

    $scope.products = null;

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
                },
                selectedPagingInfo: function () {
                    return $scope.pagingInfo;
                }
            }
        });

        modalInstance.result.then(function (refreshProducts) {
            $scope.products = refreshProducts;
        }, function () {
            //$log.info('Modal dismissed at: ' + new Date());
        });
    };

    function loadTotalItems() {
        $http.get('/api/product/getsize')
        .success(function (data, status, headers, config) {
            $scope.pagingInfo.totalItems = data;
        })
        .error(function (data, status, headers, config) {
            $scope.error = "Error has occured!";
        });
    }

    function loadProducts() {
        $http.get('/api/product/getallby', { params: $scope.pagingInfo })
        .success(function (data, status, headers, config) {
            $scope.products = data;
        })
        .error(function (data, status, headers, config) {
            $scope.error = "Error has occured!";
        });
    }    

    function pageInit() {
        loadTotalItems();
        loadProducts();
    }

    pageInit();
});

angular.module('Northwind').controller('ModalController', function ($scope, $modalInstance, $http, selectedID, selectedPagingInfo) {
    $scope.selectedID = selectedID;
    $scope.selectedPagingInfo = selectedPagingInfo;

    $scope.ok = function () {
        closeAndRefreshRepeater();
        //$modalInstance.close();
    };

    $scope.cancel = function () {
        closeAndRefreshRepeater();
        //$modalInstance.dismiss('cancel');
    };

    $scope.create = function (product) {
        if ($scope.createForm.$valid) {
            //$http.post('/api/product/create', {
            //    'ProductName' : product.ProductName,
            //    'SupplierID' : product.SupplierID,
            //    'CategoryID' : product.CategoryID,
            //    'QuantityPerUnit' : product.QuantityPerUnit,
            //    'UnitPrice' : product.UnitPrice,
            //    'UnitsInStock' : product.UnitsInStock,
            //    'UnitsOnOrder' : product.UnitsOnOrder,
            //    'ReorderLevel' : product.ReorderLevel,
            //    'Discontinued' : product.Discontinued
            //})
            $http({
                method: 'POST',
                url: '/api/product/create',
                data: {
                    'ProductName': product.ProductName,
                    'SupplierID': product.SupplierID,
                    'CategoryID': product.CategoryID,
                    'QuantityPerUnit': product.QuantityPerUnit,
                    'UnitPrice': product.UnitPrice,
                    'UnitsInStock': product.UnitsInStock,
                    'UnitsOnOrder': product.UnitsOnOrder,
                    'ReorderLevel': product.ReorderLevel,
                    'Discontinued': product.Discontinued
                },
                headers: { 'Content-Type': 'application/json' }
            })
            .success(function (data, status, headers, config) {
                closeAndRefreshRepeater();
            })
            .error(function(data, status, headers, config) {
                $scope.error = "Error has occured!";
            });
        }
        else {
        }
    };

    $scope.update = function (product) {        
        if ($scope.editForm.$valid) {
            $http({
                method: 'PUT',
                url: '/api/product/edit/' + product.ProductID ,
                data: {
                    'ProductID': product.ProductID,
                    'ProductName': product.ProductName,
                    'SupplierID': product.SupplierID,
                    'CategoryID': product.CategoryID,
                    'QuantityPerUnit': product.QuantityPerUnit,
                    'UnitPrice': product.UnitPrice,
                    'UnitsInStock': product.UnitsInStock,
                    'UnitsOnOrder': product.UnitsOnOrder,
                    'ReorderLevel': product.ReorderLevel,
                    'Discontinued': product.Discontinued
                },
                headers: { 'Content-Type': 'application/json' }
            })
            .success(function (data, status, headers, config) {
                closeAndRefreshRepeater();
            })
            .error(function (data, status, headers, config) {
                $scope.error = "Error has occured!";
            });
        }
        else {
        }
    };

    $scope.delete = function (id) {
        $http.delete('/api/product/delete', {
            params: { 'id': id }
        })
        .success(function (data, status, headers, config) {
            closeAndRefreshRepeater();
        })
        .error(function (data, status, headers, config) {
            $scope.error = "Error has occured!";
        });
    };

    function getAvailableSuppliers() {
        $scope.availableSuppliers = [];
        $http.get('/api/supplier/getall')
        .success(function (data, status, headers, config) {
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
        $http.get('/api/category/getall')
        .success(function (data, status, headers, config) {
            $.each(data, function (index, item) {
                $scope.availableCategories.push({ ID: item.CategoryID, Name: item.CategoryName });
            });
        })
        .error(function (data, status, headers, config) {
            $scope.error = "Error has occured!";
        });
    }

    function loadProduct() {
        $scope.product = null;
        $http.get('/api/product/get', { params: { id: $scope.selectedID } })
        .success(function (data, status, headers, config) {
            $scope.product = data;
        })
        .error(function (data, status, headers, config) {
            $scope.error = "Error has occured!";
        });
    }

    function loadTotalItems() {
        $http.get('/api/product/getsize')
        .success(function (data, status, headers, config) {
            $scope.selectedPagingInfo.totalItems = data;
        })
        .error(function (data, status, headers, config) {
            $scope.error = "Error has occured!";
        });
    }

    function loadProducts() {
        $http.get('/api/product/getallby', { params: $scope.selectedPagingInfo })
        .success(function (data, status, headers, config) {
            $scope.refreshProducts = data;
            $modalInstance.close(data);
        })
        .error(function (data, status, headers, config) {
            $scope.error = "Error has occured!";
        });
    }

    function closeAndRefreshRepeater() {
        loadTotalItems();
        loadProducts();
    }

    function pageInit() {
        getAvailableSuppliers();
        getAvailableCategories();

        if ($scope.selectedID != null)
            loadProduct();
    }

    pageInit();
});