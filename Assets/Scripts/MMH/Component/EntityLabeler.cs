using System.Collections.Generic;
using TMPro;
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
                labels[i].transform.position = GetLabelPosition(entities[i].transform.position);
            }
        }


        private GameObject CreateLabel(GameObject entity)
        {
            GameObject newLabel = Instantiate(
                labelPrefab,
                GetLabelPosition(entity.transform.position),
                Quaternion.identity
            );

            newLabel.name = $"Label: {entity.name}";

            TextMeshProUGUI textMesh = newLabel.GetComponent<TextMeshProUGUI>();
            textMesh.text = entity.name;
            textMesh.fontSize = Info.UI.LabelFontSize;

            newLabel.transform.SetParent(labelsObject.transform, true);

            return newLabel;
        }


        private Vector3 GetLabelPosition(Vector3 entityPosition)
        {
            Vector3 screenPosition = Util.Map.WorldToScreen(entityPosition);
            screenPosition.y += Info.UI.LabelYOffset * Info.Render.CameraZoomRatio;

            return screenPosition;
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