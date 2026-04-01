using UnityEngine;

public class GravityPreview : MonoBehaviour
{
    public Transform previewObject;

    public void Show(Vector3 dir)
    {
        previewObject.gameObject.SetActive(true);
        previewObject.rotation = Quaternion.LookRotation(dir);
    }

    public void Hide()
    {
        previewObject.gameObject.SetActive(false);
    }
}