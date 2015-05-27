using System;
using System.Collections.Generic;
using System.Linq;
using Altsoft.ShopifyImportModule.Data.Interfaces;
using Altsoft.ShopifyImportModule.Data.Models;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Catalog.Services;
using Category = VirtoCommerce.Domain.Catalog.Model.Category;

namespace Altsoft.ShopifyImportModule.Data.Services
{
    public class VirtoCatalogService : IVirtoCatalogService
    {
        #region Private Fields

        private readonly ICatalogService _catalogService;
        private readonly ILoggerFacade _loggerFacade;
        private readonly ICatalogSearchService _searchService;

        #endregion

        #region Constructors

        public VirtoCatalogService(ILoggerFacade loggerFacade, ICatalogService catalogService, ICatalogSearchService searchService)
        {
            _loggerFacade = loggerFacade;
            _catalogService = catalogService;
            _searchService = searchService;
        }

        #endregion

        #region IVirtoCatalogService Implementation

        public PaginationResult<Catalog> GetCatalogs()
        {
            List<Catalog> catalogs;

            try
            {
                catalogs = _catalogService.GetCatalogsList().ToList();

            }
            catch (Exception e)
            {
                _loggerFacade.Log(e.Message + e.StackTrace, Interfaces.Category.Exception, Priority.High);
                return new PaginationResult<Catalog>
                {
                    IsSuccess = false,
                    ErrorMessage = "Can't retrieve Virto catalogs. See log files for more information."
                };
            }

            return new PaginationResult<Catalog>
            {
                IsSuccess = true,
                TotalCount = catalogs.Count(),
                Items = catalogs
            };
        }

        public PaginationResult<Category> GetCategories(VirtoCategorySearchCriteria searchCriteria)
        {
            if (searchCriteria == null || string.IsNullOrEmpty(searchCriteria.CatalogId))
                return new PaginationResult<Category>
                {
                    IsSuccess = true,
                    TotalCount = 0
                };

            var virtoCategories = new List<Category> { new Category() };

            try
            {
                var categories = _searchService.Search(new SearchCriteria()
                {
                    CatalogId = searchCriteria.CatalogId,
                    GetAllCategories = true,
                });
                virtoCategories.AddRange(
                   categories.Categories
                        .OrderBy(category => category.Name)
                        .ToList());
            }
            catch (Exception e)
            {
                _loggerFacade.Log(e.Message + e.StackTrace, Interfaces.Category.Exception, Priority.High);
                return new PaginationResult<Category>
                {
                    IsSuccess = false,
                    ErrorMessage = "Can't retrieve Virto categories. See log files for more information."
                };
            }

            return new PaginationResult<Category>
            {
                IsSuccess = true,
                TotalCount = virtoCategories.Count(),
                Items = virtoCategories
            };
        }

        #endregion
    }
}