﻿using AlexaSkillsKit.Speechlet;
using PharmAid.Alexa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace PharmAid.RX
{
    public class Status
    {
        public static string RxUrl = "https://pharmapi.azurewebsites.net/api/HttpTriggerCSharp1?code=qRmH3eJABWVaIZJ26HVtNKqTogzkMRBzsm4acL5N6n9RMB2TTNTVLQ==";

        // Call the remote web service.  Invoked from AlexaSpeechletAsync
        // Then, call another function with the raw JSON results to generate the spoken text and card text

        // public static SpeechletResponse FillRxIntent(Session session)
        public static async Task<SpeechletResponse> FillRxIntent(Session session, HttpClient httpClient)
        {

            string httpResultString = "";

            httpClient.DefaultRequestHeaders.Clear();
            var httpResponseMessage = await httpClient.GetAsync(RxUrl+"&action=pharmFill");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                //httpResultString = httpResponseMessage.Content.ReadAsStringAsync().Result;
                httpResultString = await httpResponseMessage.Content.ReadAsStringAsync();
            }
            else
            {
                httpResponseMessage.Dispose();
                return AlexaUtils.BuildSpeechletResponse(new AlexaUtils.SimpleIntentResponse() { cardText = AlexaConstants.AppErrorMessage }, true);
            }

            // return AlexaUtils.BuildSpeechletResponse(new AlexaUtils.SimpleIntentResponse() { cardText = AlexaConstants.AppErrorMessage }, true);
            // var simpleIntentResponse = ParseResults("Your prescription will be filled");

            var simpleIntentResponse = ParseResults(httpResultString, "Prescription for Crestor");
            httpResponseMessage.Dispose();
            return AlexaUtils.BuildSpeechletResponse(simpleIntentResponse, true);
        }

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