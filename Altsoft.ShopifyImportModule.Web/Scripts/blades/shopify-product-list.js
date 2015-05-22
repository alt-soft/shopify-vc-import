angular.module('altsoft.shopifyImportModule')
.controller('shopifyProductListController', ['$scope', 'shopifyImportResources', 'platformWebApp.bladeNavigationService', function ($scope, shopifyImportResources, bladeNavigationService) {

    function closeChildrenBlades() {
        angular.forEach($scope.blade.childrenBlades.slice(), function (child) {
            bladeNavigationService.closeBlade(child);
        });
    }

    $scope.blade.refresh = function() {
        if ($scope.blade.products) {
            $scope.products = $scope.blade.products;
            $scope.blade.isLoading = false;
        } else {
            closeChildrenBlades();
            $scope.blade.isLoading = true;
            shopifyImportResources.getCollections({}, function(result) {
                $scope.products = result;
                $scope.blade.isLoading = false;
            });
        }
    };

   

    var selectChildren = function (product) {
        if (product.children && product.children.length > 0) {
            $(product.children).each(function (index, child) {
                child.isSelected = product.isSelected;
            });
        }
    }


    $scope.showChildren = function (product) {
        if (product.isCollection && product.children) {
            closeChildrenBlades();
            var blade = {
                id: 'shopify-collection-items',
                products: product.children,
                title: product.title,
                parentCollection: product,
                template: 'Modules/$(Altsoft.ShopifyImport)/Scripts/blades/shopify-product-list.tpl.html',
                isClosingDisabled: false,
                isLoading: false,
                controller: 'shopifyProductListController'
            };
            bladeNavigationService.showBlade(blade);
        }
    }
    var allProductsChecked = function (products) {
        var result = true;
        $(products).each(function (index, product) {
            if (!product.isSelected)
                result = false;
        });

        return result;
    }
    
   

    $scope.changeSelection = function(product) {
        if ($scope.blade.parentCollection) {
            $scope.blade.parentCollection.isSelected = allProductsChecked($scope.blade.parentCollection.children);
        }

        selectChildren(product);
    }

        $scope.bladeToolbarCommands = [
            {
                name: "Refresh",
                icon: 'fa fa-refresh',
                executeMethod: function() {
                    $scope.blade.refresh();
                },
                canExecuteMethod: function() {
                    return !$scope.blade.products;
                }
            }
        ];

    $scope.blade.refresh();
}]);
