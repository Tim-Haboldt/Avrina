using UnityEngine;
using Mirror;

public class DestroyActiveNetworkManager : MonoBehaviour
{
    /// <summary>
    ///  Destorys the current active network manager (Could be server or client)
    /// </summary>
    public void Activate()
    {
        Destroy(NetworkManager.singleton.gameObject);
    }
}
