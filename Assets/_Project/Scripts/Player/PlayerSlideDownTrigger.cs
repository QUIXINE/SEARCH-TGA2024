using UnityEngine;


public class PlayerSlideDownTrigger : MonoBehaviour
{
    [SerializeField] private float _triggerNum;
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag(TagManager.Player) && !col.gameObject.TryGetComponent<PlayerSlidesDown>(out PlayerSlidesDown playerSlidesDown1)
            && _triggerNum == 1)
        {
            col.gameObject.AddComponent<PlayerSlidesDown>();
            Destroy(gameObject);
        }
        else if (col.gameObject.CompareTag(TagManager.Player) && col.gameObject.TryGetComponent<PlayerSlidesDown>(out PlayerSlidesDown playerSlidesDown2)
                                                              && _triggerNum == 2)
        {
            col.gameObject.GetComponent<PlayerController>().IsSlideFound = false;
            Destroy(playerSlidesDown2);
            Destroy(gameObject);
        }
    }
}
