using UnityEngine;

public class SlimeCustomizer : MonoBehaviour
{
    public Renderer targetRenderer;

    public void SetScheme(Color main, Color specular, Color rim, Color outline)
    {
        var m = targetRenderer.material;
        m.SetColor("_Color", main);
        m.SetColor("_SpecularColor", specular);
        m.SetColor("_RimColor", rim);
        m.SetColor("_OutlineColor", outline);

        PlayerPrefs.SetFloat("m_r", main.r);
        PlayerPrefs.SetFloat("m_g", main.g);
        PlayerPrefs.SetFloat("m_b", main.b);

        PlayerPrefs.SetFloat("s_r", specular.r);
        PlayerPrefs.SetFloat("s_g", specular.g);
        PlayerPrefs.SetFloat("s_b", specular.b);

        PlayerPrefs.SetFloat("r_r", rim.r);
        PlayerPrefs.SetFloat("r_g", rim.g);
        PlayerPrefs.SetFloat("r_b", rim.b);

        PlayerPrefs.SetFloat("o_r", outline.r);
        PlayerPrefs.SetFloat("o_g", outline.g);
        PlayerPrefs.SetFloat("o_b", outline.b);

        PlayerPrefs.Save();
    }
}
