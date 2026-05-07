using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class diceHandler : MonoBehaviour
{
    public Image diceImage;
    public Sprite[] diceSprites;
    private int diceNumber;
    public void tossDice(){
         diceNumber = UnityEngine.Random.Range(1,7); 
         diceImage.sprite = diceSprites[diceNumber - 1];
    }
    public int GetDiceFace(){
        return diceNumber;
    }
}
