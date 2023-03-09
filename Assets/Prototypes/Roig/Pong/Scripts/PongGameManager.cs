using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class PongGameManager : NetworkBehaviour
{
    public GameObject ball;
    public GameObject playerPrefab;

    private NetworkVariable<int> player1Score = new NetworkVariable<int>(0);
    private NetworkVariable<int> player2Score = new NetworkVariable<int>(0);

    private GameObject spawnedBall;

    public NetworkVariable<int> Player1Score { get => player1Score; }
    public NetworkVariable<int> Player2Score { get => player2Score; }

    private void SpawnPlayer(float xPosition, ulong id)
    {
        GameObject go = Instantiate(playerPrefab, Vector3.right * xPosition, playerPrefab.transform.rotation);
        go.GetComponent<NetworkObject>().SpawnWithOwnership(id);
    }

    private void SpawnBall()
    {
        spawnedBall = Instantiate(ball, Vector3.zero, ball.transform.rotation);
        spawnedBall.GetComponent<NetworkObject>().Spawn();
    }


    void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnect;
    }

    private void OnClientConnect(ulong id)
    {
        if (NetworkManager.Singleton.ConnectedClients.Count == 1)
        {
            SpawnPlayer(-8f, id);
        }
        else
        {
            SpawnPlayer(8f, id);
            SpawnBall();
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void OnGoalServerRpc(bool isPlayer1Goal)
    {
        if (isPlayer1Goal)
        {
            player1Score.Value++;
        }
        else
        {
            player2Score.Value++;
        }
        ResetBall();
    }

    private void ResetBall()
    {
        spawnedBall.GetComponent<NetworkObject>().Despawn();
        SpawnBall();
    }
}
