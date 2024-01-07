

using System.Security.Cryptography;
using SilkClient.api;
using System.Text;

namespace SilkClient.Components;
public class ApiClientFactory
{
    LoginSession _session;

    public ApiClientFactory(LoginSession session)
    {
        _session = session;
    }

    public SilkApiClient CreateClient()
    {
        var httpClient = new HttpClient();
        if (_session.LoggedIn){
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _session.BearerToken);
        }
        return new SilkApiClient("http://localhost:5293", httpClient);
    }
    
}