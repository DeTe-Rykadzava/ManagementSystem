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
    private readonly IBasketRepository _basketRepository;
    private readonly ILogger<IBasketService> _logger;

    public BasketService(IBasketRepository basketRepository, ILogger<BasketService> logger)
    {
        _basketRepository = basketRepository;
        _logger = logger;
    }
    
    public async Task<ActionResultViewModel<BasketViewModel>> Get(int userId)
    {
        var result = new ActionResultViewModel<BasketViewModel>();
        try
        {
            var basketResult = await _basketRepository.Get(userId);
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
            var basketResult = await _basketRepository.CreateBasket(userId);
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

    public async Task<ActionResultViewModel<bool>> AddIntoBasket(int userId, int productId)
    {
        var result = new ActionResultViewModel<bool>();
        try
        {
            var model = new ManageProductIntoBasketModel
            {
                UserId = userId,
                ProductId = productId
            };
            var addResult = await _basketRepository.AddIntoBasket(model);
            if (!addResult)
            {
                result.Statuses.Add("Fail add");
                result.Statuses.Add("Fail save");
            }
            else
            {
                result.IsSuccess = true;
                result.Statuses.Add("Success add");
                result.Statuses.Add("Success save");
                result.Value = addResult;
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Exception with inset product to user basket.\nMessage: {Message}.\nInnerException: {InnerException}", e.Message, e.InnerException);
            result.Statuses.Add("Fail add");
            result.Statuses.Add("Fail save");
            result.Statuses.Add("Unknown problem");
        }
        return result;
    }

    public async Task<ActionResultViewModel<bool>> RemoveFromBasket(int userId, int productId)
    {
        var result = new ActionResultViewModel<bool>();
        try
        {
            var model = new ManageProductIntoBasketModel
            {
                UserId = userId,
                ProductId = productId
            };
            var removeResult = await _basketRepository.AddIntoBasket(model);
            if (!removeResult)
            {
                result.Statuses.Add("Fail remove");
                result.Statuses.Add("Fail save");
            }
            else
            {
                result.IsSuccess = true;
                result.Statuses.Add("Success remove");
                result.Statuses.Add("Success save");
                result.Value = removeResult;
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Exception with remove product from user basket.\nMessage: {Message}.\nInnerException: {InnerException}", e.Message, e.InnerException);
            result.Statuses.Add("Fail remove");
            result.Statuses.Add("Fail save");
            result.Statuses.Add("Unknown problem");
        }
        return result;
    }
}