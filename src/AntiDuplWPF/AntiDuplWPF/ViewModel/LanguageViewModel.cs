namespace AntiDuplWPF.ViewModel
{
    public enum LanguageEnum
    {
        English = 1,
        Russian = 2
    }

    public class LanguageViewModel : PropertyChangedBase
    {
        public LanguageViewModel()
        {
            IsChecked = false;
        }

        public LanguageEnum Enum { get; set; }
        private bool _isChecked = false;

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                RaisePropertyChangedEvent("IsChecked");
            }

        }

        public string Title { get; set; }
    }
}
