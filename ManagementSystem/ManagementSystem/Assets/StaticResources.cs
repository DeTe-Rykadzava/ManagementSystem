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
    
    public static string NoImagePicture { get; } = "avares://ManagementSystem/Assets/no-image.png";

    public static int ClientRoleId { get; } = 3;

    public static string AdminRoleName { get; } = "admin";

}