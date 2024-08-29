using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entites;

namespace Core.Specifications
{
    public class BrandListSpecification : BaseSpecifcation<Product, string>
    {
        public BrandListSpecification()
        {
            AddSelect(x => x.Brand);    
            ApplyDistinct();
        }
    }
}