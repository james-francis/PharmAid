using System.Web.Configuration;

namespace PharmAid.Alexa
{
    public class AlexaConstants
    {
        // Inbound requests from Amazon include the Voice Skills AppId, assigned to you when registering your skill
        // Validate the value against what you registered, to ensure that someone else isn't calling your service.  
        public static string AppId = WebConfigurationManager.AppSettings["AppId"];

        // The value of AppName has no correspondence to what you have registered in Amazon
        // we just store it here because it's useful. It does appear on the card that is shown to the user
        // in the Alexa Companion App
        public static string AppName = WebConfigurationManager.AppSettings["AppName"];

        // standard error message
        public static string AppErrorMessage = "Sorry, something went wrong.  Please try again.";
    }
}
