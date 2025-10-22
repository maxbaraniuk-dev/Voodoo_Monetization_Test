using System.Collections.Generic;
using UnityEngine;

namespace UI.Core
{
    [CreateAssetMenu(fileName = "UIConfig", menuName = "Scriptable Objects/UIConfig")]
    public class UIConfig : ScriptableObject
    {
        [SerializeField] private List<BaseView> views = new();
        [SerializeField] private UIContainer viewContainerPrefab;

        public T GetView<T>() where T : BaseView
        {
            var view = views.Find(v => v.GetType() == typeof(T));
            return view as T;
        }
        
        public UIContainer GetUIContainerPrefab() => viewContainerPrefab;
    }
}