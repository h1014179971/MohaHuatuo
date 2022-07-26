using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Foundation
{
    public class TextureTest 
    {
        public static Texture2D PointTest(Texture2D tex, int newWidth, int newHeight, Color color)
        {
            return new Texture2D(newWidth,newHeight,TextureFormat.ARGB32,false);
        }
    }
}

