using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TreeController : MonoBehaviour
{
    #region visible in inspector 
    [Header("Parameters")]
    [Range(1,60)]
    [SerializeField]
    private int MaxWoodCapacity;

    [Range(0.01f, 2.0f)]
    [SerializeField]
    private float GrowTimeStepSize;

    [Range(1, 5)]
    [SerializeField]
    private int ChopCount;

    [Header("References")]
    [SerializeField]
    private ParticleSystem leafsParticlesEmiter;

    #endregion

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
            yield return new WaitForSeconds(GrowTimeStepSize);
        }
    }

    void grow() {
        age++;
        wood++;
        float size = (float)age / (float)MaxWoodCapacity;        
        transform.localScale = new Vector3(size, size, size);
    }

    void checkForDebugInput() {
        if (Input.GetKey("c")) {
            Chop();
        }
    }

    // Update is called once per frame
    void Update() {

#if UNITY_EDITOR
        checkForDebugInput();
#endif

    }

    /// <returns>count of chopped wood</returns>
    public int Chop() {
        growing = false;
        leafsParticlesEmiter.Play();
        transform.DOShakeRotation(0.2f, new Vector3(7f, 0f, 7f), 40, 10, false).OnComplete(tweenToZeroRotation);
        wood -= ChopCount;
        return ChopCount;
    }

    void tweenToZeroRotation() {
        transform.DORotate(Vector3.zero, 0.1f, RotateMode.Fast);
    }

    

}
