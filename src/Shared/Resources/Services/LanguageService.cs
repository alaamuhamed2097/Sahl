namespace Resources.Services
{
    public class LanguageService
    {
        public event Action OnLanguageChanged;

        public void NotifyLanguageChanged()
        {
            OnLanguageChanged?.Invoke();
        }
    }
}
