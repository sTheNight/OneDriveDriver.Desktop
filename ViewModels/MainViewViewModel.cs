using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using OneDriveDriver.Desktop.Models;
using OneDriveDriver.Desktop.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace OneDriveDriver.Desktop.ViewModels;

public partial class MainViewViewModel : ViewModelBase {
    [ObservableProperty] private ObservableCollection<FileItem> _fileList;
    public MainViewViewModel() {
        FileList = new ObservableCollection<FileItem>();
        _ = GetFileListAsync();
    }

    public async Task GetFileListAsync() {
        try {
            using var httpClient = new HttpClient() {
                BaseAddress = new Uri(Global.ENDPOINT)
            };

            var response = await httpClient.GetAsync("/api/list");
            Console.WriteLine(response.RequestMessage.RequestUri.ToString());
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var fileList = JsonConvert.DeserializeObject<List<FileItem>>(content);
            FileList = new ObservableCollection<FileItem>(fileList);
        } catch (Exception ex) {
            Console.WriteLine(ex);
        }
    }

    [RelayCommand]
    public void ShowInfo(FileItem fileItem) {
        Console.WriteLine(111);
        Console.WriteLine(fileItem.Name);
    }
}