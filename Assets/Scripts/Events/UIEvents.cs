
namespace Events
{
    public abstract class UIEvents
    {
        public static readonly string OnPrepareNewGame = nameof(OnPrepareNewGame);
        public static readonly string OnShowResults = nameof(OnShowResults);
        public static readonly string OnStartNewGame = nameof(OnStartNewGame);
        public static readonly string OnCloseView = nameof(OnCloseView);
        public static readonly string OnBackToMenu = nameof(OnBackToMenu);
        
    }
}