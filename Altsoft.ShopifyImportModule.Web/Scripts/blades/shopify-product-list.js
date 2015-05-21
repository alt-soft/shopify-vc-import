angular.module('altsoft.shopifyImportModule')
.controller('shopifyProductListController', ['$scope', 'shopifyImportResources', 'platformWebApp.bladeNavigationService', function ($scope, shopifyImportResources, bladeNavigationService) {

    $scope.blade.refresh = function() {
        if ($scope.blade.products) {
            $scope.products = $scope.blade.products;
            $scope.blade.isLoading = false;
        } else {
            $scope.blade.isLoading = true;
            shopifyImportResources.getCollections({}, function(result) {
                $scope.products = result;
                $scope.blade.isLoading = false;
            });
        }
    };

    function closeChildrenBlades() {
        angular.forEach($scope.blade.childrenBlades.slice(), function (child) {
            bladeNavigationService.closeBlade(child);
        });
    }

    $scope.showChildren = function (product) {
        if (product.isCollection && product.children) {
            closeChildrenBlades();
            var blade = {
                id: 'shopify-collection-items',
                products: product.children,
                title: product.title,
                template: 'Modules/$(Altsoft.ShopifyImport)/Scripts/blades/shopify-product-list.tpl.html',
                isClosingDisabled: false,
                isLoading: false,
                controller: 'shopifyProductListController'
            };
            bladeNavigationService.showBlade(blade);
        }
    }

    $scope.bladeToolbarCommands = [
        {
            name: "Refresh",
            icon: 'fa fa-refresh',
            executeMethod: function () {
                $scope.blade.refresh();
            },
            canExecuteMethod: function () {
                return true;
            }
        }
    ];

    $scope.blade.refresh();
}]);
