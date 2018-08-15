using UnityEngine;

public class LevelData
{
    public Texture2D LevelMap { get; private set; }

    public int Width { get; private set; }
    public int Height { get; private set; }

    public LevelData(Texture2D levelMap)
    {
        LevelMap = levelMap;

        Width = LevelMap.width;
        Height = LevelMap.height;
    }   
}
