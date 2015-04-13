angular.module('altsoft.shopifyImportModule.blades.blade1', [
    'altsoft.shopifyImportModule.resources.shopifyImportResources'])
.controller('blade1Controller', ['$scope', 'shopifyImportResources', function ($scope, shopifyImportResources) {
    $scope.blade.refresh = function () {
        shopifyImportResources.get(function (data) {
            $scope.blade.data = data;
            $scope.blade.title = data;
            $scope.blade.isLoading = false;
        });
    }
    $scope.blade.refresh();
}]);