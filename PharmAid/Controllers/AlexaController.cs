using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace PharmAid.Controllers
{
    public class AlexaController : ApiController
    {
       // you can set an explicit route if you want ...
       // [Route("alexa/alexa-session")]
        [HttpPost]
        public async Task<HttpResponseMessage> AlexaSession()
        {
            var alexaSpeechletAsync = new Alexa.AlexaSpeechletAsync();
            return await alexaSpeechletAsync.GetResponseAsync(Request);
        }

    }
}

