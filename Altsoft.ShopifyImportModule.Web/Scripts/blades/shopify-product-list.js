angular.module('altsoft.shopifyImportModule')
.controller('shopifyProductListController', ['$scope', 'shopifyImportResources', 'platformWebApp.bladeNavigationService', function ($scope, shopifyImportResources, bladeNavigationService) {
    
    $scope.blade.refresh = function () {
        $scope.blade.isLoading = true;
        shopifyImportResources.get({}, function(result) {
            $scope.products = result;
            $scope.blade.isLoading = false;
        });
        
    };

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
