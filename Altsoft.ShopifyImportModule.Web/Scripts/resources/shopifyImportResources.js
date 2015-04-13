angular.module('altsoft.shopifyImportModule.resources.shopifyImportResources', [])
.factory('shopifyImportResources', ['$resource', function ($resource) {
    return $resource('api/shopifyImport/', {}, {
        get: { method: 'GET', url: 'api/shopifyImport/get/', isArray: true },
    });
}]);