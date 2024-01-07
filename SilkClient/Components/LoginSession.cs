

using System.Security.Cryptography;
using SilkClient.api;
using System.Text;

namespace SilkClient.Components;
public class LoginSession
{
    public string BearerToken { get; set; } = "";
    public bool LoggedIn { get; set; } = false;
    public string User { get; set; } = "";

    public event Action OnChange;

    public async Task<string> HashPassword(string username, string password)
    {
        var client = new SilkApiClient("http://localhost:5293", new HttpClient());
        var salt = await client.LoginSaltAsync(new LoginSaltRequest() { Username = username });

        StringBuilder hashBuilder = new StringBuilder();
        using (SHA256 sha = SHA256.Create())
        {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = sha.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
            foreach (byte b in data){
                hashBuilder.AppendFormat("{0:x2}", b);
            }
        }
        return hashBuilder.ToString();
    }

    public async Task Logout()
    {
        var client = new SilkApiClient("http://localhost:5293", new HttpClient());
        await client.LogoutAsync();
        LoggedIn = false;
        User = "";
        NotifyLogin();
    }


    public void NotifyLogin() {
        OnChange?.Invoke();
    }
    
}