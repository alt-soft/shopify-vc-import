//Call this to register our module to main application
var moduleTemplateName = "altsoft.shopifyImportModule";

if (AppDependencies != undefined) {
    AppDependencies.push(moduleTemplateName);
}

angular.module(moduleTemplateName, [
    'altsoft.shopifyImportModule.blades.shopify-import-job-list'
])
.config(
  ['$stateProvider', '$urlRouterProvider',
    function ($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('workspace.shopifyImportModuleTemplate', {
                url: '/shopifyImportModuleTemplate',
                templateUrl: 'Modules/$(Altsoft.ShopifyImport)/Scripts/home/home.tpl.html',
                controller: [
                    '$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
                        var blade = {
                            id: 'shopify-import-job-list',
                            controller: 'shopifyImportJobListController',
                            template: 'Modules/$(Altsoft.ShopifyImport)/Scripts/blades/shopify-import-job-list.tpl.html',
                            isClosingDisabled: true
                        };
                        bladeNavigationService.showBlade(blade);
                    }
                ]
            });
    }
  ]
)
.run(
  ['$rootScope', 'mainMenuService', 'widgetService', '$state', function ($rootScope, mainMenuService, widgetService, $state) {
      //Register module in main menu
      var menuItem = {
          path: 'browse/shopifyImportModule',
          icon: 'fa fa-cube',
          title: 'Shopify import',
          priority: 100,
          action: function () { $state.go('workspace.shopifyImportModuleTemplate') },
          permission: 'shopifyImportModulePermission'
      };
      mainMenuService.addMenuItem(menuItem);
  }]);

