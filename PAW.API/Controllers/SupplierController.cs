using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class SupplierController(ILogger<SupplierController> logger, ISupplierRepository supplierRepository) : ControllerBase
    {
        [HttpGet(Name = "GetSuppliers")]
        public async Task<IEnumerable<SupplierDTO>> GetAll()
        {
            var suppliers = await supplierRepository.ReadAsync() ?? [];
            return suppliers.Select(SupplierDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetSupplierById")]
        public async Task<ActionResult<SupplierDTO>> GetById(int id)
        {
            var supplier = await supplierRepository.FindAsync(id);
            return SupplierDTO.ConvertFrom(supplier);
        }

        /*[HttpPost("filter", Name = "FilterSuppliers")]
        public async Task<IEnumerable<Supplier>> Filter(ConditionViewModel condition)
        {
            var predicate = ConditionResolver<Supplier>.ResolveCondition(condition.Criteria, condition.Property, condition.Value, condition.Start, condition.End);
            var results = await businessSupplier.Filter(predicate);
            return results;
        }*/

        [HttpPost]
        public async Task<bool> Save([FromBody] IEnumerable<Supplier> suppliers)
        {
            foreach (var p in suppliers)
            {
                if (p.SupplierId > 0)
                    await supplierRepository.UpdateAsync(p);
                else
                    await supplierRepository.CreateAsync(p);
            }

            /*Suppliers.ToList().ForEach(async x =>
            {
                if (x.Id > 0)
                    await supplierRepository.CreateAsync(x);
                else
                    await supplierRepository.UpdateAsync(x);
            });*/
            return true;
        }

        [HttpDelete("{id:int}")]
        public async Task<bool> Delete(int id)
        {
            var supplier = await supplierRepository.FindAsync(id);
            if (supplier == null) return false;
            return await supplierRepository.DeleteAsync(supplier);
        }
    }
}