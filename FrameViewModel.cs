using System;
using ReactiveUI;
using Weather.NET.Models.WeatherModel;

public class FrameViewModel : ReactiveObject
{
    string? photoPath;
    public string? PhotoPath 
    { 
        get => photoPath; 
        set => this.RaiseAndSetIfChanged(ref photoPath, value); 
    }

    DateTime time;
    public DateTime Time 
    { 
            get => time;
            set => this.RaiseAndSetIfChanged(ref time, value);  
    }

    WeatherModel? currentWeather;
    public WeatherModel? CurrentWeather
    {
        get => currentWeather;
        set => this.RaiseAndSetIfChanged(ref currentWeather, value);  
    }

    Bins.BinCollection? binCollection;
    public Bins.BinCollection? Bins
    {
        get => binCollection;
        set => this.RaiseAndSetIfChanged(ref binCollection, value);  
    }
}