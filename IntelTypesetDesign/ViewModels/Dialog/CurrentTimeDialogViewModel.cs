using System;
using System.Reactive.Linq;

namespace IntelTypesetDesign.ViewModels.Dialog;

public class CurrentTimeDialogViewModel:ViewModelBase
{
    public DateTime CurrentTime => DateTime.Now;

    public CurrentTimeDialogViewModel() =>
        Observable.Timer(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1)).Subscribe((_) =>
        {
            this.OnPropertyChanged(nameof(CurrentTime));
        });
}