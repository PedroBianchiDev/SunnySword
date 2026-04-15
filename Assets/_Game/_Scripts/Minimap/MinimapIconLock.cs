using UnityEngine;

public class MinimapIconLock : MonoBehaviour
{
    void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}