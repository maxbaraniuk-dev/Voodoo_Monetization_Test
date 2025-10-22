namespace UI.Core
{
    public class BaseDataView<T> : BaseView
    {
        public virtual void Show(T viewModel) { }
    }
}