using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskProcessor.Application;
using TaskProcessor.Application.DTOs;
using TaskProcessor.Application.interfaces;
using TaskProcessor.Application.Interfaces;
using TaskProcessor.Models;

namespace TaskProcessor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly IJobApplicationService _jobAppService;

        public JobsController(IJobApplicationService jobAppService)
        {
            _jobAppService = jobAppService;
        }

        [HttpGet("seed")]
        public async Task<IActionResult> CriarJobExemplo()
        {
            var job = new JobRequest
            {
                Tipo = "EmailJob",
                Dados = "email@email"
            };

            var result = await _jobAppService.CriarJobAsync(job);
            if (result == null)
                return BadRequest("Erro ao criar o job.");

            return Ok(new { job = job, result = result });
        }

        [HttpPost]
        [Route("send")]
        public async Task<IActionResult> CreateJob([FromBody] JobRequest jobRequest)
        {
            var result = await _jobAppService.CriarJobAsync(jobRequest);
            if (result == null)
                return BadRequest("Erro ao criar o job.");

            return Ok(new { job = jobRequest, result = result });
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetStatus(string id)
        {
            var status = await _jobAppService.ObterPorIdAsync(id);
            if (status == null)
                return BadRequest();

            return Ok(status);
        }
    }
}
