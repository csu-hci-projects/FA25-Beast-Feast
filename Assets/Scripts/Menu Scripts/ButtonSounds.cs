using UnityEngine;
using UnityEngine.EventSystems;

public class MenuSFX : MonoBehaviour
{
    public static MenuSFX i;
    public AudioSource sfx;
    public AudioClip hover;
    public AudioClip click;

    void Awake() => i = this;

    public void Hover() => sfx.PlayOneShot(hover);
    public void Click() => sfx.PlayOneShot(click);
}
