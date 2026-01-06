using UnityEngine;

[System.Serializable]
public class PlayerData
{
    // We save position as 3 floats because Unity vectors can be tricky in JSON
    public float[] position;
    public float[] rotation;

    // Constructor: This runs when we create the save data
    public PlayerData(MyCharacterController player)
    {
        // 1. Save Position
        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;

        // 2. Save Rotation (so you face the same way)
        rotation = new float[4];
        rotation[0] = player.transform.rotation.x;
        rotation[1] = player.transform.rotation.y;
        rotation[2] = player.transform.rotation.z;
        rotation[3] = player.transform.rotation.w;
    }
}