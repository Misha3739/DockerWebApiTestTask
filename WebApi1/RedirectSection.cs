namespace WebApi1
{
    public class RedirectSection
    {
        public string Host { get; set; }
        
        public string Port { get; set; }
    }

    public class RedirectConfigSection
    {
        public RedirectSection Http { get; set; }
        
        public RedirectSection Https { get; set; }
    }
}