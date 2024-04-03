using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using ManagementSystem.ViewModels.Core;
using Microsoft.Extensions.Logging;

namespace ManagementSystem.Services.Storage;

public class StorageService : IStorageService
{
    private readonly IStorageProvider _storageProvider;
    private readonly ILogger<IStorageService> _logger;
    public StorageService(IStorageProvider storageProvider, ILogger<StorageService> logger) => 
        (_storageProvider, _logger) = (storageProvider, logger);
    
    public async Task<ActionResultViewModel<IStorageFile>> OpenFileAsync(FilePickerOpenOptions options)
    {
        var result = new ActionResultViewModel<IStorageFile>();
        try
        {
            var openResult = await _storageProvider.OpenFilePickerAsync(options);
            if (!openResult.Any())
            {
                result.Statuses.Add("Fail get files");
                result.Statuses.Add("The user has not selected any file");
            }
            else
            {
                result.Value = openResult[0];
                result.IsSuccess = true;
                result.Statuses.Add($"Success get file {result.Value.Name}");
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Error with get file.\nException:\t{Exception}.\nInnerException:\t{InnerException}",
                e.Message, e.InnerException);
            result.Statuses.Add("Fail get file");
            result.Statuses.Add("Unknown problem");
        }
        return result;
    }

    public async Task<ActionResultViewModel<IReadOnlyList<IStorageFile>>> OpenFilesAsync(FilePickerOpenOptions options)
    {
        var result = new ActionResultViewModel<IReadOnlyList<IStorageFile>>();
        try
        {
            var openResult = await _storageProvider.OpenFilePickerAsync(options);
            if (!openResult.Any())
            {
                result.Statuses.Add("Fail get files");
                result.Statuses.Add("The user has not selected any files");
            }
            else
            {
                result.Value = openResult;
                result.IsSuccess = true;
                result.Statuses.Add("Success get files");
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Error with get files.\nException:\t{Exception}.\nInnerException:\t{InnerException}",
                e.Message, e.InnerException);
            result.Statuses.Add("Fail get files");
            result.Statuses.Add("Unknown problem");
        }
        return result;
    }
    
    public async Task<ActionResultViewModel<IStorageFile>> SaveFileAsync(FilePickerSaveOptions options)
    {
        var result = new ActionResultViewModel<IStorageFile>();
        try
        {
            var saveResult = await _storageProvider.SaveFilePickerAsync(options);
            if (saveResult == null)
            {
                result.Statuses.Add("Fail save file");
                result.Statuses.Add("The user did not save the file");
            }
            else
            {
                result.Value = saveResult;
                result.IsSuccess = true;
                result.Statuses.Add("Success save files");
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Error with save file.\nException:\t{Exception}.\nInnerException:\t{InnerException}",
                e.Message, e.InnerException);
            result.Statuses.Add("Fail save file");
            result.Statuses.Add("Unknown problem");
        }
        return result;
    }

    public async Task<ActionResultViewModel<IStorageFolder>> OpenFolderAsync(FolderPickerOpenOptions options)
    {
        var result = new ActionResultViewModel<IStorageFolder>();
        try
        {
            var openResult = await _storageProvider.OpenFolderPickerAsync(options);
            if (!openResult.Any())
            {
                result.Statuses.Add("Fail get folder");
                result.Statuses.Add("The user has not selected any folder");
            }
            else
            {
                result.Value = openResult[0];
                result.IsSuccess = true;
                result.Statuses.Add("Success get folder");
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Error with get file.\nException:\t{Exception}.\nInnerException:\t{InnerException}",
                e.Message, e.InnerException);
            result.Statuses.Add("Fail get file");
            result.Statuses.Add("Unknown problem");
        }
        return result;
    }
}