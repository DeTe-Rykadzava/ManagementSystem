using System;
using System.Threading.Tasks;
using Database.Interfaces;
using Database.Models.Basket;
using ManagementSystem.Services.DatabaseServices.Interfaces;
using ManagementSystem.Services.UserStorage;
using ManagementSystem.ViewModels.Core;
using ManagementSystem.ViewModels.DataVM.Basket;
using Microsoft.Extensions.Logging;

namespace ManagementSystem.Services.DatabaseServices.Services;

public class BasketService : IBasketService
{
    private readonly IUserStorageService _userStorageService;
    private readonly IBasketRepository _basketRepository;
    private readonly ILogger<IBasketService> _logger;

    public BasketService(IUserStorageService userStorageService, IBasketRepository basketRepository, ILogger<BasketService> logger)
    {
        _userStorageService = userStorageService;
        _basketRepository = basketRepository;
        _logger = logger;
    }
    
    public async Task<ActionResultViewModel<BasketViewModel>> Get(int userId)
    {
        var result = new ActionResultViewModel<BasketViewModel>();
        try
        {
            if (_userStorageService.CurrentUser == null)
            {
                result.IsSuccess = false;
                result.Statuses.Add("Fail get basket");
                result.Statuses.Add("User is null");
                return result;
            }

            var basketResult = await _basketRepository.Get(_userStorageService.CurrentUser.Id);
            if (!basketResult.IsSuccess || basketResult.Value == null)
            {
                result.IsSuccess = false;
                result.Statuses.Add("Fail get basket");
                result.Statuses.Add("Basket not exist");
                return result;
            }

            result.IsSuccess = true;
            result.Value = new BasketViewModel(basketResult.Value);
            result.Statuses.Add("Success get");
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError("Exception with get basket by user id.\nMessage: {Message}.\nInnerException: {InnerException}", e.Message, e.InnerException);
        }
        return result;
    }

    public async Task<ActionResultViewModel<BasketViewModel>> CreateBasket(int userId)
    {
        var result = new ActionResultViewModel<BasketViewModel>();
        try
        {
            if (_userStorageService.CurrentUser == null)
            {
                result.IsSuccess = false;
                result.Statuses.Add("Fail create basket");
                result.Statuses.Add("User is null");
                return result;
            }

            var basketResult = await _basketRepository.CreateBasket(_userStorageService.CurrentUser.Id);
            if (!basketResult.IsSuccess || basketResult.Value == null)
            {
                result.IsSuccess = false;
                result.Statuses.Add("Fail create basket");
                return result;
            }

            result.IsSuccess = true;
            result.Value = new BasketViewModel(basketResult.Value);
            result.Statuses.Add("Success create");
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError("Exception with create basket by user id.\nMessage: {Message}.\nInnerException: {InnerException}", e.Message, e.InnerException);
        }
        return result;
    }

    public async Task<bool> AddIntoBasket(int productId)
    {
        try
        {
            if (_userStorageService.CurrentUser == null)
            {
                return false;
            }

            var model = new ManageProductIntoBasketModel
            {
                UserId = _userStorageService.CurrentUser.Id,
                ProductId = productId
            };
            var addResult = await _basketRepository.AddIntoBasket(model);
            return addResult;
        }
        catch (Exception e)
        {
            _logger.LogError("Exception with inset product to user basket.\nMessage: {Message}.\nInnerException: {InnerException}", e.Message, e.InnerException);
            return false;
        }
    }

    public async Task<bool> RemoveFromBasket(int productId)
    {
        try
        {
            if (_userStorageService.CurrentUser == null)
            {
                return false;
            }

            var model = new ManageProductIntoBasketModel
            {
                UserId = _userStorageService.CurrentUser.Id,
                ProductId = productId
            };
            var removeResult = await _basketRepository.AddIntoBasket(model);
            return removeResult;
        }
        catch (Exception e)
        {
            _logger.LogError("Exception with remove product from user basket.\nMessage: {Message}.\nInnerException: {InnerException}", e.Message, e.InnerException);
            return false;
        }
    }
}