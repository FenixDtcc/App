﻿using Android.App;
using Android.Content.PM;
using Android.OS;

namespace QuantoDemoraApp.Platforms.Android;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        Xamarin.Essentials.Platform.Init(this, savedInstanceState);
        base.OnCreate(savedInstanceState);
    }
}
