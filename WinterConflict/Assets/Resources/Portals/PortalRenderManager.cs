using UnityEngine;

public class PortalRenderManager : MonoBehaviour
{
    private Portal[] portals;

    private void Awake()
    {
        portals = FindObjectsByType<Portal>(FindObjectsSortMode.None);
    }
    private void OnPreCull()
    {
        for (int i = 0; i < portals.Length; i++)
        {
            portals[i].Render();
        }
        for (int i = 0; i < portals.Length; i++)
        {
            portals[i].PostRender();
        }
    }
}