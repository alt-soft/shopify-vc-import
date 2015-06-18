angular.module('altsoft.shopifyImportModule')
.factory('shopifyImportResources', ['$resource', function ($resource) {
    return $resource('api/shopifyImport/', {}, {
        getCollections: { method: 'GET', url: 'api/shopifyCatalog/get-collections/' },
        
        getCatalogs: { method: 'GET', url: 'api/virtoCatalog/get-catalogs/' },
        getCategories: { method: 'GET', url: 'api/virtoCatalog/get-categories/' },

        startImport: { method: 'POST', url: 'api/shopifyImport/start-import/' },
        getProgress: { method: 'GET', url: 'api/shopifyImport/get-progress/' }
    });
}])
.factory('shopifyAuthenticationResources', ['$resource', function ($resource) {
    return $resource('api/shopifyAuthentication/', {}, {
        isAuthenticated: { method: 'GET', url: 'api/shopifyAuthentication/is-authenticated/' },
        authenticate: { method: 'POST', url: 'api/shopifyAuthentication/authenticate'}
    });
}]);