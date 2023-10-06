using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Voice.PUN;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class AvatarPalette : MonoBehaviour {

    [SerializeField]
    public List<Color> palette;
    //public bool[] colorsCurrentlyUsed;

    private Color selectedCol;

    private List<Color> backup = new List<Color>();

    void Start()

    {
        //custom inputs to control transparency - FigJ
        backup.Add(new Color(1, 0, 0, 0.9f)); //red
        backup.Add(new Color(0, 1, 0, 0.9f)); //green
        backup.Add(new Color(0, 0, 1, 0.9f)); //blue
        backup.Add(new Color(0, 1, 1, 0.9f)); //cyan
        backup.Add(new Color(1, 1, 0, 0.9f)); //yellow
        backup.Add(new Color(1, 0, 1, 0.9f)); //magenta
        backup.Add(new Color(0, 0, 0, 0.9f)); //black
        backup.Add(new Color(1, 1, 1, 0.9f)); //white
        backup.Add(new Color(0.5f, 0.5f, 0.5f, 0.9f)); //gray

      //  colorsCurrentlyUsed = new bool[palette.Count];
    }


    public Color PickColor(int pick)
    {
        if (palette.Count == 0)
            palette = backup;

        //var pick = Random.Range(0, palette.Count);
        //var selectedCol = palette[pick];
        while (pick >= palette.Count)
        {
            //easy method- while it's larger than palette count, subtract palette count from it until it's smaller so it cycles again
            pick -= palette.Count;
        }

        if(pick < 0)
        {
            pick = 0;
        }
        /* maybe re-implement this later? Something's causing this to greatly affect the other user colors and I don't like it
        int numberOfColorsBeingUsed = 0;
        while (colorsCurrentlyUsed[pick] && numberOfColorsBeingUsed < colorsCurrentlyUsed.Length)
        {
            //if this color is being used, go to the next one, and loop around until it can't be used anymore!
            pick++;
            numberOfColorsBeingUsed++;
            if(pick >= palette.Count)
            {
                pick = 0;
            }
        }*/


        selectedCol = palette[pick];
   //     colorsCurrentlyUsed[pick] = true;
        //palette.RemoveAt(pick); //no 2 avatars will have the same color. Can change this later if need be - FigJ

        return selectedCol;
    }
    /*
    public void FreeColor(int id)
    {
        // call when user leaves, tell the bool that the color in the id isn't being used
        while(id >= palette.Count)
        {
            id -= palette.Count;
        }

        colorsCurrentlyUsed[id] = false;
    }*/
}
