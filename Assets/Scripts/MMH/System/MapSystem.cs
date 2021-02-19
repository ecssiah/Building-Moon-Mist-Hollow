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

        private Component.Map map;
        public Component.Map Map { get => map; }


        void Awake()
        {
            renderSystem = GameObject.Find("Render System").GetComponent<RenderSystem>();

            map = GameObject.Find("Map").AddComponent<Component.Map>();

            map.SetupMap();
            map.ConstructMap();
            map.CalculateEdges();
        }


        public void ConstructCell(Data.Cell cellData)
        {
            renderSystem.ConstructCell(cellData);
        }


        public void SelectCell(int2 cellPosition)
        {
            map.SelectCell(cellPosition);
        }


        public void SaveMapData(string name)
        {
            using (StreamWriter file = File.CreateText($"Assets/Resources/Data/{name}.json"))
            {
                string jsonCellsText = JsonUtility.ToJson(map, true);

                file.Write(jsonCellsText);
            }
        }


        public void LoadMapData(string name)
        {
            using (StreamReader reader = new StreamReader($"Assets/Resources/Data/{name}.json"))
            {
                string jsonText = reader.ReadToEnd();

                map.SetData(JsonUtility.FromJson<Data.Map>(jsonText));
                map.ConstructMap();
            }
        }
    }
}