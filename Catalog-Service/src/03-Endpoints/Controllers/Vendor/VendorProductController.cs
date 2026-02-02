using AutoMapper;
using Catalog_Service.src._01_Domain.Core.Contracts.Services;
using Catalog_Service.src._01_Domain.Core.Entities;
using Catalog_Service.src._01_Domain.Core.Enums;
using Catalog_Service.src._01_Domain.Core.Primitives;
using Catalog_Service.src._03_Endpoints.DTOs.Requests.Vendor;
using Catalog_Service.src._03_Endpoints.DTOs.Responses;
using Catalog_Service.src._03_Endpoints.DTOs.Responses.Vendor;
using Catalog_Service.src.CrossCutting.Exceptions;
using Catalog_Service.src.CrossCutting.Security;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog_Service.src._03_Endpoints.Controllers.Vendor
{
    [ApiController]
    [Route("api/vendor/products")]
    [Authorize(Roles = $"{RoleConstants.SuperAdministrator},{RoleConstants.Vendor}")]
    public class VendorProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IProductVariantService _productVariantService;
        private readonly IImageService _imageService;
        private readonly IProductAttributeService _productAttributeService;
        private readonly IProductTagService _productTagService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateProductRequest> _createProductValidator;
        private readonly IValidator<UpdateProductRequest> _updateProductValidator;
        private readonly IValidator<VendorProductSearchRequest> _searchValidator;

        public VendorProductController(
            IProductService productService,
            IProductVariantService productVariantService,
            IImageService imageService,
            IProductAttributeService productAttributeService,
            IProductTagService productTagService,
            IMapper mapper,
            IValidator<CreateProductRequest> createProductValidator,
            IValidator<UpdateProductRequest> updateProductValidator,
            IValidator<VendorProductSearchRequest> searchValidator)
        {
            _productService = productService;
            _productVariantService = productVariantService;
            _imageService = imageService;
            _productAttributeService = productAttributeService;
            _productTagService = productTagService;
            _mapper = mapper;
            _createProductValidator = createProductValidator;
            _updateProductValidator = updateProductValidator;
            _searchValidator = searchValidator;
        }

        private string GetCurrentUserId()
        {
            return User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                   ?? User.FindFirst("sub")?.Value
                   ?? throw new System.UnauthorizedAccessException("User ID not found in token.");
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<VendorProductResponse>>> GetProducts(
             [FromQuery] VendorProductSearchRequest request,
             CancellationToken cancellationToken)
        {
            await _searchValidator.ValidateAndThrowAsync(request, cancellationToken);

            var (products, totalCount) = await _productService.GetPagedAsync(
                request.PageNumber,
                request.PageSize,
                request.SearchTerm,
                request.CategoryId,
                request.BrandId,
                null, // status
                request.MinPrice,
                request.MaxPrice,
                request.SortBy,
                request.SortAscending,
                cancellationToken);

            var productResponses = _mapper.Map<IEnumerable<VendorProductResponse>>(products);

            return Ok(new PagedResponse<VendorProductResponse>
            {
                Items = productResponses,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            });
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<VendorProductResponse>> GetProduct(int id, CancellationToken cancellationToken)
        {
            var vendorUserId = GetCurrentUserId();
            var product = await _productService.GetByIdAsync(id, vendorUserId, cancellationToken);

            if (product == null)
                throw new NotFoundException("Product", id);

            var productResponse = _mapper.Map<VendorProductResponse>(product);
            return Ok(productResponse);
        }

        [HttpPost]
        public async Task<ActionResult<VendorProductResponse>> CreateProduct(
            [FromBody] CreateProductRequest request,
            CancellationToken cancellationToken)
        {
            await _createProductValidator.ValidateAndThrowAsync(request, cancellationToken);

            var vendorUserId = GetCurrentUserId();

            if (await _productService.ExistsBySkuAsync(request.Sku, cancellationToken))
            {
                throw new DuplicateEntityException($"A product with SKU '{request.Sku}' already exists.");
            }

            Money? originalPriceMoney = null;
            if (request.OriginalPrice.HasValue)
            {
                originalPriceMoney = Money.Create(request.OriginalPrice.Value, "USD");
            }

            // Updated CreateAsync call to include request.ImageUrls
            var product = await _productService.CreateAsync(
                request.Name,
                request.Description,
                Money.Create(request.Price, "USD"),
                request.BrandId,
                request.CategoryId,
                request.Sku,
                Dimensions.Create(request.Dimensions.Length, request.Dimensions.Width, request.Dimensions.Height, "cm"),
                Weight.Create(request.Weight, "kg"),
                vendorUserId,
                request.MetaTitle,
                request.MetaDescription,
                request.ImageUrls, // PASS THE LIST OF IMAGE URLS
                cancellationToken);

            var productResponse = _mapper.Map<VendorProductResponse>(product);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, productResponse);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<VendorProductResponse>> UpdateProduct(
            int id,
            [FromBody] UpdateProductRequest request,
            CancellationToken cancellationToken)
        {
            await _updateProductValidator.ValidateAndThrowAsync(request, cancellationToken);

            var vendorUserId = GetCurrentUserId();
            var product = await _productService.GetByIdAsync(id, vendorUserId, cancellationToken);

            if (product == null) // اگر محصول یافت نشد (مالکیت ندارد)
                throw new NotFoundException("Product", id);

            if (await _productService.ExistsBySkuAsync(request.Sku, cancellationToken) && product.Sku != request.Sku)
            {
                throw new DuplicateEntityException($"A product with SKU '{request.Sku}' already exists.");
            }

            // Updated UpdateAsync call to include request.ImageUrls
            await _productService.UpdateAsync(
                id,
                request.Name,
                request.Description,
                Money.Create(request.Price, "USD"),
                request.OriginalPrice.HasValue ? Money.Create(request.OriginalPrice.Value, "USD") : null,
                Dimensions.Create(request.Dimensions.Length, request.Dimensions.Width, request.Dimensions.Height, "cm"),
                Weight.Create(request.Weight, "kg"),
                request.MetaTitle,
                request.MetaDescription,
                request.ImageUrls, // PASS THE LIST OF IMAGE URLS
                cancellationToken);

            var updatedProduct = await _productService.GetByIdAsync(id, vendorUserId, cancellationToken);
            var productResponse = _mapper.Map<VendorProductResponse>(updatedProduct);
            return Ok(productResponse);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id, CancellationToken cancellationToken)
        {
            var vendorUserId = GetCurrentUserId();

            // ارسال vendorUserId به متد DeleteAsync بسیار مهم است
            // این متد طبق تغییرات قبلی، چک می‌کند که آیا محصول متعلق به این فروشنده است یا خیر
            // و نیازی به GetAsync جداگانه قبل از آن نیست (چون خود DeleteAsync وجود را چک می‌کند)
            await _productService.DeleteAsync(id, vendorUserId, cancellationToken);

            return NoContent();
        }
        [HttpPost("{id}/publish")]
        public async Task<IActionResult> PublishProduct(int id, CancellationToken cancellationToken)
        {
            var vendorUserId = GetCurrentUserId();
            var product = await _productService.GetByIdAsync(id, vendorUserId, cancellationToken);

            if (product == null)
                throw new NotFoundException("Product", id);

            await _productService.PublishAsync(id, cancellationToken);
            return NoContent();
        }

        [HttpPost("{id}/unpublish")]
        public async Task<IActionResult> UnpublishProduct(int id, CancellationToken cancellationToken)
        {
            var vendorUserId = GetCurrentUserId();
            var product = await _productService.GetByIdAsync(id, vendorUserId, cancellationToken);

            if (product == null)
                throw new NotFoundException("Product", id);

            await _productService.UnpublishAsync(id, cancellationToken);
            return NoContent();
        }

        [HttpPost("{id}/archive")]
        public async Task<IActionResult> ArchiveProduct(int id, CancellationToken cancellationToken)
        {
            var vendorUserId = GetCurrentUserId();
            var product = await _productService.GetByIdAsync(id, vendorUserId, cancellationToken);

            if (product == null)
                throw new NotFoundException("Product", id);

            await _productService.ArchiveAsync(id, cancellationToken);
            return NoContent();
        }

        [HttpPost("{id}/feature")]
        public async Task<IActionResult> FeatureProduct(int id, CancellationToken cancellationToken)
        {
            var vendorUserId = GetCurrentUserId();
            var product = await _productService.GetByIdAsync(id, vendorUserId, cancellationToken);

            if (product == null)
                throw new NotFoundException("Product", id);

            await _productService.SetAsFeaturedAsync(id, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id}/feature")]
        public async Task<IActionResult> UnfeatureProduct(int id, CancellationToken cancellationToken)
        {
            var vendorUserId = GetCurrentUserId();
            var product = await _productService.GetByIdAsync(id, vendorUserId, cancellationToken);

            if (product == null)
                throw new NotFoundException("Product", id);

            await _productService.RemoveFromFeaturedAsync(id, cancellationToken);
            return NoContent();
        }

        [HttpPost("{id}/stock")]
        public async Task<IActionResult> UpdateStock(int id, [FromBody] UpdateStockRequest request, CancellationToken cancellationToken)
        {
            var vendorUserId = GetCurrentUserId();
            var product = await _productService.GetByIdAsync(id, vendorUserId, cancellationToken);

            if (product == null)
                throw new NotFoundException("Product", id);

            await _productService.UpdateStockQuantityAsync(id, request.Quantity, cancellationToken);
            return NoContent();
        }

        [HttpPost("{id}/slug")]
        public async Task<IActionResult> UpdateSlug(int id, [FromBody] UpdateSlugRequest request, CancellationToken cancellationToken)
        {
            var vendorUserId = GetCurrentUserId();
            var product = await _productService.GetByIdAsync(id, vendorUserId, cancellationToken);

            if (product == null)
                throw new NotFoundException("Product", id);

            await _productService.SetSlugAsync(id, request.Title, cancellationToken);
            return NoContent();
        }
    }

    public class UpdateStockRequest
    {
        public int Quantity { get; set; }
    }

    public class UpdateSlugRequest
    {
        public string Title { get; set; }
    }
}