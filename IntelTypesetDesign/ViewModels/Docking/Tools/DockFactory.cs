using Dock.Model.Controls;
using Dock.Model.Core;
using Dock.Model.Mvvm;
using IntelTypesetDesign.ViewModels.Editor;
using ReactiveUI;

namespace IntelTypesetDesign.ViewModels.Docking.Tools;

public class DockFactory( ProjectEditorViewModel projectEditor) : Factory
{
    private readonly ProjectEditorViewModel _projectEditor = projectEditor;
    
    /// <summary>
    /// Root Dock
    /// </summary>
    private IRootDock? _rootDock;
    
    public IRootDock? RootDock => _rootDock;
    
    public override IDocumentDock CreateDocumentDock()
    {
        
        return base.CreateDocumentDock();
    }

    public override IRootDock CreateLayout()
    {
        return base.CreateLayout();
    }

    public override void InitLayout(IDockable layout)
    {
        base.InitLayout(layout);
    }
}