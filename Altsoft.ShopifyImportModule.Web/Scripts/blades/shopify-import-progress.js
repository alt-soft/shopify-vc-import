angular.module('altsoft.shopifyImportModule')
.controller('shopifyImportProgressController', ['$scope', 'shopifyImportResources', 'platformWebApp.bladeNavigationService', function ($scope, shopifyImportResources, bladeNavigationService) {
        $scope.blade.isLoading = false;
        $scope.isCompleted = false;

        $scope.currentOperationDescription = "";
        $scope.progressPercentage = 0;
        $scope.addedItemsCount = 0;
        $scope.totalItemsCount = 0;

        $scope.updateProgress = function () {
            shopifyImportResources.getProgress({}, function (result) {
                $scope.currentOperationDescription = result.magentoImportProgress.currentOperationDescription;
                $scope.progressPercentage = result.magentoImportProgress.progressPercentage;
                $scope.addedItemsCount = result.magentoImportProgress.addedItemsCount;
                $scope.totalItemsCount = result.magentoImportProgress.totalItemsCount;
            }, function (error) {
                $scope.isSuccess = false;
                $scope.errorMessage = error;
                $scope.blade.isLoading = false;
            });

            if ($scope.blade.isLoading == false) {
                clearInterval($scope.refreshIntervalId);
            }
        }

        $scope.import = function () {
            $scope.blade.isLoading = true;

            shopifyImportResources.importProducts({
                magentoProductIds: $scope.blade.selectedProductIds,
                virtoCatalogId: $scope.blade.selectedCatalogId,
                virtoCategoryId: $scope.blade.selectedCategoryId,
                isRetainCategoryHierarchy: $scope.blade.isRetainCategoryHierarchy
            }, function (result) {
                $scope.isSuccess = result.isSuccess;

                if (!result.isSuccess) {
                    $scope.errorMessage = result.errorMessage;
                }

                $scope.isCompleted = true;
                $scope.blade.isLoading = false;
            }, function (error) {
                $scope.isSuccess = false;
                $scope.errorMessage = error;
                $scope.isCompleted = true;
                $scope.blade.isLoading = false;
            });

            $scope.refreshIntervalId = setInterval($scope.updateProgress, 250);
        };
    }
}]);
