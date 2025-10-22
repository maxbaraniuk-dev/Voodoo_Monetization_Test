using UnityEngine;

namespace UI.Core
{
    public class UIContainer : MonoBehaviour
    {
        [SerializeField] private Transform persistenceUIRoot;
        [SerializeField] private Transform dialogsRoot;
        [SerializeField] private Transform notificationsRoot;

        public Transform GetLayerRoot(ViewLayer layer)
        {
            return layer switch
            {
                ViewLayer.PersistenceUI => persistenceUIRoot,
                ViewLayer.Dialogs => dialogsRoot,
                ViewLayer.Notifications => notificationsRoot,
                _ => null
            };
        }
    }
}