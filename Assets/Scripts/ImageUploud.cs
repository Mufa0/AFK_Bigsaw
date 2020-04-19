using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageUploud : MonoBehaviour
{
    public Material puzzleMaterial;
    public void GalleryPrompt()
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            Debug.Log("Image path: " + path);
            if (path != null)
            {
                // Create Texture from selected image
                Texture2D texture = NativeGallery.LoadImageAtPath(path);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }
                puzzleMaterial.mainTexture = texture;

                // If a procedural texture is not destroyed manually, 
                // it will only be freed after a scene change
              
            }
        }, "Select a PNG image", "image/png");
    }
}
