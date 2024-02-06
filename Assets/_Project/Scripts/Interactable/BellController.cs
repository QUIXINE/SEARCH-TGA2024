using UnityEngine;

public class BellController : MonoBehaviour, IButton
{
    [SerializeField] private PigController _pigController;
    public void Execute()
    {
        _pigController.enabled = true;
    }
}
