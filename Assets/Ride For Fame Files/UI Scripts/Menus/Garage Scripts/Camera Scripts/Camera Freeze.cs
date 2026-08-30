using UnityEngine;

public class CameraFreeze : MonoBehaviour
{
    public float freezePosition;


    public void FreezePosition ()
    {
        transform.eulerAngles = new Vector3(0 , freezePosition , 0 );
    }
}
