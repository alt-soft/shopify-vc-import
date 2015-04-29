using System;
using System.Collections.Generic;
using System.Diagnostics;
using Altsoft.ShopifyImportModule.Data.Interfaces;
using Altsoft.ShopifyImportModule.Data.Models;
using ShopifyAPIAdapterLibrary;
using VirtoCommerce.Platform.Core.Settings;

namespace Altsoft.ShopifyImportModule.Data.Repositories
{
    public class ShopifyRepository:IShopifyRepository
    {
        public PaginationResult<ShopifyProduct> GetShopifyProductsFromSource(ShopifyProductSearchCriteria searchCriteria)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ShopifyCategory> GetShopifyCategoriesTree()
        {
            string shopName = "shopify-import-test-shop";// get the shop name from the user (i.e. a web form)
            // you will need to pass a URL that will handle the response from Shopify when it passes you the code parameter
            var consumerKey = "4c5624c29b9722f7a8cd276d7c1dedcb";
            var consumerSecret = "209c14a41f95facc8724016869638818";


            var scope = "read_products, write_products";
            var authorizer = new ShopifyAPIAuthorizer(shopName,
                consumerKey,
                consumerSecret);

            // get the Authorization URL and redirect the user
            var authUrl = authorizer.GetAuthorizationURL(new string[] { scope });
            Process.Start(authUrl);
            //Redirect(authUrl);

            // Meanwhile the User is click "yes" to authorize your app for the specified scope.  
            // Once this click, yes or no, they are redirected back to the return URL

            // Handle the shopify response at the Return URL:

            // get the following variables from the Query String of the request
            string code = "";
            string shop = "";
            string error = "";

            // get the authorization state
            ShopifyAuthorizationState authState = authorizer.AuthorizeClient(code);

            if (authState != null && authState.AccessToken != null)
            {
                // store the auth state in the session or DB to be used for all API calls for the specified shop
            }
            throw new NotImplementedException();
        }
    }
}