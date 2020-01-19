using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManagement : MonoBehaviour
{

    public static List<PlayerData> playerDataPerevice = new List<PlayerData>();

    void OnPlayerJoined(PlayerInput pi)
    {
        pi.camera.name = System.Guid.NewGuid().ToString();
        foreach (var device in pi.devices)
        {
            Debug.Log($"Player joined {device.deviceId} , {pi.camera.name}");
            playerDataPerevice.Add(new PlayerData()
            {
                deviceId = device.deviceId,
                playerCamera = pi.camera
            });
        }
    }
    
    void OnPlayerLeft(
        PlayerInput pi)
    {
            foreach (var device in pi.devices)
        {
            playerDataPerevice.RemoveAll(pd => pd.deviceId == device.deviceId);
        }
    }

    public struct PlayerData
    {
        public int deviceId;
        public Camera playerCamera;
    }
}
