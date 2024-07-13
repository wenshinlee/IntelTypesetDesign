using System;
using CommunityToolkit.Mvvm.Input;
using HanumanInstitute.MvvmDialogs;
using IntelTypesetDesign.Modules.Dialog;
using IntelTypesetDesign.ViewModels.Dialog;
using IntelTypesetDesign.Views.Dialog;

namespace IntelTypesetDesign.ViewModels.Editor;

public partial class ProjectEditorViewModel : ViewModelBase
{
    public string Test { get; set; } = "liwenxin";

    private IServiceProvider ServiceProvider { get; }
    
    private IDialogService DialogService { get; }
    
    public IRelayCommand TestButtonCommand { get; set; }

    public ProjectEditorViewModel(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        DialogService = ServiceProvider.GetService<IDialogService>();
        TestButtonCommand = new RelayCommand(TestButtonCommandFunc);
    }

    private void TestButtonCommandFunc()
    {
        Show(viewModel => DialogService.Show<CurrentTimeDialog>(this, viewModel));
    }
    
    private void Show(Action<CurrentTimeDialogViewModel> show)
    {
        var dialogViewModel = DialogService.CreateViewModel<CurrentTimeDialogViewModel>();
        show(dialogViewModel);
    }
}
