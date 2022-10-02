using BackEndDotnetPlumsail.Data.Models;
using BackEndDotnetPlumsail.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndDotnetPlumsail.Controllers
{
    public class RationController : BaseController
    {
        private readonly IRationRepository _rationRepository;
        private IElasticSearchService _elasticsearchService;
        public RationController(IRationRepository rationRepository, IElasticSearchService elasticsearchService)
        {
            _rationRepository = rationRepository;
            _elasticsearchService = elasticsearchService;
        }

        [HttpGet]
        [Route("GetCurrentAppVersion")]
        public async Task<IActionResult> GetCurrentAppVersion()
        {
            try
            {
                return Ok(await _rationRepository.GetCurrentAppVersion());
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet]
        [Route("GetCurrentAppVersionFile")]
        public async Task<IActionResult> GetCurrentAppVersionFile()
        {
            try
            {
                var fileName = $"Application_{DateTime.Now.ToString("dd.MM.yyyy_hh:mm:sss")}.html";
                var mimeType = "text/html";

                Response.Clear();
                Response.Headers.Add("content-disposition", String.Format("inline;filename=\"{0}\"", fileName));
                Response.ContentType = mimeType;
                var res = await _rationRepository.GetCurrentAppVersionFile();

                return File(res, mimeType);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet]
        [Route("GetAppVersionFile")]
        public async Task<IActionResult> GetAppVersionFile(int id)
        {
            try
            {
                var fileName = $"Application_{DateTime.Now.ToString("dd.MM.yyyy_hh:mm:sss")}.html";
                var mimeType = "text/html";

                Response.Clear();
                Response.Headers.Add("content-disposition", String.Format("inline;filename=\"{0}\"", fileName));
                Response.ContentType = mimeType;
                var res = await _rationRepository.GetAppVersion(id);

                return File(res, mimeType);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet]
        [Route("GetAppVersionByRationId")]
        public async Task<IActionResult> GetAppVersionByRationId(int id)
        {
            try
            {
                var fileName = $"Application_{DateTime.Now.ToString("dd.MM.yyyy_hh:mm:sss")}.html";
                var mimeType = "text/html";

                Response.Clear();
                Response.Headers.Add("content-disposition", String.Format("inline;filename=\"{0}\"", fileName));
                Response.ContentType = mimeType;
                var res = await _rationRepository.GetAppVersionByRationId(id);

                return File(res, mimeType);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet]
        [Route("GetAppVersionByRationIdElastic")]
        public async Task<IActionResult> GetAppVersionByRationIdElastic(string id)
        {
            try
            {
                var fileName = $"Application_{DateTime.Now.ToString("dd.MM.yyyy_hh:mm:sss")}.html";
                var mimeType = "text/html";

                Response.Clear();
                Response.Headers.Add("content-disposition", String.Format("inline;filename=\"{0}\"", fileName));
                Response.ContentType = mimeType;
                var res = await _elasticsearchService.GetAppVersionByRationId(id);

                return File(res, mimeType);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPost]
        [Route("AddRation")]
        public async Task<IActionResult> AddRation([FromBody] Ration ration)
        {
            try
            {
                return Ok(await _rationRepository.AddRation(ration));
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPost]
        [Route("AddRationElastic")]
        public async Task<IActionResult> AddRationElastic([FromBody] dynamic ration)
        {
            try
            {
                await _elasticsearchService.InsertRation(ration);
                return Ok();
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet]
        [Route("SearchRations")]
        public async Task<IActionResult> SearchRations(string query)
        {
            try
            {
                return Ok(await _rationRepository.SearchRations(query));
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet]
        [Route("SearchRationsElastic")]
        public async Task<IActionResult> SearchRationsElastic(string query)
        {
            try
            {
                return Ok(await _elasticsearchService.SearchRations(query));
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }


        [HttpGet]
        [Route("GetRation")]
        public async Task<IActionResult> GetRation(int id)
        {
            try
            {
                return Ok(await _rationRepository.GetRation(id));
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet]
        [Route("GetRationElastic")]
        public async Task<IActionResult> GetRationElastic(string id)
        {
            try
            {
                return Ok(await _elasticsearchService.GetRation(id));
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
