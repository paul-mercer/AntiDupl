using AntiDuplWPF.ViewModel;

namespace AntiDuplWPF.View
{
    public interface IWindowService
    {
        ProgressDialogViewModel OpenProgressWindow(ProgressDialogViewModel vm);
        void ShowDialogWindow<T>(object dataContext) where T : System.Windows.Window, new();
        void CloseProgressWindow();
    }
}
