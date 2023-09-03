using MiniETrade.Application.Repositories.Products;
using MiniETrade.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.BusinessRules.Products
{
    public class ProductBusinessRules :BaseBusinessRules
    {
        private readonly IProductReadRepository _productReadRepository;
        public ProductBusinessRules(IProductReadRepository productReadRepository)
        {
            _productReadRepository = productReadRepository;
        }

        public async Task CheckIfProductNameIsDuplicate(string productName)
        {
            var product = await _productReadRepository.GetAsync(p => p.Name == productName);
            if (product is not null) throw new BusinessException("Please choose another product name. This product name is already used for another product. Product Id : " + product.Id);
        }
    }
}