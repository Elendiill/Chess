using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareSelectorCreator : MonoBehaviour
{
    [SerializeField] private Material freeSquareMaterial;
    [SerializeField] private Material opponentSquareMaterial;
    [SerializeField] private GameObject selectorPrefab;
    private List<GameObject> instantiatedSelectors = new List<GameObject>();

    public void ShowSelection(Dictionary<Vector3, bool> squareData)
    {
        ClearSelection();
        Vector3 zOffset = new Vector3(0, 0, -1);
        foreach (var data in squareData)
        {
            GameObject selector = Instantiate(selectorPrefab, data.Key + zOffset, Quaternion.identity);
            instantiatedSelectors.Add(selector);
            foreach (var setter in selector.GetComponentsInChildren<SpriteRenderer>())
            {
                setter.material = data.Value ? freeSquareMaterial : opponentSquareMaterial;
            }
        }
    }

    public void ClearSelection()
    {
        foreach (var selector in instantiatedSelectors)
        {
            Destroy(selector.gameObject);
        }
        instantiatedSelectors.Clear();
    }
}
