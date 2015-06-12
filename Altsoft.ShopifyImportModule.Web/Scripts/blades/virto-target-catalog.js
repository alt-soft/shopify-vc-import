angular.module('altsoft.shopifyImportModule')
.controller('virtoTargetCatalogController', ['$scope', 'platformWebApp.bladeNavigationService', 'shopifyImportResources', function ($scope, bladeNavigationService, shopifyImportResources) {
    $scope.$watch('selectedCatalog', function (selectedCatalog) {
        if (selectedCatalog == null) return;

        $scope.isLoading = true;

        shopifyImportResources.getCategories(
            { catalogId: selectedCatalog.id },
            function (result) {
                if (result.isSuccess) {
                    $scope.categories = result.items;
                    $scope.selectedCategory = $scope.categories[0];
                } else {
                    $scope.errorMessage = result.errorMessage;
                }

                $scope.isLoading = false;
            },
            function (error) {
                $scope.errorMessage = error;
                $scope.isLoading = false;
            });
    });

    $scope.nextStep = function () {
        var newBlade = {
            id: "shopifyImportProgress",
            title: 'Shopify Import — Step 3',
            subtitle: 'Import Progress',
            template: 'Modules/$(Altsoft.ShopifyImport)/Scripts/blades/shopify-import-progress.tpl.html',
            selectedProductIds: $scope.blade.selectedProductIds,
            selectedCatalogId: $scope.selectedCatalog.id,
            selectedCategoryId: $scope.selectedCategory.virtoId,
            isRetainCategoryHierarchy: $scope.isRetainCategoryHierarchy,
            controller: 'shopifyImportProgressController'
        };

        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };

    $scope.isRetainCategoryHierarchy = true;
    $scope.blade.isLoading = true;

    shopifyImportResources.getCatalogs(
        {},
        function (result) {
            if (result.isSuccess) {
                $scope.catalogs = result.items;
                $scope.selectedCatalog = $scope.catalogs[0];
            } else {
                $scope.errorMessage = result.errorMessage;
            }

            $scope.blade.isLoading = false;
        },
        function (error) {
            $scope.errorMessage = error;
            $scope.blade.isLoading = false;
        });
}]);
