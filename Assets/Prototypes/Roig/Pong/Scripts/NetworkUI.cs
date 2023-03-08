using Unity.Netcode;
using UnityEngine;

public class NetworkUI : MonoBehaviour
{
    public GameObject uiContainer;


    public void HostButton()
    {
        NetworkManager.Singleton.StartHost();
        DisableContainer();
    }

    public void ClientButton()
    {
        NetworkManager.Singleton.StartClient();
        DisableContainer();
    }

    private void DisableContainer()
    {
        uiContainer.SetActive(false);
    }
}
