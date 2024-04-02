using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ManagementSystem.ViewModels.DataVM.User;

namespace ManagementSystem.Assets;

public class StaticResources
{
    public static string AppName { get; } = "ManagementSystem";

    public static string AppIcon { get; } = "Assets/icon.png";
    
    public static string AppIconResourceLink { get; } = "avares://ManagementSystem/Assets/icon.png";

    public static int ClientRoleId { get; } = 1;

    public static string AdminRoleName { get; } = "admin";

}