angular.module('altsoft.shopifyImportModule')
.controller('shopifyProductListController', ['$scope', 'shopifyImportResources', 'platformWebApp.bladeNavigationService', function ($scope, shopifyImportResources, bladeNavigationService) {
    var processProductId = function (product) {
        if (product.isSelected)
            $scope.selectedProductIds.push(product.id);
        else {
            var index = $scope.selectedProductIds.indexOf(product.id);
            if (index > -1)
                $scope.selectedProductIds.splice(index, 1);
        }
    }

    function closeChildrenBlades() {
        angular.forEach($scope.blade.childrenBlades.slice(), function (child) {
            bladeNavigationService.closeBlade(child);
        });
    }

    $scope.blade.refresh = function () {
        if ($scope.blade.selectedProductIds) {
            $scope.selectedProductIds = $scope.blade.selectedProductIds;
        } else {
            $scope.selectedProductIds = [];
        }
        if ($scope.blade.products) {
            $scope.products = $scope.blade.products;
            $scope.blade.isLoading = false;
        } else {
            closeChildrenBlades();
            $scope.blade.isLoading = true;
            shopifyImportResources.getCollections({}, function (result) {
                $scope.products = result.items;
                $scope.blade.isLoading = false;
            });
        }
    };

    var selectChildren = function (product) {
        if (product.children && product.children.length > 0) {
            $(product.children).each(function (index, child) {
                child.isSelected = product.isSelected;
                processProductId(child);
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
                controller: 'shopifyProductListController',
                selectedProductIds: $scope.selectedProductIds
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

    $scope.nextStep = function () {
        var newBlade = {
            id: "virtoTargetCatalog",
            title: 'Shopify Import — Step 2',
            subtitle: 'Virto Target Catalog',
            template: 'Modules/$(Altsoft.ShopifyImport)/Scripts/blades/virto-target-catalog.tpl.html',
            selectedProductIds: $scope.selectedProductIds
        };
        closeChildrenBlades();
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };


    $scope.changeSelection = function (product) {
        if ($scope.blade.parentCollection) {
            $scope.blade.parentCollection.isSelected = allProductsChecked($scope.blade.parentCollection.children);
            processProductId($scope.blade.parentCollection);
        }

        selectChildren(product);

        processProductId(product);
    }

    $scope.blade.toolbarCommands = [
        {
            name: "Refresh",
            icon: 'fa fa-refresh',
            executeMethod: function () {
                $scope.blade.refresh();
            },
            canExecuteMethod: function () {
                return !$scope.blade.products;
            }
        }
    ];

    $scope.blade.refresh();
}]);
