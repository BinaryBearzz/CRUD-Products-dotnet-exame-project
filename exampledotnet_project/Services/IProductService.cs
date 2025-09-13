using exampledotnet_project.DTOs;

namespace exampledotnet_project.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync();
        Task<ProductResponseDto?> GetProductByIdAsync(int id);
        Task<ProductResponseDto> CreateProductAsync(CreateProductDto createProductDto);
        Task<ProductResponseDto?> UpdateProductAsync(int id, UpdateProductDto updateProductDto);
        Task<bool> DeleteProductAsync(int id);
        Task<IEnumerable<ProductResponseDto>> GetProductsByCategoryAsync(string category);
        Task<IEnumerable<ProductResponseDto>> SearchProductsByNameAsync(string name);
    }
}