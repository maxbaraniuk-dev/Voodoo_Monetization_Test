using System.Collections.Generic;
using System.Linq;
using Events;
using Infrastructure;
using Logs;
using UI.Core;
using UnityEngine;

namespace UI
{
    public class UISystem : MonoBehaviour, IUISystem
    {
        
        [SerializeField] private UIConfig uiConfig;
        
        private ILog _log;
        private UIContainer _uiContainer;
        readonly List<BaseView> _openedViews = new();
        
        public void Initialize()
        {
            _log = Context.GetSystem<ILog>();
            _log.Debug(() => "UIManager initialized");
            _uiContainer = Instantiate(uiConfig.GetUIContainerPrefab());
            EventsMap.Subscribe<BaseView>(UIEvents.OnCloseView, OnViewClosedEventListener);
        }

        public void Dispose() { }
        
        public T ShowView<T>(Transform parent = null) where T : BaseView
        {
            var view = CreateView<T>(parent);
            view.Show();
            return view;
        }
        
        public T ShowView<T, Tp>(Tp viewModel, Transform parent = null) where T : BaseDataView<Tp>
        {
            var view = CreateView<T>(parent);
            view.Show(viewModel);
            return view;
        }

        public void CloseView<T>() where T : BaseView
        {
            var view = _openedViews.FirstOrDefault(view => view is T);
            if (view == null)
            {
                _log.Warning(()=> $"There is no opened view with type {typeof(T)}");
                return;
            }
            
            view.Close();
        }

        private void OnViewClosedEventListener(BaseView view)
        {
             _openedViews.Remove(view);
             Destroy(view.gameObject);
        }
        
        private T CreateView<T>(Transform parent = null) where T : BaseView
        {
            var openedView = _openedViews.FirstOrDefault(view => view is T);
            if (openedView != null) 
                return openedView as T;
            
            var widgetPrefab = uiConfig.GetView<T>();
            if (widgetPrefab == null)
            {
                _log.Error(() => $"There no prefab with type {typeof(T)}");
                return null;
            }

            var root = _uiContainer.GetLayerRoot(widgetPrefab.Layer);
            var view = Instantiate(widgetPrefab, parent == null ? root : parent);
            _openedViews.Add(view);
            return view;
        }
    }
}