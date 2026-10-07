namespace PAW.Web.Services;

public abstract class ServiceBase
{
    protected string BaseUrl { get; set; } = "https://localhost:7038/";

    // Ensure trailing slash so relative paths (like id) are appended correctly by HttpClient
    protected string SetPathUrl(string name) => $"{BaseUrl}{name}/";
}
