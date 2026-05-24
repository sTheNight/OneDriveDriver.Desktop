using System.Threading.Tasks;

namespace OneDriveDriver.Desktop.Services;

public interface IUrlLauncher {
    Task<bool> LaunchAsync(string url);
}
