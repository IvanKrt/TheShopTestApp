namespace TheShop.Models.Exceptions
{
    public class NoMatchingArticleException : BusinessException
    {
        public NoMatchingArticleException() : base("No matching article for your request.")
        {
        }
    }
}
