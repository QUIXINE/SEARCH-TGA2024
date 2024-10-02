using System.Collections;
using UnityEngine;

public class WendigoTakeDamage : MonoBehaviour
{
    [SerializeField] private WendigoStateManager _wendigoStateManager;
    public void TakeDamage()
    {
        Collider thisCollider = GetComponent<Collider>();
        Collider[] childrenColliders = GetComponentsInChildren<Collider>();
        thisCollider.enabled = false;
        foreach (var collider in childrenColliders)
        {
            collider.enabled = false;
        }
        transform.rotation = Quaternion.Euler(-90, 90, 0);
        _wendigoStateManager.MyAnimator.SetBool("Walk", false);
        StartCoroutine(StopAnimation());
        _wendigoStateManager.enabled = false;
    }

    private IEnumerator StopAnimation()
    {
        yield return new WaitForSecondsRealtime(5f);
        Destroy(gameObject);
    }
}
