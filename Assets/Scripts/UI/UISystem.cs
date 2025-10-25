using System.Collections.Generic;
using System.Linq;
using Events;
using Infrastructure;
using Logs;
using UI.Core;
using UnityEngine;
using Zenject;

namespace UI
{
    public class UISystem : IUISystem, ISystem
    {
        [Inject] private ILog _log;
        [Inject] private UIConfig _uiConfig;
        
        private UIContainer _uiContainer;
        readonly List<BaseView> _openedViews = new();
        
        public void Initialize()
        {
            _log.Debug(() => "UIManager initialized");
            _uiContainer = Object.Instantiate(_uiConfig.GetUIContainerPrefab());
            EventsMap.Subscribe<BaseView>(UIEvents.OnCloseView, OnViewClosedEventListener);
        }

        public void Dispose() { }
        
        public T ShowView<T>(Transform parent = null) where T : BaseView
        {
            var view = GetView<T>(parent);
            view.Show();
            return view;
        }
        
        public T ShowView<T, Tp>(Tp viewModel, Transform parent = null) where T : BaseDataView<Tp>
        {
            var view = GetView<T>(parent);
            view.Show(viewModel);
            return view;
        }

        public T GetView<T>() where T : BaseView
        {
            return _openedViews.FirstOrDefault(view => view is T) as T;
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
             Object.Destroy(view.gameObject);
        }
        
        private T GetView<T>(Transform parent = null) where T : BaseView
        {
            var openedView = _openedViews.FirstOrDefault(view => view is T);
            if (openedView != null) 
                return openedView as T;
            
            var widgetPrefab = _uiConfig.GetView<T>();
            if (widgetPrefab == null)
            {
                _log.Error(() => $"There no prefab with type {typeof(T)}");
                return null;
            }

            var root = _uiContainer.GetLayerRoot(widgetPrefab.Layer);
            var view = Object.Instantiate(widgetPrefab, parent == null ? root : parent);
            _openedViews.Add(view);
            return view;
        }
    }
}