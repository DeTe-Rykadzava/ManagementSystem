using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using ManagementSystem.ViewModels.Core;

namespace ManagementSystem.Services.Storage;

public interface IStorageService
{
    public Task<ActionResultViewModel<IStorageFile>> OpenFileAsync(FilePickerOpenOptions options);
    public Task<ActionResultViewModel<IReadOnlyList<IStorageFile>>> OpenFilesAsync(FilePickerOpenOptions options);
    public Task<ActionResultViewModel<IStorageFile>> SaveFileAsync(FilePickerSaveOptions options);
    public Task<ActionResultViewModel<IStorageFolder>> OpenFolderAsync(FolderPickerOpenOptions options);
}