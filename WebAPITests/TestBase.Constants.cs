namespace WebAPITests;
public partial class TestBase
{
    protected const string WebApiRoute = "https://localhost:4300";
    public static class Routes
    {
        public static class WebApi
        {
            public static string Dogs => $"{WebApiRoute}/dogs";
            public static string Dog => $"{WebApiRoute}/dog";

        }
    }
}
