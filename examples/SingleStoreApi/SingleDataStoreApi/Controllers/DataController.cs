using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DataStoreFramework.Data;
using DataStoreFramework.Providers;

namespace SingleDataStoreApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataController : ControllerBase
    {
        private readonly ILogger<DataController> _logger;

        private readonly IDataStoreProvider _dataStore;

        public DataController(ILogger<DataController> logger, IDataStoreProvider dataStore)
        {
            _logger = logger;
            _dataStore = dataStore;
        }

        [HttpGet("{id}/metadata")]
        public async Task<ObjectMetadata> Get(string id)
        {
            _logger.LogDebug($"Retrieving metadata for: {id}");

            return await _dataStore.GetMetadataAsync(id, HttpContext.RequestAborted);
        }
    }
}
