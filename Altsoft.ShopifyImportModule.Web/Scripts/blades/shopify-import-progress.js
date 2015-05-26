angular.module('altsoft.shopifyImportModule')
.controller('shopifyImportProgressController', ['$scope', 'shopifyImportResources', 'platformWebApp.bladeNavigationService', function ($scope, shopifyImportResources, bladeNavigationService) {
    $scope.bladeToolbarCommands = [
        {
            name: "Start import",
            icon: 'fa fa-refresh',
            executeMethod: function () {
                
            },
            canExecuteMethod: function () {
                return true;
            }
        }
    ];
}]);
