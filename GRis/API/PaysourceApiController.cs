using Gris.Application.Core.Interfaces;
using System.Linq;
using System.Web.Http;

namespace GRis.API
{
    [Authorize]
    [RoutePrefix("api/paysource")]
    public class PaysourceApiController : ApiController
    {
        private readonly IPaySourceService _paySourceService;

        public PaysourceApiController(IPaySourceService paySourceService)
        {
            _paySourceService = paySourceService;
        }

        [Route("{paysourceId:int}/programs")]
        public IHttpActionResult GetPrgramsByPaysourceId(int paysourceId)
        {
            var result = _paySourceService.GetById(paysourceId).Programs.OrderBy(t => t.Name).Select(t => new
            {
                Text = t.Name,
                Value = t.Id.ToString()
            });
            return Ok(result);
        }
    }
}