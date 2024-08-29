using API.RequestHelpers;
using Core.Entites;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseAPIController : ControllerBase
    {
        protected async Task<ActionResult<T>> CreatePageResult<T>(IGenericRepository<T> repo, 
            ISpecification<T> spec, int PageIndex, int PageSize ) where T : BaseEntity
        {
           var items = await repo.ListAsync(spec);
           var count = await repo.CountAsync(spec);
           
           var pagination = new Pagination<T>(PageIndex, PageSize, count, items);
              return Ok(pagination);
           
        }
        
        
    }
}