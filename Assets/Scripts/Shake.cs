using UnityEngine;

public class Shake : MonoBehaviour
{
    public Animator camAnim;
    public static Shake instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ScreenShake()
    {
        if (camAnim != null)
        {
            camAnim.SetTrigger("shake");
        }
    }
}
