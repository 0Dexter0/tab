using Microsoft.AspNetCore.Components.WebView.Maui;

//using VijayAnand.MauiBlazor.Markup;

namespace WinUIApp1.Views;

public partial class MauiPage : ContentPage
{
    public MauiPage()
    {
        // ControlTemplate = (ControlTemplate)Application.Current!.Resources[nameof(VersionTemplate)];

        // For a much simplified initialization
        // Add reference to the VijayAnand.MauiBlazor.Markup NuGet package
        // dotnet add package VijayAnand.MauiBlazor.Markup
        // Then uncomment the using statement at the top of this file and the line below
        //var bwv = new BlazorWebView().Configure(typeof(Main), "/counter");

        BlazorWebView bwv = new()
        {
            StartPath = "/",
            HostPage = "wwwroot/index.html"
        };

        bwv.RootComponents.Add(new()
        {
            Selector = "#app",
            ComponentType = typeof(Main),
            Parameters = null
        });

        Content = bwv;
    }
}