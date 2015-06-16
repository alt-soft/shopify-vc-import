using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using Altsoft.ShopifyImportModule.Data.Interfaces;
using Altsoft.ShopifyImportModule.Data.Models;
using VirtoCommerce.CatalogModule.Data.Converters;
using VirtoCommerce.CatalogModule.Data.Model;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Catalog.Services;
using Catalog = VirtoCommerce.Domain.Catalog.Model.Catalog;
using Category = VirtoCommerce.Domain.Catalog.Model.Category;

namespace Altsoft.ShopifyImportModule.Data.Services
{
    public class VirtoCatalogService :CatalogRepositoryImpl, IVirtoCatalogService
    {
        #region Private Fields

        private readonly ICatalogService _catalogService;
        private readonly ILoggerFacade _loggerFacade;
        private readonly ICatalogSearchService _searchService;

        #endregion

        #region Constructors

        public VirtoCatalogService(ILoggerFacade loggerFacade, ICatalogService catalogService, ICatalogSearchService searchService, Func<ICatalogRepository> catalogRepositoryFactory)
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
                _loggerFacade.Log(e.Message + e.StackTrace, Interfaces.LogCategory.Exception, LogPriority.High);
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

            var virtoCategories = new List<Category>();

            try
            {
                var categories = _searchService.Search(new SearchCriteria()
                {
                    CatalogId = searchCriteria.CatalogId,
                    GetAllCategories = true,
                    ResponseGroup = ResponseGroup.WithCategories
                });
                virtoCategories.AddRange(
                   categories.Categories
                        .OrderBy(category => category.Name)
                        .ToList());
            }
            catch (Exception e)
            {
                _loggerFacade.Log(e.Message + e.StackTrace, LogCategory.Exception, LogPriority.High);
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

        public void AddCategory(VirtoCategory virtoCategory)
        {
            var category = new Category
            {
                Id = virtoCategory.VirtoId,
                Name = virtoCategory.Name,
                CreatedDate = DateTime.Now,
                Code = virtoCategory.Code,
                IsActive = true,
                Priority = virtoCategory.Priority,
                CatalogId = virtoCategory.CatalogId,
                ParentId = virtoCategory.ParentCategoryId,
                
            };
            var dbCategory = category.ToDataModel();
            Add(dbCategory);
        }

        public void AddProduct(Product virtoProduct, string virtoCatalogId, IEnumerable<string> virtoCategoryIds)
        {
            Add(virtoProduct);

            if (virtoCategoryIds != null)
            {
                foreach (var categoryId in virtoCategoryIds)
                {
                    AddCategoryItemRelation(virtoProduct.Id, categoryId, virtoCatalogId);
                }
            }
        }

        public void CommitChanges()
        {
            try
            {
                SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    _loggerFacade.Log(string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State),LogCategory.Exception, LogPriority.High);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine();

                        _loggerFacade.Log(string.Format("- Property: \"{0}\", Error: \"{1}\"",ve.PropertyName, ve.ErrorMessage),
                            LogCategory.Exception, LogPriority.High);
                    }
                }
                throw;
            }
        }

        #endregion

        private void AddCategoryItemRelation(string productId, string categoryId, string catalogId)
        {
            var categoryItemRelation = new CategoryItemRelation
            {
                ItemId = productId,
                CategoryId = categoryId,
                CatalogId = catalogId
            };
            Add(categoryItemRelation);
        }
    }
}