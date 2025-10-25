using UI.Core;
using UnityEngine;

namespace UI
{
    public interface IUISystem
    {
        public T ShowView<T>(Transform parent = null) where T : BaseView;
        public T ShowView<T, Tp>(Tp viewModel, Transform parent = null) where T : BaseDataView<Tp>;
        public T GetView<T>() where T : BaseView;
        public void CloseView<T>() where T : BaseView;
    }
}