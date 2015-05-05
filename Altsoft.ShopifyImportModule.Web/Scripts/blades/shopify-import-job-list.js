angular.module('altsoft.shopifyImportModule')
.controller('shopifyImportJobListController', ['$scope', 'shopifyImportResources', 'shopifyAuthorizationResources', 'bladeNavigationService', 'dialogService', function ($scope, shopifyImportResources, shopifyAuthorizationResources, bladeNavigationService, dialogService) {

    $scope.isAuthorized = false;

    $scope.blade.refresh = function () {
        $scope.blade.isLoading = true;
        shopifyAuthorizationResources.isAuthorized({}, function (data) {
            $scope.isAuthorized = data.isAuthorized;
            $scope.blade.isLoading = false;
        });
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
        },
        {
            name: "Login",
            icon: 'fa fa-plus',
            executeMethod: function() {
                closeChildrenBlades();

                var newBlade = {
                    id: 'shopifyLogin',
                    title: 'Login to shopify',
                    subtitle: 'Please enter cridentials',
                    controller: 'shopifyLogin',
                    template: 'Modules/$(Altsoft.ShopifyImport)/Scripts/blades/shopify-login.tpl.html'
                };

                bladeNavigationService.showBlade(newBlade, $scope.blade);
            },
            canExecuteMethod: function() {
                return true;
            }
        }
    ];


    $scope.blade.refresh();
}]);
