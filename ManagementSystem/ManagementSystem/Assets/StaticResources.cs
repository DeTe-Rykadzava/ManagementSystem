using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using ManagementSystem.ViewModels.DataVM.User;

namespace ManagementSystem.Assets;

public class StaticResources
{
    // app data
    public static string AppName { get; } = "ManagementSystem";

    // icons
    public static string AppIconResourceLink { get; } = "avares://ManagementSystem/Assets/icon.png";
    
    public static Bitmap? AppIconImage
    {
        get
        {
            var exist = AssetLoader.Exists(new Uri(AppIconResourceLink));
            if (!exist)
                return null;
            var imageStream = new MemoryStream();
            AssetLoader.Open(new Uri(AppIconResourceLink)).CopyTo(imageStream);
            imageStream.Position = 0;
            return new Bitmap(imageStream);
        }
    }
    
    public static string AppIconWhiteResourceLink { get; } = "avares://ManagementSystem/Assets/icon-white.png";
    
    public static Bitmap? AppIconWhiteImage
    {
        get
        {
            var exist = AssetLoader.Exists(new Uri(AppIconWhiteResourceLink));
            if (!exist)
                return null;
            var imageStream = new MemoryStream();
            AssetLoader.Open(new Uri(AppIconWhiteResourceLink)).CopyTo(imageStream);
            imageStream.Position = 0;
            return new Bitmap(imageStream);
        }
    }
    
    public static string NoImagePictureResourceLink { get; } = "avares://ManagementSystem/Assets/no-image.png";

    public static Bitmap? NoImagePictureImage
    {
        get
        {
            var exist = AssetLoader.Exists(new Uri(NoImagePictureResourceLink));
            if (!exist)
                return null;
            var imageStream = new MemoryStream();
            AssetLoader.Open(new Uri(NoImagePictureResourceLink)).CopyTo(imageStream);
            imageStream.Position = 0;
            return new Bitmap(imageStream);
        }
    }

    public static string BasketAddIconResourceLink { get; } = "avares://ManagementSystem/Assets/basket.png";
    public static Bitmap? BasketAddIconImage
    {
        get
        {
            var exist = AssetLoader.Exists(new Uri(BasketAddIconResourceLink));
            if (!exist)
                return null;
            var imageStream = new MemoryStream();
            AssetLoader.Open(new Uri(BasketAddIconResourceLink)).CopyTo(imageStream);
            imageStream.Position = 0;
            return new Bitmap(imageStream);
        }
    }
    
    public static string BasketRemoveIconResourceLink { get; } = "avares://ManagementSystem/Assets/basket-remove.png";
    public static Bitmap? BasketRemoveIconImage
    {
        get
        {
            var exist = AssetLoader.Exists(new Uri(BasketRemoveIconResourceLink));
            if (!exist)
                return null;
            var imageStream = new MemoryStream();
            AssetLoader.Open(new Uri(BasketRemoveIconResourceLink)).CopyTo(imageStream);
            imageStream.Position = 0;
            return new Bitmap(imageStream);
        }
    }
    
    public static string EditIconResourceLink { get; } = "avares://ManagementSystem/Assets/edit.png";
    public static Bitmap? EditIconImage
    {
        get
        {
            var exist = AssetLoader.Exists(new Uri(EditIconResourceLink));
            if (!exist)
                return null;
            var imageStream = new MemoryStream();
            AssetLoader.Open(new Uri(EditIconResourceLink)).CopyTo(imageStream);
            imageStream.Position = 0;
            return new Bitmap(imageStream);
        }
    }
    
    public static string AppendIconResourceLink { get; } = "avares://ManagementSystem/Assets/add.png";
    public static Bitmap? AppendIconImage
    {
        get
        {
            var exist = AssetLoader.Exists(new Uri(AppendIconResourceLink));
            if (!exist)
                return null;
            var imageStream = new MemoryStream();
            AssetLoader.Open(new Uri(AppendIconResourceLink)).CopyTo(imageStream);
            imageStream.Position = 0;
            return new Bitmap(imageStream);
        }
    }
    
    public static string MinusIconResourceLink { get; } = "avares://ManagementSystem/Assets/minus.png";
    public static Bitmap? MinusIconImage
    {
        get
        {
            var exist = AssetLoader.Exists(new Uri(MinusIconResourceLink));
            if (!exist)
                return null;
            var imageStream = new MemoryStream();
            AssetLoader.Open(new Uri(MinusIconResourceLink)).CopyTo(imageStream);
            imageStream.Position = 0;
            return new Bitmap(imageStream);
        }
    }
    
    public static string ShoppingBasketAddIconResourceLink { get; } = "avares://ManagementSystem/Assets/shopping-basket.png";
    public static Bitmap? ShoppingBasketAddIconImage
    {
        get
        {
            var exist = AssetLoader.Exists(new Uri(ShoppingBasketAddIconResourceLink));
            if (!exist)
                return null;
            var imageStream = new MemoryStream();
            AssetLoader.Open(new Uri(ShoppingBasketAddIconResourceLink)).CopyTo(imageStream);
            imageStream.Position = 0;
            return new Bitmap(imageStream);
        }
    }
    
    public static string ShoppingBasketRemoveIconResourceLink { get; } = "avares://ManagementSystem/Assets/shopping-basket-remove.png";
    public static Bitmap? ShoppingBasketRemoveIconImage
    {
        get
        {
            var exist = AssetLoader.Exists(new Uri(ShoppingBasketRemoveIconResourceLink));
            if (!exist)
                return null;
            var imageStream = new MemoryStream();
            AssetLoader.Open(new Uri(ShoppingBasketRemoveIconResourceLink)).CopyTo(imageStream);
            imageStream.Position = 0;
            return new Bitmap(imageStream);
        }
    }
    
    public static string TrashIconResourceLink { get; } = "avares://ManagementSystem/Assets/trash.png";
    public static Bitmap? TrashIconImage
    {
        get
        {
            var exist = AssetLoader.Exists(new Uri(TrashIconResourceLink));
            if (!exist)
                return null;
            var imageStream = new MemoryStream();
            AssetLoader.Open(new Uri(TrashIconResourceLink)).CopyTo(imageStream);
            imageStream.Position = 0;
            return new Bitmap(imageStream);
        }
    }
    
    // other

    public static double ImageWidth => 250D;
    public static double ImageHeight => 250D;
    public static double ImageWidthAlt => 125D;
    public static double ImageHeightAlt => 125D;

    public static int ClientRoleId { get; } = 3;

    public static string AdminRoleName { get; } = "admin";
    
    public static string StorekeeperRoleName { get; } = "storekeeper";

}