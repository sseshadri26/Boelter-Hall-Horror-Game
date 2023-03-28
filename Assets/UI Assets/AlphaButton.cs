using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AlphaButton : Button
{
    public new class UxmlFactory : UxmlFactory<AlphaButton, UxmlTraits> { }

    // Rewires the button's logic to detect a click/hover only when the mouse is
    // hovering over a pixel value in its image that has an alpha greater than some threshold.
    // This is useful for buttons that appear to have non-square shapes.
    public override bool ContainsPoint(Vector2 localPoint)
    {
        // NOTE: the property "Scale Mode" property must be on "stretch-to-fit" for now, since
        // this implementation is hard-wired to work for this specific mode. Perhaps it would be
        // a good idea to adjust for this later.

        // The given localPoint is in the button's coordinate space, which is different from the texture's.
        // For this reason, we must map the button space to the texture space and then locate the color
        // of the pixel at that location on the texture.

        // NOTE: the resolvedStyle property seems to be the property we see in UI Builder and represents the "final"
        // styling applied to the element? Not too sure, gonna have to fact-check me.
        Texture2D imageTexture = resolvedStyle.backgroundImage.texture;
        float image_to_button_x_ratio = imageTexture.width/resolvedStyle.width;
        float image_to_button_y_ratio = imageTexture.height/resolvedStyle.height;

        // Where the mouse pointer X is located in the texture's coordinates
        int simulatedTexturePointX = (int)(localPoint.x * image_to_button_x_ratio);

        // Where the mouse pointer Y is located in the texture's coordinates
        // Textures have a coordinate system with origin at bottom left, whereas whereas visual elements have origin top left, hence the sign flip
        int simulatedTexturePointY = (int)((resolvedStyle.height - localPoint.y) * image_to_button_y_ratio);

        Color colorAtButtonLocation = imageTexture.GetPixel(simulatedTexturePointX, simulatedTexturePointY);

        // Debug.LogFormat("TEXTURE POS - W: ({0}, {1}) H: ({2}, {3})", simulatedTexturePointX, imageTexture.width, simulatedTexturePointY, imageTexture.height);
        // Debug.LogFormat("BUTTON POS - W: ({0}, {1}) H: ({2}, {3})", localPoint.x, resolvedStyle.width, localPoint.y, resolvedStyle.height);
        // Debug.LogFormat("COLOR AT LOCATION: {0}", colorAtButtonLocation);
        
        return colorAtButtonLocation.a > 0.5f;
    }

}
