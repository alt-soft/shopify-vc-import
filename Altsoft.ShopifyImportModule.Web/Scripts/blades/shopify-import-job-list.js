angular.module('altsoft.shopifyImportModule')
.controller('shopifyImportJobListController', ['$scope', 'shopifyImportResources', 'bladeNavigationService', 'dialogService', function ($scope, shopifyImportResources, bladeNavigationService, dialogService) {
    
    $scope.blade.refresh = function () {
        $scope.blade.isLoading = true;
        shopifyImportResources.get({}, function(result) {
            $scope.products = result;
        });
        $scope.blade.isLoading = false;
    };

    function closeChildrenBlades() {
        angular.forEach($scope.blade.childrenBlades.slice(), function (child) {
            bladeNavigationService.closeBlade(child);
        });
    }

    $scope.bladeToolbarCommands = [
        {
            name: "Refresh",
            icon: 'fa fa-refresh',
            executeMethod: function() {
                $scope.blade.refresh();
            },
            canExecuteMethod: function() {
                return true;
            }
        }
    ];


    $scope.blade.refresh();
}]);
