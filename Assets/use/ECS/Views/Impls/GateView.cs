using UnityEngine;

namespace ECS.Views.Impls
{
    public class GateView : InteractableView
    {
        [SerializeField] private GameObject neighbour;
        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            neighbour.SetActive(false);
        }
    }
}