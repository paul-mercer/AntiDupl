using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;
using AntiDuplWPF.ViewModel;

namespace AntiDuplWPF.Service
{
    public interface ILanguageService
    {
        event EventHandler LanguageChanged;

        CultureInfo Language { get; set; }
        ObservableCollection<LanguageViewModel> LanguageCollection { get; }
        ICommand LanguageSelectedCommand { get; }
    }
}
