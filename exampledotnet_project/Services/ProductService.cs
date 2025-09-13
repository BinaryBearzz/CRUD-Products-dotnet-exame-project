using exampledotnet_project.DTOs;
using exampledotnet_project.Models;
using exampledotnet_project.Repositories;

namespace exampledotnet_project.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IProductRepository productRepository, ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return products.Select(MapToResponseDto);
        }

        public async Task<ProductResponseDto?> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return product != null ? MapToResponseDto(product) : null;
        }

        public async Task<ProductResponseDto> CreateProductAsync(CreateProductDto createProductDto)
        {
            var product = new Product
            {
                Name = createProductDto.Name,
                Description = createProductDto.Description,
                Price = createProductDto.Price,
                Stock = createProductDto.Stock,
                Category = createProductDto.Category,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdProduct = await _productRepository.CreateAsync(product);
            _logger.LogInformation("Product created: {ProductName} with ID {ProductId}", 
                createdProduct.Name, createdProduct.Id);
            
            return MapToResponseDto(createdProduct);
        }

        public async Task<ProductResponseDto?> UpdateProductAsync(int id, UpdateProductDto updateProductDto)
        {
            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null)
            {
                return null;
            }

            // Only update fields that are provided (not null)
            if (!string.IsNullOrEmpty(updateProductDto.Name))
                existingProduct.Name = updateProductDto.Name;
            
            if (updateProductDto.Description != null)
                existingProduct.Description = updateProductDto.Description;
            
            if (updateProductDto.Price.HasValue)
                existingProduct.Price = updateProductDto.Price.Value;
            
            if (updateProductDto.Stock.HasValue)
                existingProduct.Stock = updateProductDto.Stock.Value;
            
            if (updateProductDto.Category != null)
                existingProduct.Category = updateProductDto.Category;

            var updatedProduct = await _productRepository.UpdateAsync(id, existingProduct);
            if (updatedProduct != null)
            {
                _logger.LogInformation("Product updated: {ProductName} with ID {ProductId}", 
                    updatedProduct.Name, updatedProduct.Id);
            }
            
            return updatedProduct != null ? MapToResponseDto(updatedProduct) : null;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var result = await _productRepository.DeleteAsync(id);
            if (result)
            {
                _logger.LogInformation("Product deleted with ID {ProductId}", id);
            }
            return result;
        }

        public async Task<IEnumerable<ProductResponseDto>> GetProductsByCategoryAsync(string category)
        {
            var products = await _productRepository.GetByCategoryAsync(category);
            return products.Select(MapToResponseDto);
        }

        public async Task<IEnumerable<ProductResponseDto>> SearchProductsByNameAsync(string name)
        {
            var products = await _productRepository.SearchByNameAsync(name);
            return products.Select(MapToResponseDto);
        }

        private static ProductResponseDto MapToResponseDto(Product product)
        {
            return new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                Category = product.Category,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt
            };
        }
    }
}