using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class LvlSelectController : MonoBehaviour
{
    // Dummie Labels
    public Text lvl1;
    public Text lvl2;

    // LvlSave is used for loading in scores
    [Serializable]
    public struct LvlSave
    {
        public int deaths;

        public LvlSave(int deaths)
        {
            this.deaths = deaths;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Load in the scores of all the rounds

        LvlSave save = Load("saves/Lvl1.xml");
        if (save.deaths > -1)
        {   
            lvl1.text = "Deaths: " + save.deaths; 
        }

        save = Load("saves/Lvl2.xml");
        if (save.deaths > -1)
        {
            lvl2.text = "Deaths: " + save.deaths;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Load returns the information stored about a level save
    public LvlSave Load(string filename)
    {
        LvlSave state = new LvlSave(-1);
        if (File.Exists(filename))
        {

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(filename);
            string xmlString = xmlDocument.OuterXml;
            using (StringReader read = new StringReader(xmlString))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(LvlSave));
                using (XmlReader reader = new XmlTextReader(read))
                {
                    state = (LvlSave)serializer.Deserialize(reader);
                }
            }

            return state;
        }
        return state;

    }
}
