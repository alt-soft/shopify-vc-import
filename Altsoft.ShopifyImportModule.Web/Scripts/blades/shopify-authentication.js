angular.module('altsoft.shopifyImportModule')
.controller('altsoft.shopifyImportModule.shopifyAuthenticationController', ['$scope', 'shopifyAuthenticationResources', 'platformWebApp.bladeNavigationService', function ($scope, shopifyAuthenticationResources, bladeNavigationService) {

    $scope.apiKey = '';
    $scope.password = '';
    $scope.shopName = '';


    var blade = $scope.blade;
    blade.isLoading = false;
    blade.title = 'Shopify authentication';
    blade.subtitle = 'Please enter shopify cridentials';
    blade.headIcon = 'fa-key';

    $scope.refresh = function () {
        blade.isLoading = true;
        shopifyAuthenticationResources.isAuthenticated({}, function (result) {
            blade.isLoading = false;
            if(result.isAuthenticated)
            {
                var newBlade = {
                    id: "virtoTargetCatalog",
                    title: 'Shopify Import — Step 2',
                    subtitle: 'Virto Target Catalog',
                    template: 'Modules/$(Altsoft.ShopifyImport)/Scripts/blades/virto-target-catalog.tpl.html',
                    selectedProductIds: $scope.selectedProductIds
                };

                bladeNavigationService.showBlade(newBlade, blade.parentBlade);
            }
        });
    }

    $scope.isValid = function() {
        return $scope.apiKey && $scope.password  && $scope.shopName ;
    }

    $scope.save = function() {
        $scope.blade.isLoading = false;
        shopifyAuthenticationResources.authenticate({
            apiKey: $scope.apiKey,
            password: $scope.password,
            shopName: $scope.shopName
        }, function () {
            $scope.blade.isLoading = false;
            $scope.refresh();
        });
    }

    $scope.refresh();
}]);
