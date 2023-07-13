namespace RealEstateWebAPI.Middleware
{
    /// <summary>
    /// Specifikon nese nje metode ose nje klase lejon anonymous access
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class AllowAnonymousAttribute : Attribute
    {
    }
}
