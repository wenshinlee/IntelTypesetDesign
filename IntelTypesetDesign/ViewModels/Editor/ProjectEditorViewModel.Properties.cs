using CommunityToolkit.Mvvm.ComponentModel;

namespace IntelTypesetDesign.ViewModels.Editor;

public partial class ProjectEditorViewModel : ViewModelBase
{
     [ObservableProperty]
     private object? _rootDock;

     [ObservableProperty]
     private object? _dockFactory;
}