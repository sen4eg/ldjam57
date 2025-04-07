using UnityEngine;

[System.Serializable]
public class TubeLevelInfo
{
    public int levelIndex;
    public Material[] insideTextures;
    public float levelHeight;
    public Color tubeColor;
    public Material textureLessMaterial;  // Material without texture
}

public class TubeScript : MonoBehaviour
{
    [SerializeField] private TubeLevelInfo[] levelInfoArray;
    [SerializeField] private MeshFilter meshFilter;
    private float tubeHeight;
    private Mesh tubeMesh;

    void Start()
    {
        if (meshFilter != null)
        {
            tubeMesh = meshFilter.mesh;
            tubeHeight = tubeMesh.bounds.size.y;
        }

        BuildTube();
    }

    void Update()
    {
    }

    private void BuildTube()
    {
        if (levelInfoArray != null && levelInfoArray.Length > 0)
        {
            for (int i = 0; i < levelInfoArray.Length; i++)
            {
                var levelInfo = levelInfoArray[i];
                ApplyLevelInfo(levelInfo, i);
            }
        }
    }

    private void ApplyLevelInfo(TubeLevelInfo levelInfo, int index)
    {
        if (levelInfo.insideTextures != null && levelInfo.insideTextures.Length > 0 && meshFilter != null)
        {
            var meshRenderer = meshFilter.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.material = levelInfo.insideTextures[0];  // Apply first texture for simplicity
            }
        }

        if (levelInfo.tubeColor != null)
        {
            var meshRenderer = meshFilter.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.material.color = levelInfo.tubeColor;
            }
        }

        if (levelInfo.textureLessMaterial != null)
        {
            var meshRenderer = meshFilter.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.material = levelInfo.textureLessMaterial;  // Use the texture-less material
            }
        }
    }
}
