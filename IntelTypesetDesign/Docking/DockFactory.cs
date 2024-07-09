using Dock.Model.Mvvm;
using IntelTypesetDesign.ViewModels.Editor;

namespace IntelTypesetDesign.Docking;

public class DockFactory(ProjectEditorViewModel projectEditor) : Factory
{
    private readonly ProjectEditorViewModel _projectEditor = projectEditor;
}