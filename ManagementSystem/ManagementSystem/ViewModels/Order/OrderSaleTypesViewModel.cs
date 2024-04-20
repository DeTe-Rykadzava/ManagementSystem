using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Threading;
using ManagementSystem.Services.DatabaseServices.Interfaces;
using ManagementSystem.Services.DialogService;
using ManagementSystem.Services.NavigationService;
using ManagementSystem.ViewModels.Core;
using ManagementSystem.ViewModels.DataVM.Order;
using MsBox.Avalonia.Enums;
using ReactiveUI;

namespace ManagementSystem.ViewModels.Order;

public class OrderSaleTypesViewModel : RoutableViewModelBase
{
    public override string ViewModelViewPath { get; } = "sale-types";
    public override INavigationService RootNavManager { get; protected set; } = null!;
    
    // services
    private readonly IOrderSaleTypeService _orderSaleTypeService;
    private readonly IDialogService _dialogService;

    // fields
    public ObservableCollection<OrderSaleTypeViewModel> Types { get; } = new ();

    private bool _typesIsEmpty = true;
    public bool TypesIsEmpty
    {
        get => _typesIsEmpty;
        private set => this.RaiseAndSetIfChanged(ref _typesIsEmpty, value);
    }

    private string _newTypeName = string.Empty;
    [Required(ErrorMessage = "Type name is required")]
    public string NewTypeName
    {
        get => _newTypeName;
        set => this.RaiseAndSetIfChanged(ref _newTypeName, value);
    }

    // commands
    public ICommand AddTypeCommand { get; }
    public ReactiveCommand<OrderSaleTypeViewModel, Unit> DeleteTypeCommand { get; }

    public OrderSaleTypesViewModel(IOrderSaleTypeService orderSaleTypeService, IDialogService dialogService)
    {
        _orderSaleTypeService = orderSaleTypeService;
        _dialogService = dialogService;
        var canAddNewType = this
            .WhenAnyValue(x => x.NewTypeName, (typeName) => !string.IsNullOrWhiteSpace(typeName))
            .DistinctUntilChanged();
        AddTypeCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var result = await _orderSaleTypeService.AddTypeAsync(NewTypeName);
            if (!result.IsSuccess || result.Value == null)
            {
                await dialogService.ShowPopupDialogAsync("Error", $"Sorry but type was not added. Purpose:\n\t{string.Join("\n\t", result.Statuses)}", icon: Icon.Error);
            }
            else
            {
                await dialogService.ShowPopupDialogAsync("Success", $"Success add type", icon: Icon.Success);
                Types.Add(result.Value);
            }
            TypesIsEmpty = !Types.Any();
        },
        canAddNewType);
        DeleteTypeCommand = ReactiveCommand.CreateFromTask(async (OrderSaleTypeViewModel type) =>
        {
            var dialogResult =
                await _dialogService.ShowPopupDialogAsync("Question", "Are you sure you want to delete a type? In this case, the related orders will also be deleted, make sure that the data is saved.", ButtonEnum.YesNo, Icon.Question);
            if(dialogResult == ButtonResult.No)
                return;
            var deleteResult = await _orderSaleTypeService.RemoveTypeAsync(type.Id);
            if (!deleteResult.IsSuccess || deleteResult.Value == false)
            {
                await dialogService.ShowPopupDialogAsync("Error", $"Sorry but type was not deleted. Purpose:\n\t{string.Join("\n\t", deleteResult.Statuses)}");
                return;
            }
            Types.Remove(type);
            TypesIsEmpty = !Types.Any();
        });
    }
    
    public override async Task OnShowed()
    {
        Task.Run(new Action(async () =>
        {
            Dispatcher.UIThread.Invoke(new Action(() =>
            {
                Types.Clear();
            }));
            var categoriesResult = await _orderSaleTypeService.GetAllAsync();
            if (!categoriesResult.IsSuccess || categoriesResult.Value == null || !categoriesResult.Value.Any())
            {
                TypesIsEmpty = true;
                return;
            }
            foreach (var category in categoriesResult.Value)
            {
                Dispatcher.UIThread.Invoke(new Action(() =>
                {
                    Types.Add(category);
                }));
            }
            TypesIsEmpty = !Types.Any();
        }));
    }
}