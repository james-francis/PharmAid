using AlexaSkillsKit.Speechlet;
using PharmAid.Alexa;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace PharmAid.RX
{
    public class PharmStatus
    {
        // This is Azure Function which would simulate a restful call to a pharmacy system API.
        public static string RxUrl = "http://pharmapi.azurewebsites.net/api/PAFunc1";

        // This method is invoked when the user want to get his prescription filled.
        public static async Task<SpeechletResponse> FillRxIntent(Session session, HttpClient httpClient)
        {

            string httpResultString = "";

            httpClient.DefaultRequestHeaders.Clear();
            var httpResponseMessage = await httpClient.GetAsync(RxUrl+"?action=pharmFill");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                httpResultString = await httpResponseMessage.Content.ReadAsStringAsync();
            }
            else
            {
                httpResponseMessage.Dispose();
                return AlexaUtils.BuildSpeechletResponse(new AlexaUtils.SimpleIntentResponse() { cardText = AlexaConstants.AppErrorMessage }, true);
            }

            // prescription is hard-coded to Crestor
            var simpleIntentResponse = ParseResults(httpResultString, "Prescription for Crestor");
            httpResponseMessage.Dispose();
            return AlexaUtils.BuildSpeechletResponse(simpleIntentResponse, true);
        }

        // This method is invoked when the user wants to know where his prescription is.
        public static async Task<SpeechletResponse> FindRxIntent(Session session, HttpClient httpClient)
        {

            string httpResultString = "";

            httpClient.DefaultRequestHeaders.Clear();
            var httpResponseMessage = await httpClient.GetAsync(RxUrl + "?action=pharmFind");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                httpResultString = await httpResponseMessage.Content.ReadAsStringAsync();
            }
            else
            {
                httpResponseMessage.Dispose();
                return AlexaUtils.BuildSpeechletResponse(new AlexaUtils.SimpleIntentResponse() { cardText = AlexaConstants.AppErrorMessage }, true);
            }

            // prescription is hard-coded to Crestor
            var simpleIntentResponse = ParseResults(httpResultString, "Prescription for Crestor");
            httpResponseMessage.Dispose();
            return AlexaUtils.BuildSpeechletResponse(simpleIntentResponse, true);
        }

        // This method is invoked when the user wants to know where his prescription is.
        public static async Task<SpeechletResponse> WhenRxIntent(Session session, HttpClient httpClient)
        {

            string httpResultString = "";

            httpClient.DefaultRequestHeaders.Clear();
            var httpResponseMessage = await httpClient.GetAsync(RxUrl + "?action=pharmWhen");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                httpResultString = await httpResponseMessage.Content.ReadAsStringAsync();
            }
            else
            {
                httpResponseMessage.Dispose();
                return AlexaUtils.BuildSpeechletResponse(new AlexaUtils.SimpleIntentResponse() { cardText = AlexaConstants.AppErrorMessage }, true);
            }

            // prescription is hard-coded to Crestor
            var simpleIntentResponse = ParseResults(httpResultString, "Prescription for Crestor");
            httpResponseMessage.Dispose();
            return AlexaUtils.BuildSpeechletResponse(simpleIntentResponse, true);
        }

        // This method is invoked when the user wants to know where his prescription is.
        public static async Task<SpeechletResponse> CallDoctorIntent(Session session, HttpClient httpClient)
        {

            string httpResultString = "";

            httpClient.DefaultRequestHeaders.Clear();
            var httpResponseMessage = await httpClient.GetAsync(RxUrl + "?action=pharmCall");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                httpResultString = await httpResponseMessage.Content.ReadAsStringAsync();
            }
            else
            {
                httpResponseMessage.Dispose();
                return AlexaUtils.BuildSpeechletResponse(new AlexaUtils.SimpleIntentResponse() { cardText = AlexaConstants.AppErrorMessage }, true);
            }

            var simpleIntentResponse = ParseResults(httpResultString, "Doctor");
            httpResponseMessage.Dispose();
            return AlexaUtils.BuildSpeechletResponse(simpleIntentResponse, true);
        }

        // This method parses the json object returned from the Azure function.
        private static AlexaUtils.SimpleIntentResponse ParseResults(string resultString, string rx)
        {
            string stringToRead = String.Empty;
            string stringForCard = String.Empty;

            // you'll need to use JToken instead of JObject with results
            dynamic resultObject = JToken.Parse(resultString);

            // if you're into structured data objects, use JArray
            // JArray resultObject2 = JArray.Parse(resultString);
            stringToRead = resultObject.message;

            // Build the response
            if (stringForCard == String.Empty && stringToRead == String.Empty)
            {
                string noRx = "Unable to find a valid prescription";
                stringToRead += Alexa.AlexaUtils.AddSpeakTagsAndClean(noRx);
                stringForCard = noRx;
            }
            else
            {
                stringForCard = rx + " " + stringToRead;
                stringToRead = Alexa.AlexaUtils.AddSpeakTagsAndClean(rx + " " + stringToRead);
            }
            
            return new AlexaUtils.SimpleIntentResponse() { cardText = stringForCard, ssmlString = stringToRead };

            // if you want to add images, you can include them in the reply
            // images should be placed into the ~/Images/ folder of this project
            // 

            // JPEG or PNG supported, no larger than 2MB
            // 720x480 - small size recommendation
            // 1200x800 - large size recommendation
            /*
            return new AlexaUtils.SimpleIntentResponse()
            {
                cardText = stringForCard,
                ssmlString = stringToRead,
                largeImage = "msft.png",
                smallImage = "msft.png",
            };
            */
        }
    }
} 
