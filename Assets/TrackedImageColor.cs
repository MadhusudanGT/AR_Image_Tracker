using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.ARFoundation; // import the XR libraries
using TMPro;
public class TrackedImageColor : MonoBehaviour
{
    [SerializeField]
    ARTrackedImageManager m_TrackedImageManager;

    [SerializeField] 
    List<AlphabetTracker> prefabSpawns;

    [SerializeField]
    TMP_Text _selectedTxt;
    [SerializeField] List<GameObject> spawnedObjects;

    void OnEnable() => m_TrackedImageManager.trackedImagesChanged += OnChanged;

    void OnDisable() => m_TrackedImageManager.trackedImagesChanged -= OnChanged;

    void OnChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var newImage in eventArgs.added)
        {
            _selectedTxt.text = newImage.referenceImage.name;
            Debug.Log(newImage.referenceImage.name);
            SpawnItems(newImage);
        }

        foreach (var removedImage in eventArgs.removed)
        {
            Destroy(removedImage.gameObject);
        }
    }

    void SpawnItems(ARTrackedImage trackedAlphabet)
    {
        if (trackedAlphabet.referenceImage.name == null)
        {
            _selectedTxt.text = "NULL";
            return;
        }

        _selectedTxt.text = trackedAlphabet.referenceImage.name;

        foreach (GameObject item1 in spawnedObjects)
        {
            GameObject.Destroy(item1);
        }

        spawnedObjects.Clear();

        string alphabetN = trackedAlphabet.referenceImage.name;
        AlphabetTracker item = prefabSpawns.FirstOrDefault(item => item.alphabetName == alphabetN);
        GameObject obj = Instantiate(item.alphabetPrefab);
        obj.name = alphabetN;
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = trackedAlphabet.transform.position;
        obj.gameObject.SetActive(true);

        spawnedObjects.Add(obj);
    }
}

[System.Serializable]
public struct AlphabetTracker
{
    public string alphabetName;
    public GameObject alphabetPrefab;
}