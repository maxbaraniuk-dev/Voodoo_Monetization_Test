using Events;
using UnityEngine;

namespace UI.Core
{
    public abstract class BaseView : MonoBehaviour
    {
        [SerializeField] private ViewLayer layer;

        public ViewLayer Layer => layer;

        public virtual void Show() { }
        public virtual void UpdateView() { }
        
        public void Close()
        {
            CloseInternal();
            EventsMap.Dispatch(UIEvents.OnCloseView, this);
        }

        protected virtual void CloseInternal() { }
    }
}