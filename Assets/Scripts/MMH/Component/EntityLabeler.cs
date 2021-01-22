using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

namespace MMH
{
    public class EntityLabeler : MonoBehaviour
    {
        private GameObject labelsObject;
        private GameObject labelPrefab;

        private List<GameObject> labels;
        private List<GameObject> entities;


        void Awake()
        {
            labels = new List<GameObject>();
            labelsObject = GameObject.Find("Labels");
            labelPrefab = Resources.Load<GameObject>("Prefabs/UI/Label");

            entities = new List<GameObject>();
        }


        void Update()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                labels[i].transform.position = GetLabelPosition(entities[i]);
            }
        }


        private GameObject CreateLabel(GameObject entity)
        {
            GameObject newLabel = Instantiate(
                labelPrefab,
                GetLabelPosition(entity),
                Quaternion.identity
            );

            newLabel.name = $"Label: {entity.name}";

            TextMeshProUGUI textMesh = newLabel.GetComponent<TextMeshProUGUI>();
            textMesh.text = entity.name;
            textMesh.fontSize = Info.UI.LabelFontSize;

            newLabel.transform.SetParent(labelsObject.transform, true);

            return newLabel;
        }


        private Vector3 GetLabelPosition(GameObject entity)
        {
            float2 worldPosition = new float2(entity.transform.position.x, entity.transform.position.y);
            float2 screenPosition = Util.Map.WorldToScreen(worldPosition);

            screenPosition.y += Info.UI.LabelYOffset * Info.Render.CameraZoomRatio;

            Vector3 labelPosition = new Vector3(
                screenPosition.x,
                screenPosition.y + Info.UI.LabelYOffset * Info.Render.CameraZoomRatio,
                0
            );

            return labelPosition;
        }


        public void SelectEntity(GameObject entity)
        {
            Clear();

            entities = new List<GameObject>() { entity };
            labels = new List<GameObject>() { CreateLabel(entity) };
        }


        public void Clear()
        {
            entities = new List<GameObject>();
            labels = new List<GameObject>();

            foreach (Transform child in labelsObject.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}