using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace TakeHomeAssignment.General.AccountAPI.UnitTest
{
    public class BaseControllerTest
    {
        public ITestOutputHelper _output;

        public BaseControllerTest(ITestOutputHelper output)
        {
            _output = output;
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
            Environment.SetEnvironmentVariable("THA_JWT_SIGNING_KEY", "85a2bc50c48feda8ad7e0c04e703e4e4");
        }

        public void AssertJson(object ExpectedObject, object ActualObject)
        {
            string SerializeExpected = JsonConvert.SerializeObject(ExpectedObject);
            string SerializeActual = JsonConvert.SerializeObject(ActualObject);
            _output.WriteLine("Expected: " + SerializeExpected);
            _output.WriteLine("Actual: " + SerializeActual);
            Assert.Equal(SerializeExpected, SerializeActual);
        }

        public static void AssertResponseStatus(HttpStatusCode ExpectedStatusCode, HttpStatusCode ActualStatusCode)
        {
            Assert.Equal(ExpectedStatusCode, ActualStatusCode);
        }

        public static void AssertResponseStatusOK(HttpStatusCode ActualStatusCode)
        {
            AssertResponseStatus(HttpStatusCode.OK, ActualStatusCode);
        }

        public static void AssertResponseStatusInvalidParameter(HttpStatusCode ActualStatusCode)
        {
            AssertResponseStatus(HttpStatusCode.BadRequest, ActualStatusCode);
        }

        public static void AssertResponseStatusUnauthorized(HttpStatusCode ActualStatusCode)
        {
            AssertResponseStatus(HttpStatusCode.Unauthorized, ActualStatusCode);
        }

        public static void AssertResponseStatus404(HttpStatusCode ActualStatusCode)
        {
            AssertResponseStatus(HttpStatusCode.NotFound, ActualStatusCode);
        }

        public static void AssertResponseStatusMethodNotSupported(HttpStatusCode ActualStatusCode)
        {
            AssertResponseStatus(HttpStatusCode.MethodNotAllowed, ActualStatusCode);
        }

        public static void AssertResponseStatusErrorUnhandled(HttpStatusCode ActualStatusCode)
        {
            AssertResponseStatus(HttpStatusCode.InternalServerError, ActualStatusCode);
        }
    }
}
