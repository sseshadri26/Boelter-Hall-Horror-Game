using UnityEngine;
using System.IO;
using System.Linq;

[ExecuteInEditMode]

public class FilePicker : MonoBehaviour
{
    public string folderPath;
    private Texture2D[] textures;

    public float quadWidth = 1.0f; // default width is 1 unit
    public float quadHeight;

    

    [SerializeField] public float quadArea = 20;

    float oldArea;

    private Vector3 oldLocation;

    private void Start()
    {
        string[] filepaths = Directory.GetFiles(folderPath);
        textures = filepaths
            .Where(filepath => filepath.EndsWith(".png") || filepath.EndsWith(".jpg") || filepath.EndsWith(".jpeg"))
            .Select(filepath => LoadTextureFromFile(filepath))
            .ToArray();
        //oldWidth = quadWidth;
        //oldLocation = gameObject.transform.position;
        TextureAssign();

        


    }
    void Update()
    {
        if (Application.isPlaying)
            return;



        TextureAssign();

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
            Random.InitState((int)((gameObject.transform.position.x + gameObject.transform.position.y + gameObject.transform.position.z) * 10));

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