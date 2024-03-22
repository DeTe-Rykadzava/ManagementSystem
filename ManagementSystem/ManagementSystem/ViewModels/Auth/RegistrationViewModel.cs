﻿using System.Reactive;
using System.Threading.Tasks;
using ManagementSystem.Services;
using ManagementSystem.ViewModels.Core;

namespace ManagementSystem.ViewModels.Auth;

public class RegistrationViewModel : RoutableViewModelBase
{
    public override NavigationService RootNavManager { get; protected set; }
    
}