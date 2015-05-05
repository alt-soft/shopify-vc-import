angular.module('altsoft.shopifyImportModule')
.factory('shopifyImportResources', ['$resource', function ($resource) {
    return $resource('api/shopifyImport/', {}, {
        get: { method: 'GET', url: 'api/shopifyImport/get/', isArray: true },
    });
}]);