angular.module('altsoft.shopifyImportModule.resources.shopifyAuthorizationResources', [])
.factory('shopifyAuthorizationResources', ['$resource', function ($resource) {
    return $resource('api/shopifyAuthorization/', {}, {
        isAuthorized: { method: 'GET', url: 'api/shopifyAuthorization/isAuthorized/' }
    });
}]);