angular.module('altsoft.shopifyImportModule')
.factory('shopifyAuthorizationResources', ['$resource', function ($resource) {
    return $resource('api/shopifyAuthorization/', {}, {
        isAuthorized: { method: 'GET', url: 'api/shopifyAuthorization/isAuthorized/' },
        authorize: { method: 'POST', url: 'api/shopifyAuthorization/authorize/' }

    });
}]);