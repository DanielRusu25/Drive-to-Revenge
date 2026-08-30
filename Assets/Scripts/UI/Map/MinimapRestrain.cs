using UnityEngine;

public class MinimapRestrain : MonoBehaviour
{
    public Transform Player;       // The player's transform
    public Transform Arrow;        // The arrow icon on the minimap
    public float Height = 200f;    

    private void LateUpdate()
    {
        // Keep the camera above the player at a fixed height
        transform.position = new Vector3(Player.position.x, Height, Player.position.z);
        transform.rotation = Quaternion.Euler(90f, Player.eulerAngles.y, 0f); // Face straight down (top-down view)

        // Keep the arrow at a fixed height and match player's Y rotation
        if (Arrow != null)
        {
            Arrow.position = new Vector3(Player.position.x, Height - 50f, Player.position.z); // Slightly under the camera
            Arrow.rotation = Quaternion.Euler(90f, Player.eulerAngles.y, 0f); // Rotate to match player's Y
        }
    }
}
