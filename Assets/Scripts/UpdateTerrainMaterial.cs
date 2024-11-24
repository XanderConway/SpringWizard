using UnityEngine;

public class UpdateTerrainMaterial : MonoBehaviour
{
    public Material terrainMaterial; // Assign the material using your Shader Graph
    private Terrain terrain; // Assign your Terrain component

    void Start()
    {
        terrain = GetComponent<Terrain>();
        // Access the first splatmap (terrain control texture)
        TerrainData terrainData = terrain.terrainData;
        Texture terrainHoles = terrainData.holesTexture;

        // Pass the splatmap to the material
        //terrainMaterial.SetTexture("_TerrainHoles", tempTexture);
        terrainMaterial.SetTexture("_TerrainHoles", terrainHoles);
    }
}
