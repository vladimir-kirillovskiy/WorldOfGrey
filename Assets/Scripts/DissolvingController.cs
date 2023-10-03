using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DissolvingController : MonoBehaviour
{
    [SerializeField]
    private SkinnedMeshRenderer skinnedMesh;
    [SerializeField]
    private VisualEffect vfxGraph;

    [SerializeField]
    private float refreshRate = 0.0125f;
    [SerializeField]
    private float dissolveRate = 0.025f;


    private Material[] skinnedMaterials;

    private void Awake()
    {
        if (skinnedMesh != null)
        {
            skinnedMaterials = skinnedMesh.materials;
        }
    }

    public void Dissolve()
    {
        StartCoroutine(DissolveCo());
    }

    private IEnumerator DissolveCo()
    {
        if (vfxGraph != null)
        {
            vfxGraph.Play();
        }

        if (skinnedMaterials.Length > 0)
        {
            float counter = 0;
            while (skinnedMaterials[0].GetFloat("_DissolveAmount") < 1)
            {
                counter += dissolveRate;
                for (int i = 0; i < skinnedMaterials.Length; i++)
                {
                    skinnedMaterials[i].SetFloat("_DissolveAmount", counter);
                }
                yield return new WaitForSeconds(refreshRate);
            }
        }
    }

    public void Appear()
    {
        Hide();
        StartCoroutine(AppearCo());
    }

    private void Hide()
    {
        if (skinnedMaterials.Length > 0)
        {
            for (int i = 0; i < skinnedMaterials.Length; i++)
            {
                skinnedMaterials[i].SetFloat("_DissolveAmount", 1);
            }

        }
    }

    private IEnumerator AppearCo()
    {
        if (skinnedMaterials.Length > 0)
        {
            float counter = 0;
            while (skinnedMaterials[0].GetFloat("_DissolveAmount") >= 0 )
            {
                counter += dissolveRate;
                for (int i = 0; i < skinnedMaterials.Length; i++)
                {
                    skinnedMaterials[i].SetFloat("_DissolveAmount", 1 - counter);
                }
                yield return new WaitForSeconds(refreshRate);
            }
        }
    }
}
