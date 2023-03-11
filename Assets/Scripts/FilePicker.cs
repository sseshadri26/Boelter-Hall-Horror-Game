using UnityEngine;
using System.IO;
using System.Linq;
using System.Collections.Generic;


//[ExecuteInEditMode]

public class FilePicker : MonoBehaviour
{
    public string folderPath;
    private Texture2D[] textures;

    public float quadWidth = 1.0f; // default width is 1 unit
    public float quadHeight;

    

    [SerializeField] public float quadArea = 20;

    float oldArea;

    private Vector3 oldLocation;


    [SerializeField] private List<Material> materials = new List<Material>();
    
    private Renderer rendererComponent;

    private void Start()
    {
        //string[] filepaths = Directory.GetFiles(folderPath);
        //textures = filepaths
        //    .Where(filepath => filepath.EndsWith(".mat")
        //    .Select(filepath => LoadTextureFromFile(filepath))
            //.ToArray();
        //oldWidth = quadWidth;
        //oldLocation = gameObject.transform.position;
        //LoadMaterials(folderPath);
        AssignRandomMaterial();

        


    }


    private void LoadMaterials(string folderPath)
    {
        var materialsFolder = Directory.GetFiles(folderPath);
        if (materialsFolder == null)
        {
            Debug.LogError($"Folder {folderPath} not found in Resources folder.");
            return;
        }

        var materialsInFolder = Resources.LoadAll(folderPath, typeof(Material));
        foreach (var material in materialsInFolder)
        {
            materials.Add((Material)material);
        }
    }

    private void AssignRandomMaterial()
    {
        //if (materials.Count == 0)
        //{
        //    Debug.LogError($"No materials found in {folderPath} folder.");
        //    return;
        //}
        Random.InitState((int)((gameObject.transform.position.x + 2.417289f + gameObject.transform.position.z) * 10));
        var randomIndex = Random.Range(0, materials.Count-1);
        //Debug.Log(randomIndex);
        Material randomMaterial = materials[randomIndex];
        GetComponent<Renderer>().material = randomMaterial;
    }

    void Update()
    {
        //if (Application.isPlaying)
        //    return;



        //TextureAssign();

    }

    private Texture2D LoadTextureFromFile(string filepath)
    {
        Texture2D texture = new Texture2D(2, 2);
        byte[] bytes = File.ReadAllBytes(filepath);
        texture.LoadImage(bytes);
        return texture;
    }

    private void TextureAssign()
    {
        if (gameObject.transform.position == oldLocation && oldArea == quadArea)
        {
            return;
        }
        oldLocation = gameObject.transform.position;
        oldArea = quadArea;


        // Load all textures from folderPath


        // Choose a random texture and set it as the quad's texture
        if (textures.Length > 0)
        {
            Random.InitState((int)((gameObject.transform.position.x + 2.417289f + gameObject.transform.position.z) * 10));

            Texture2D randomTexture = textures[Random.Range(0, textures.Length)];
            GetComponent<Renderer>().material.mainTexture = randomTexture;

            // Calculate the aspect ratio of the randomTexture
            float textureAspect = (float)randomTexture.width / randomTexture.height;

            // Calculate the height of the quad to match the aspect ratio
            quadHeight = Mathf.Sqrt(quadArea / textureAspect);

            // Calculate the width of the quad to match the aspect ratio
            quadWidth = quadHeight * textureAspect;

            // Scale the quad to match the area while preserving the aspect ratio
            transform.localScale = new Vector3(quadWidth, quadHeight, 1);

        }
    }


}