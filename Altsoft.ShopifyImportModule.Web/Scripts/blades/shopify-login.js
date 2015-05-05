angular.module('altsoft.shopifyImportModule')
.controller('shopifyLogin', ['$scope', 'shopifyAuthorizationResources', 'bladeNavigationService',
    function ($scope, shopifyAuthorizationResources, bladeNavigationService) {

        $scope.setForm = function (form) {
            $scope.formScope = form;
        }
        
        $scope.login = function(userName, password, shopName) {
            $scope.blade.isLoading = true;

            shopifyAuthorizationResources.authorize({
                userName: userName,
                password: password,
                shopName:shopName
            }, function (data) {
                if (data.IsSuccess) {
                    bladeNavigationService.closeBlade($scope.blade);
                    $scope.blade.parentBlade.refresh();
                }
                $scope.blade.isLoading = false;
            });
        }

        $scope.blade.isLoading = false;
    }]);
