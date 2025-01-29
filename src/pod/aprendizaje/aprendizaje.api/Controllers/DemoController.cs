using FluentStorage.Blobs;
using Microsoft.AspNetCore.Mvc;
using servicio.almacenamiento;

namespace aprendizaje.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        private readonly IFabricaProveedorAlmacenamiento fabricaProveedor;
        public DemoController(IFabricaProveedorAlmacenamiento fabricaProveedor) {
            this.fabricaProveedor = fabricaProveedor;
        }

        [HttpGet]
        public async Task<IActionResult> Demo()
        {
            var r = await fabricaProveedor.ObtieneProveedor("demo", null);
            await r.WriteTextAsync($"doc{DateTime.Now.Ticks}.txt", $"Now {DateTime.Now.Ticks}\r\n", System.Text.Encoding.Default, CancellationToken.None);
            return Ok();
        }
    }
}
