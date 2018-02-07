using System;
using AlexaSkillsKit.Speechlet;
using AlexaSkillsKit.Slu;
using AlexaSkillsKit.Authentication;
using AlexaSkillsKit.Json;
using System.Threading.Tasks;
using System.Net.Http;

namespace PharmAid.Alexa
{
    // Follow the AlexaSkillsKit documentation and override the base class and children with our own implementation
    // the functions below map to the Alexa requests described at this URL
    // https://developer.amazon.com/public/solutions/alexa/alexa-skills-kit/docs/handling-requests-sent-by-alexa

    public class AlexaSpeechletAsync : SpeechletAsync
    {

        // Alexa provides a security wrapper around requests sent to your service, which the 
        // AlexaSkillsKit nuget package validates by default.  However, you might not want this wrapper enabled while
        // you do local development and testing - in DEBUG mode.

        // Note: the default Azure publishing option in Visual Studio is Release (not Debug), so by default the
        // security wrapper will be enabled when you publish to Azure.

        // Amazon requires that your skill validate requests sent to it for certification, so you shouldn't 
        // deploy to production with validation disabled

        //#if DEBUG

        public override bool OnRequestValidation(SpeechletRequestValidationResult result, DateTime referenceTimeUtc, SpeechletRequestEnvelope requestEnvelope)
        {
            return true;
        }

        //#endif

        // Invoked when the user begins their session
        // according to the docs we can load user data here
        public override Task OnSessionStartedAsync(SessionStartedRequest sessionStartedRequest, Session session)
        {
            // this function is invoked when a user begins a session with your skill
            // this is a chance to load user data at the start of a session

            // Respond with an error message is an invalid Alexa Application Id is passed
            if (AlexaUtils.IsRequestInvalid(session))
            {
                return Task.FromResult<SpeechletResponse>(InvalidApplicationId(session));
            }

            // return some sort of Task per function definition
            return Task.Delay(0);
        }

        // Invoked when a user ends their session
        // according to the docs we can save user data here
        public override Task OnSessionEndedAsync(SessionEndedRequest sessionEndedRequest, Session session)
        {
            // Respond with an error message is an invalid Alexa Application Id is passed
            if (AlexaUtils.IsRequestInvalid(session))
            {
                return Task.FromResult<SpeechletResponse>(InvalidApplicationId(session));
            }

            // return some sort of Task per function definition
            return Task.Delay(0);

        }

        // No intent was passed from Alexa
        public override async Task<SpeechletResponse> OnLaunchAsync(LaunchRequest launchRequest, Session session)
        {
            // Respond with an error message is an invalid Alexa Application Id is passed
            if (AlexaUtils.IsRequestInvalid(session))
            {
                return await Task.FromResult<SpeechletResponse>(InvalidApplicationId(session));
            }

            // No intent was passed
            return await Task.FromResult<SpeechletResponse>(GetOnLaunchAsyncResult(session));
        }

        // We have a valid intent passed to us from Alexa
        public override async Task<SpeechletResponse> OnIntentAsync(IntentRequest intentRequest, Session session)
        {
            // Respond with an error message is an invalid Alexa Application Id is passed
            if (AlexaUtils.IsRequestInvalid(session))
            {
                return await Task.FromResult<SpeechletResponse>(InvalidApplicationId(session));
            }

            // intentRequest.Intent.Name contains the name of the intent
            // intentRequest.Intent.Slots.* contains slot values if you're using them 
            // session.User.AccessToken contains the Oauth 2.0 access token if the user has linked to your auth system

            // Get intent from the request object
            Intent intent = intentRequest.Intent;
            string intentName = (intent != null) ? intent.Name : null;

            // Create an http client which can be reused across requests
            var httpClient = new HttpClient();

            // Since we have a valid intent, we can process the request in our PharmStatus class
            switch (intentName)
            {
               case ("FillRxIntent"):
                    return await RX.PharmStatus.FillRxIntent(session, httpClient);
                case ("FindRxIntent"):
                    return await RX.PharmStatus.FindRxIntent(session, httpClient);
                case ("WhenRxIntent"):
                    return await RX.PharmStatus.WhenRxIntent(session, httpClient);
                case ("CallDoctorIntent"):
                    return await RX.PharmStatus.CallDoctorIntent(session, httpClient);
                default:
                    return await Task.FromResult<SpeechletResponse>(GetOnLaunchAsyncResult(session));
            }

        }

        // Called by OnLaunchAsync - when the skill is invoked without an intent.
        // Called by OnIntentAsync - when an intent is not mapped to action.
        private SpeechletResponse GetOnLaunchAsyncResult(Session session)
        {
            return AlexaUtils.BuildSpeechletResponse(new AlexaUtils.SimpleIntentResponse() { cardText = "Not sure what you are asking" }, true);
        }

        // Called when the request does not include an Alexa Skills AppId.
        private SpeechletResponse InvalidApplicationId(Session session)
        {
            return AlexaUtils.BuildSpeechletResponse(new AlexaUtils.SimpleIntentResponse()
            {
                cardText = "An invalid Application ID was received from Alexa."
            }, true);
        }
    }
}
