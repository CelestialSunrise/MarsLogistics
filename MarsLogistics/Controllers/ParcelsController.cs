using MarsLogistics.Models;
using MarsLogistics.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MarsLogistics.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParcelsController : ControllerBase
    {
        private readonly IParcelService _service;

        public ParcelsController(IParcelService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterParcel([FromBody] Parcel parcel)
        {
            var result = await _service.RegisterParcelAsync(parcel);
            return Ok(result);
        }

        [HttpGet("{barcode}")]
        public async Task<IActionResult> GetParcel(string barcode)
        {
            var parcel = await _service.GetParcelAsync(barcode);
            return parcel is null ? NotFound() : Ok(parcel);
        }

        [HttpPatch("{barcode}")]
        public async Task<IActionResult> UpdateStatus(string barcode, [FromBody] StatusUpdateRequest request)
        {
            var updated = await _service.UpdateStatusAsync(barcode, request.NewStatus);
            return updated is null ? NotFound() : Ok(updated);
        }
    }

    public class StatusUpdateRequest
    {
        public ParcelStatus NewStatus { get; set; }
    }
}

