﻿using App.Core.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;

namespace App.ViewModels;

public class MainViewModel : ObservableRecipient
{
    public MainViewModel(
        IMockDataService dataService
        )
    {

    }
}
