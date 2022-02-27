using System;
using Newtonsoft.Json;
using PrivacyTypes.SampleImplementations;

namespace PrivacyTypes
{
    class Program
    {
        static void Main(string[] args)
        {
            // example one
            var logger = new Logger(PrivateTypeAuthorizationContextPrivacyLevel.LOW);

            var highlyPrivateString = new HighPrivateString(YourApp.GetSecretValueFromVault("high-secret-key1"));
            var mediumPrivateString = new MediumPrivateString(YourApp.GetSecretValueFromVault("medium-secret-key1"));

            try
            {
                Console.WriteLine(mediumPrivateString);
            }
            catch (InvalidOperationException)
            {
                // fails, throws InvalidOperationException
            }

            using (var context = new PrivateTypeAuthorizationContext(PrivateTypeAuthorizationContextPrivacyLevel.VERYHIGH)) // prevent accessing secret data by isolating the access level
            {
                HighPrivateString contaminatedConcat = highlyPrivateString.Concat(mediumPrivateString, context); // mixing access levels contaminates data on a type level

                logger.Log(contaminatedConcat, context); // contents can only be logged explicitly if it is not private (logger won't log this value because the logger's level is set to LOW)
            }

            // example two

            // retrieve a person record from the server (which has confidential info)
            var unsafePerson = JsonConvert.DeserializeObject<UnsafePerson>(GetPersonJson());
            Console.WriteLine(unsafePerson.fullName); // oh no! prints "John Smith", which is bad because this is secret information

            // retrieve a person record from the server (which has confidential info) but this time, let's do it safely
            var safePerson = JsonConvert.DeserializeObject<SafePerson>(GetPersonJson());
            try
            {
                Console.WriteLine(safePerson
                    .fullName); // fails because it is private information based on the type of the model
            }
            catch (InvalidOperationException)
            {
                // ignored
            }

            
            try
            {
                Console.WriteLine(safePerson.lastName); // fails because it is private information based on the type of the model
            }
            catch (InvalidOperationException)
            {
                // ignored
            }

            // restrict when data is accessed and disposed of
            using (var context = new PrivateTypeAuthorizationContext(PrivateTypeAuthorizationContextPrivacyLevel.LOW))
            {
                logger.Log(safePerson.lastName, context); // successfully logs last name because logger is authorized to use this privacy level, and we're in a low privacy context
                logger.Log(safePerson.fullName, context); // does not work because the logger and context is set to LOW, but the privacy level of the full name is HIGH

                // safePerson.lastName + safePerson.fullName; // mixing types implicitly is prohibited (fails to compile)
                // you can code your own methods that allow for validation and concatenation for example
                // concatenating a medium private string with a high private one turns it into a high private string due to contamination
                var firstPlusFullName = highlyPrivateString.Concat(safePerson.lastName, context); 
            }
        }

        private static string GetPersonJson()
        {
            return @"{""firstName"":""John"",""lastName"":""Smith"",""fullName"":""John Smith""}";
        }
    }
}
