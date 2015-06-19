angular.module('altsoft.shopifyImportModule')
.controller('altsoft.shopifyImportModule.shopifyImportParamsController', ['$scope', 'shopifyAuthenticationResources', 'platformWebApp.bladeNavigationService', function ($scope, shopifyAuthenticationResources, bladeNavigationService) {

    var blade = $scope.blade;
    blade.isLoading = false;

    $scope.availableShops = [
        { id: 0, name: 'Apple' },
        { id: 1, name: 'Sony' },
        { id: 2, name: 'Abibas' }
    ];

    $scope.importParams = {
        virtoCatalogId: blade.virtoCatalogId,
        virtoCategoryId: blade.virtoCategoryId,
        importProducts: false,
        importImages: false,
        importProperties: false,
        importCustomers: false,
        importOrders: false,
        importThemes: false,
        storeId: null
    }

    $scope.isValid = function () {
        var importParams = $scope.importParams;
        var valid =
            importParams.virtoCatalogId &&
            importParams.virtoCategoryId &&
            (importParams.importProducts ||
            importParams.importImages ||
            importParams.importProperties ||
            importParams.importCustomers ||
            importParams.importOrders ||
            importParams.importThemes);

        if (valid && importParams.importThemes)
            valid = !importParams.storeId;

        return valid;
    }

    $scope.startImport = function (params) {
    }
}]);
