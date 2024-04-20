using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Threading;
using ManagementSystem.Services.DatabaseServices.Interfaces;
using ManagementSystem.Services.DialogService;
using ManagementSystem.Services.NavigationService;
using ManagementSystem.ViewModels.Core;
using ManagementSystem.ViewModels.DataVM.Warehouse;
using ManagementSystem.ViewModels.Warehouse.Factories;
using MsBox.Avalonia.Enums;
using ReactiveUI;

namespace ManagementSystem.ViewModels.Warehouse;

public class WarehousesViewModel : RoutableViewModelBase
{
    public override string ViewModelViewPath { get; } = "warehouses";
    public override INavigationService RootNavManager { get; protected set; } = null!;
    
    // services
    private readonly IWarehouseService _warehouseService;
    private readonly IEditWarehouseFactory _editWarehouseFactory;
    private readonly IDialogService _dialogService;
    
    // fields
    public ObservableCollection<WarehouseViewModel> Warehouses { get; } = new ();

    private bool _warehousesIsEmpty = true;
    public bool WarehousesIsEmpty
    {
        get => _warehousesIsEmpty;
        private set => this.RaiseAndSetIfChanged(ref _warehousesIsEmpty, value);
    }

    private string _newWarehouseName = string.Empty;
    [Required(ErrorMessage = "Warehouse name is required")]
    public string NewWarehouseName
    {
        get => _newWarehouseName;
        set => this.RaiseAndSetIfChanged(ref _newWarehouseName, value);
    }

    // commands
    public ICommand AddWarehouseCommand { get; }
    public ReactiveCommand<WarehouseViewModel, Unit> RemoveWarehouseCommand { get; }
    public ReactiveCommand<WarehouseViewModel, Unit> EditWarehouseCommand { get; }


    public WarehousesViewModel(IWarehouseService warehouseService, IEditWarehouseFactory editWarehouseFactory, IDialogService dialogService)
    {
        _warehouseService = warehouseService;
        _editWarehouseFactory = editWarehouseFactory;
        _dialogService = dialogService;

        AddWarehouseCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            if (string.IsNullOrWhiteSpace(NewWarehouseName))
            {
                await _dialogService.ShowPopupDialogAsync("Stop", "You did not specify the name of the warehouse", icon: Icon.Stop);
                return;
            }

            var addResult = await _warehouseService.AddWarehouseAsync(NewWarehouseName);
            if (!addResult.IsSuccess || addResult.Value == null)
            {
                await _dialogService.ShowPopupDialogAsync("Error", $"Failed to add a warehouse.Purposes:\n\t *{string.Join("\n\t *", addResult.Statuses)}", icon: Icon.Error);
                return;
            }

            Warehouses.Add(addResult.Value);
            WarehousesIsEmpty = !Warehouses.Any();
        });

        RemoveWarehouseCommand = ReactiveCommand.CreateFromTask(async (WarehouseViewModel warehouse) =>
        {
            var dialogResult =
                await _dialogService.ShowPopupDialogAsync("Question", "Are you sure you want to delete a warehouse?", ButtonEnum.YesNo, Icon.Question);
            if(dialogResult == ButtonResult.No)
                return;
            var removeResult = await _warehouseService.DeleteWarehouseAsync(warehouse.Id);
            if(!removeResult.IsSuccess || !removeResult.Value)
                await _dialogService.ShowPopupDialogAsync("Error", $"The warehouse has not been deleted.Purposes:\n\t *{string.Join("\n\t *", removeResult.Statuses)}",icon: Icon.Error);
            else
            {
                await _dialogService.ShowPopupDialogAsync("Success", "Success", icon: Icon.Success);
                Warehouses.Remove(warehouse);
            }
            
            WarehousesIsEmpty = !Warehouses.Any();
        });

        EditWarehouseCommand = ReactiveCommand.CreateFromTask(async (WarehouseViewModel warehouse) =>
        {
            var editVm = _editWarehouseFactory.Create(warehouse);
            await RootNavManager.NavigateTo(editVm);
        });
    }
    
    public override async Task OnShowed()
    {
        Task.Run(LoadWarehouses);
    }

    private async Task LoadWarehouses()
    {
        Dispatcher.UIThread.Invoke(new Action(() =>
        {
            Warehouses.Clear();
        }));
        var warehousesResult = await _warehouseService.GetWarehousesAsync();
        if(!warehousesResult.IsSuccess || warehousesResult.Value == null)
            return;
        foreach (var product in warehousesResult.Value)
        {
            Dispatcher.UIThread.Invoke(new Action(() =>
            {
                Warehouses.Add(product);
            }));
        }
        WarehousesIsEmpty = !Warehouses.Any();
    }
    
}