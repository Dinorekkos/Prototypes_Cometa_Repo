using Unity.Netcode;
using UnityEngine;

public class PongGameManager : NetworkManager
{
    public GameObject ball;
    public GameObject playerPrefab;

    private void SpawnPlayer(float xPosition, ulong id)
    {
        GameObject go = Instantiate(playerPrefab, Vector3.right * xPosition, playerPrefab.transform.rotation);
        go.GetComponent<NetworkObject>().SpawnWithOwnership(id);
    }

    private void SpawnBall()
    {
        GameObject go = Instantiate(ball, Vector3.zero, ball.transform.rotation);
        go.GetComponent<NetworkObject>().Spawn();
    }


    void Start()
    {
        OnClientConnectedCallback += OnClientConnect;
    }

    private void OnClientConnect(ulong id)
    {
        if (ConnectedClients.Count == 1)
        {
            SpawnPlayer(-8f, id);
        }
        else
        {
            SpawnPlayer(8f, id);
            SpawnBall();
        }
    }
}
