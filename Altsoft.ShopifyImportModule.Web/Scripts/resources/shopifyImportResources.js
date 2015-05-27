angular.module('altsoft.shopifyImportModule')
.factory('shopifyImportResources', ['$resource', function ($resource) {
    return $resource('api/shopifyImport/', {}, {
        get: { method: 'GET', url: 'api/shopifyImport/get/' },
        getCollections: { method: 'GET', url: 'api/shopifyImport/get-collections/', isArray: true },
        startImport: { method: 'GET', url: 'api/shopifyImport/start-import/' },
        getCatalogs:{method: 'GET', url:'api/shopifyImport/'}
    });
}]);