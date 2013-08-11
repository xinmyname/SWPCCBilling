namespace SWPCCBilling
{
    public class HelloModule : Nancy.NancyModule
    {
        public HelloModule()
        {
            Get["/"] = _ => "Hello world!";
        }
    }
}