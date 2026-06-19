using UnityEngine;

public class PaletteController : MonoBehaviour
{
    [Header("Matériaux de quantification")]
    public Material quantizeMaterial;

    [Header("Palette 16-32 Couleurs")] 
    public Color[] palette = new Color[]
    {

    };

    private Vector4[] m_PaletteVectors;
    
    private void Awake() => ApplyPalette();
    private void OnValidate() => ApplyPalette();

    private void ApplyPalette()
    {
        if (quantizeMaterial == null || palette == null) return;

        int count = Mathf.Min(palette.Length, 32);
        m_PaletteVectors = new Vector4[count];
        
        for (int i = 0; i < count; i++)
            m_PaletteVectors[i] = new Vector4(palette[i].r, palette[i].g, palette[i].b, 1f);
        
        quantizeMaterial.SetInt("_PaletteSize", count);
        quantizeMaterial.SetVectorArray("_Palette", m_PaletteVectors);
    }

    public void SwapPalette(Color[] newPalette)
    {
        palette = newPalette;
        ApplyPalette();
    }

}
