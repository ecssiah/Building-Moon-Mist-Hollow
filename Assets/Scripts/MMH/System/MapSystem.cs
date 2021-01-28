using System;
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEngine;

namespace MMH.System
{
    public class MapSystem : MonoBehaviour
    {
        private RenderSystem renderSystem;

        private Map map;


        void Awake()
        {
            renderSystem = GameObject.Find("RenderSystem").GetComponent<RenderSystem>();

            map = GameObject.Find("Map").AddComponent<Map>();
        }


        public void ConstructCell(Data.Cell cellData)
        {
            renderSystem.SetCell(cellData);
        }


        public void SelectCell(int2 position)
        {
            renderSystem.SetOverlay(position, Type.Overlay.Selection);
        }


        public void ClearSelection()
        {
            renderSystem.SetOverlay(map.SelectedCell, Type.Overlay.None);
        }


        public List<int> GetEdgeData()
        {
            return new List<int>();
        }


        public void SaveMapData(string name)
        {
            using (StreamWriter file = File.CreateText($"Assets/Resources/Data/{name}.json"))
            {
                //string jsonCellsText = JsonUtility.ToJson(mapData, true);

                //file.Write(jsonCellsText);
            }
        }


        public void LoadMapData(string name)
        {
            using (StreamReader reader = new StreamReader($"Assets/Resources/Data/{name}.json"))
            {
                //string jsonText = reader.ReadToEnd();

                //mapData = JsonUtility.FromJson<Data.Map>(jsonText);
            }
        }
    }
}