using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//THIS SCRIPT IS DESIGNED TO STORE ALL OF THE INFORMATION REGARDING THE AVAILABLE LANGUAGES,
//THE GAMES AVAILABLE FOR EACH LANGUAGE, AND OTHER SUCH CONTENT.


public class LangData : MonoBehaviour
{
    public TMPro.TMP_Dropdown langs;
    public Language[] langList;
    public int index;
    
    // Start is called before the first frame update
    void Start()
    {
        langList = new Language[langs.options.Count];
        for(int i = 0; i < langList.Length; i++)
        {
            langList[i] = new Language(langs.options.ToArray()[i].ToString());
        }
        
        //SHOULD BE TURNED INTO XML OR JSON AT SOME POINT
        //PP= Present Progressive
        //C = Cooking
        //0 = English, 1 = Spanish
        Topic PPC0 = new Topic("Present Progressive Cooking", 0);
        Topic PPC1 = new Topic("Present Progressive Cooking", 1);
        //^^^

        Topic[] tops = new Topic[] { PPC0, PPC1 };
        foreach(Topic top in tops)
        {
            langList[top.language].lessons.AddLast(top);
        }

    }
    
    public void UpdateGameList()
    {
        //langs.value
    }

}
