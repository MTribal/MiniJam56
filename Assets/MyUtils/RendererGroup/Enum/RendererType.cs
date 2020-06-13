namespace My_Utils
{
    /// <summary>
    /// Type that will renderer. Smart will choose the most adequate.
    /// </summary>

    public enum RendererType
    {
        SpriteRenderer = 1,
        ColorGroup = 2,
        AlphaGroup = 4,
        Tilemap = 8,
        Image = 16,
        RawImage = 32,
        Text = 64,
        TmProText = 128,
        CanvasGroup = 256
    }
}
