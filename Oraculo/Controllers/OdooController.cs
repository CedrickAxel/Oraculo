using Microsoft.AspNetCore.Mvc;
using Oraculo.Data.Repositories;
using Oraculo.Models;
using System;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Oraculo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OdooController : ControllerBase
    {
        private readonly IOdooRepository _odooRepository;

        public OdooController(IOdooRepository odooRepository)
        {
            _odooRepository = odooRepository;
        }

        // GET: api/Odoo/1
        [HttpGet("{environment}")]
        public async Task<IActionResult> GetProducts(int environment)
        {
            try
            {
                var productList = await _odooRepository.GetProductsAsync(environment);

                var response = new Response<List<OdooProductStockPrice>>
                {
                    Code = 200,
                    Description = "Consulta exitosa",
                    Data = productList
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                var error = new Response<string>
                {
                    Code = 500,
                    Description = "Error al obtener los datos: " + ex.Message,
                    Data = null
                };

                return StatusCode(500, error);
            }
        }
    }
}