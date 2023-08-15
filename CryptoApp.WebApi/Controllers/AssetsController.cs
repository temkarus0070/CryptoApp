using CryptoApp.database;
using CryptoApp.WebApi.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace CryptoApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AssetsController : ControllerBase
    {
        private CryptoService cryptoService;

        public AssetsController(CryptoService cryptoService)
        {
            this.cryptoService = cryptoService;
        }

        [HttpGet]
        public async Task<IEnumerable<CryptoAsset>> Get()
        {
            return await cryptoService.CryptoAssets();
        }
    }
}
