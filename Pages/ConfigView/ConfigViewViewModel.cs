using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using OneDriveDriver.Desktop.Services;
using OneDriveDriver.Desktop.Utils;
using OneDriveDriver.Desktop.ViewModels;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace OneDriveDriver.Desktop.Pages.ConfigView;

public partial class ConfigViewViewModel : ViewModelBase {
    // TODO: 将 Config 的 API 请求行为抽离到单独的 Service 中
    private readonly HttpClient _httpClient = new() {
        BaseAddress = new Uri(Global.ENDPOINT)
    };

    private readonly NotificationService _notificationService;

    [ObservableProperty] private string _username = null!;
    [ObservableProperty] private string _password = null!;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsLoggedIn))]
    private string? _accessToken;

    [ObservableProperty] private string _onedriveRootPath = null!;
    [ObservableProperty] private string _oneDriveClientId = null!;
    [ObservableProperty] private string _oneDriveClientSecret = null!;
    [ObservableProperty] private string _oneDriveRefreshToken = null!;

    public bool IsLoggedIn => !string.IsNullOrWhiteSpace(AccessToken);

    public ConfigViewViewModel(NotificationService notificationService) {
        _notificationService = notificationService;
    }

    [RelayCommand]
    public async Task LoginAsync() {
        try {
            var loginRequest = new LoginRequest {
                Username = Username,
                Password = Password
            };
            var loginJson = JsonConvert.SerializeObject(loginRequest);
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
        } catch (HttpRequestException e) {
            _notificationService.ShowNotification(new Notification("Error", e.Message, NotificationType.Error));
            Console.Error.WriteLine(e.Message);
        } catch (Exception e) {
            Console.Error.WriteLine(e);
        }
    }

    partial void OnAccessTokenChanged(string? value) {
        if (IsLoggedIn) _ = GetOneDriveConfig();
    }

    public async Task GetOneDriveConfig() {
        try {
            if (string.IsNullOrWhiteSpace(AccessToken)) return;

            var httpMessage = new HttpRequestMessage(HttpMethod.Get, "/api/admin/onedrive-config");
            httpMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            var response = await _httpClient.SendAsync(httpMessage);
            response.EnsureSuccessStatusCode();
            var configJson = await response.Content.ReadAsStringAsync();
            var configResponse = JsonConvert.DeserializeObject<GetOneDriveConfigResponse>(configJson);
            if (configResponse is not null) {
                OneDriveClientId = configResponse.OneDriveClientId;
                OnedriveRootPath = configResponse.OneDriveRootPath;
                OneDriveClientSecret = configResponse.OneDriveClientSecret;
                OneDriveRefreshToken = configResponse.OneDriveRefreshToken;
            }
        } catch (HttpRequestException e) {
            _notificationService.ShowNotification(new Notification("Error", e.Message, NotificationType.Error));
            Console.Error.WriteLine(e.Message);
        } catch (Exception e) {
            Console.Error.WriteLine(e);
        }
    }

    [RelayCommand]
    public async Task UpdateOneDriveConfig() {
        try {
            if (string.IsNullOrWhiteSpace(AccessToken)) return;

            var httpMessage = new HttpRequestMessage(HttpMethod.Put, "/api/admin/onedrive-config");
            httpMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

            var request = new GetOneDriveConfigResponse() {
                OneDriveClientId = OneDriveClientId,
                OneDriveClientSecret = OneDriveClientSecret,
                OneDriveRefreshToken = OneDriveRefreshToken,
                OneDriveRootPath = OnedriveRootPath
            };
            httpMessage.Content =
                new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.SendAsync(httpMessage);
            response.EnsureSuccessStatusCode();
            Console.WriteLine("success");
        } catch (HttpRequestException e) {
            Console.Error.WriteLine(e.Message);
            _notificationService.ShowNotification(new Notification("Error", e.Message, NotificationType.Error));
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

public class GetOneDriveConfigResponse {
    [JsonProperty(PropertyName = "oneDriveRootPath")]
    public string OneDriveRootPath { get; set; } = null!;

    [JsonProperty(PropertyName = "oneDriveClientId")]
    public string OneDriveClientId { get; set; } = null!;

    [JsonProperty(PropertyName = "oneDriveClientSecret")]
    public string OneDriveClientSecret { get; set; } = null!;

    [JsonProperty(PropertyName = "oneDriveRefreshToken")]
    public string OneDriveRefreshToken { get; set; } = null!;
}