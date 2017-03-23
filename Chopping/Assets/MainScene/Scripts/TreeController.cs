using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : MonoBehaviour
{
    [Header("Parameters")]
    [Range(1,60)]
    [SerializeField]
    private int MaxWoodCapacity;
    
    [Range(1, 5)]
    [SerializeField]
    private int ChopCount;

    [Header("References")]
    [SerializeField]



    #region private
    private bool growing = true;
    private int wood = 0;
    private int age = 0;
    #endregion


    // Use this for initialization
    void Start() {
        transform.localScale = Vector3.zero;
        StartCoroutine(GrowCoroutine());
    }

    IEnumerator GrowCoroutine() {
        while (growing && (age < MaxWoodCapacity)) {
            grow();
            yield return new WaitForSeconds(1.0f);
        }
    }

    void grow() {
        age++;
        wood++;
        float size = age / MaxWoodCapacity;        
        transform.localScale = new Vector3(size, size, size);
    }

    // Update is called once per frame
    void Update() {
        
    }

    /// <returns>count of chopped wood</returns>
    public int Chop() {
        growing = false;
        wood -= ChopCount;
        return ChopCount;
    }

}
