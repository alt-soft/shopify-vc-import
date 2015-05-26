angular.module('altsoft.shopifyImportModule')
.controller('virtoTargetCatalogController', ['$scope', 'platformWebApp.bladeNavigationService', 'shopifyImportResources', function ($scope, bladeNavigationService, shopifyImportResources) {
    $scope.$watch('selectedCatalog', function (selectedCatalog) {
        if (selectedCatalog == null) return;

        $scope.isLoading = true;

        shopifyImportResources.getCategories(
            { catalogId: selectedCatalog.id },
            function (result) {
                if (result.isSuccess) {
                    $scope.categories = result.categories;
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
            id: "magentoImport",
            title: 'Magento Import — Step 4',
            subtitle: 'Magento Import',
            template: 'Modules/MagentoImport/VirtoCommerce.MagentoImportModule.Web/Scripts/blades/magento-import.tpl.html',
            selectedProductIds: $scope.blade.selectedProductIds,
            selectedCatalogId: $scope.selectedCatalog.id,
            selectedCategoryId: $scope.selectedCategory.virtoId,
            isRetainCategoryHierarchy: $scope.isRetainCategoryHierarchy
        };

        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };

    $scope.isRetainCategoryHierarchy = true;
    $scope.blade.isLoading = true;

    virtoCatalogService.getCatalogs(
        {},
        function (result) {
            if (result.isSuccess) {
                $scope.catalogs = result.catalogs;
                $scope.selectedCatalog = $scope.catalogs[0];
            } else {
                $scope.errorMessage = result.errorMessage;
            }

            $scope.blade.isLoading = false;
        },
        function (error) {
            $scope.errorMessage = error;
        });
}]);
