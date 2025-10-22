using AppStates;
using Infrastructure;
using UnityEngine;
using User;

public class AppLoader : MonoBehaviour
{
    private void Start()
    {
        LoadApp();
    }

    private void LoadApp()
    {
        Context.GetAllSystems()
               .ForEach(system => system.Initialize());
        
        Voodoo.Monetization.Initialize(Context.GetSystem<IUserSystem>().GetUserData().id,
                                                         () => Context.AppStateMachine.Enter<LobbyState>(), null);
    }
}
