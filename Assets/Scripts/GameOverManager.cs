using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameOverManager : NetworkBehaviour
{
    public static GameOverManager Instance;

    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;

    private HashSet<ulong> deadPlayers = new HashSet<ulong>();

    private void Awake()
    {
        Instance = this;
    }

    public void PlayerDied(ulong clientId)
    {
        deadPlayers.Add(clientId);

        if (IsServer)
        {
            // สมมติเล่น 2 คน → อีกคนตาย แปลว่าเราชนะ
            if (NetworkManager.ConnectedClients.Count - deadPlayers.Count == 1)
            {
                foreach (var client in NetworkManager.ConnectedClientsList)
                {
                    bool win = !deadPlayers.Contains(client.ClientId);

                    var rpcParams = new ClientRpcParams
                    {
                        Send = new ClientRpcSendParams
                        {
                            TargetClientIds = new[] { client.ClientId }
                        }
                    };

                    ShowGameOverClientRpc(win, rpcParams);
                }
            }
        }
    }

    [ClientRpc]
    private void ShowGameOverClientRpc(bool isWin, ClientRpcParams rpcParams = default)
    {
        if (isWin)
            winPanel.SetActive(true);
        else
        {
            losePanel.SetActive(true);
           // Debug.Log("🟥 YOU LOSE panel activated");
        }
    }

}
