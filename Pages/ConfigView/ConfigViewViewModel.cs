using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using OneDriveDriver.Desktop.Utils;
using OneDriveDriver.Desktop.ViewModels;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace OneDriveDriver.Desktop.Pages.ConfigView;

public partial class ConfigViewViewModel : ViewModelBase {
    private readonly HttpClient _httpClient = new HttpClient() {
        BaseAddress = new Uri(Global.ENDPOINT)
    };

    [ObservableProperty] private string _username;
    [ObservableProperty] private string _password;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsLoggedIn))]
    private string? _accessToken;

    public bool IsLoggedIn => !string.IsNullOrWhiteSpace(AccessToken);

    [RelayCommand]
    public async Task LoginAsync() {
        try {
            var loginRequest = new LoginRequest {
                Username = Username,
                Password = Password
            };
            var loginJson = JsonConvert.SerializeObject(loginRequest);
            Console.WriteLine(loginJson);
            var response = await _httpClient.PostAsync(
                "/api/admin/login",
                new StringContent(loginJson, Encoding.UTF8, new MediaTypeHeaderValue("application/json"))
            );
            response.EnsureSuccessStatusCode();
            var loginResponse =
                JsonConvert.DeserializeObject<LoginResponse>(await response.Content.ReadAsStringAsync());
            if (loginResponse is not null) {
                AccessToken = loginResponse.AccessToken;
            }

            Console.WriteLine((string?)AccessToken);

            Console.WriteLine("is logged in");
        } catch (HttpRequestException e) {
            Console.WriteLine(e.Message);
        } catch (Exception e) {
            Console.Error.WriteLine(e);
        }
    }
}

public class LoginRequest {
    [JsonProperty(PropertyName = "username")]
    public string Username { get; set; } = null!;

    [JsonProperty(PropertyName = "password")]
    public string Password { get; set; } = null!;
}

public class LoginResponse {
    public uint Id { get; private set; }
    public string Username { get; private set; } = null!;

    [JsonProperty(PropertyName = "accessToken")]
    public string AccessToken { get; private set; } = null!;
}