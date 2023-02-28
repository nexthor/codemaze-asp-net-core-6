using CompanyEmployees.Presentation.ModelBinders;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CompanyEmployees.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly IServiceManager _services;

        public CompaniesController(IServiceManager services)
        {
            _services = services;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetCompaniesAsync([FromQuery] CompanyParameters companyParameters)
        {
            var companies = await _services.CompanyService.GetAllCompaniesAsync(companyParameters, false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(companies.metaData));

            return Ok(companies.companies);
        }

        [HttpGet("{id:guid}", Name = "CompanyById")]
        public async Task<IActionResult> GetCompanyAsync(Guid id)
        {
            var company = await _services.CompanyService.GetCompanyAsync(id, false);

            return Ok(company);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompanyAsync([FromBody] CompanyForCreationDto company)
        {
            if (company is null)
                return BadRequest("CompanyForCreationDto object is null");

            var createdCompany = await _services.CompanyService.CreateCompanyAsync(company);

            return CreatedAtRoute("CompanyById", new { id = createdCompany.Id }, createdCompany);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateCompanyAsync(Guid id, [FromBody] CompanyForUpdateDto company)
        {
            if (company is null)
                return BadRequest("CompanyForCreationDto object is null");

            await _services.CompanyService.UpdateCompanyAsync(id, company, true);

            return NoContent();
        }

        [HttpGet("collection/({ids})", Name = "CompanyCollection")]
        public async Task<IActionResult> GetCompanyCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            var companies = await _services.CompanyService.GetByIdsAsync(ids, false);

            return Ok(companies);
        }

        [HttpPost("collection")]
        public async Task<IActionResult> CreateCompanyCollectionAsync([FromBody] IEnumerable<CompanyForCreationDto> companyCollection)
        {
            var result = await _services.CompanyService.CreateCompanyCollectionAsync(companyCollection);

            return CreatedAtRoute("CompanyCollection", new { result.ids }, result.companies);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCompanyAsync(Guid id)
        {
            await _services.CompanyService.DeleteCompanyAsync(id, false);

            return NoContent();
        }

        [HttpPost("{id:guid}/upload")]
        public async Task<IActionResult> UploadCompanyLogoAsync(Guid id)
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("StaticFiles", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition)
                                    .FileName.Trim();
                    var fullPath = Path.Combine(pathToSave, fileName.ToString());
                    var dbPath = Path.Combine(folderName, fileName.ToString());
                    
                    using(var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    return Ok(dbPath);
                }
                else
                {
                    return BadRequest("no file selected");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
