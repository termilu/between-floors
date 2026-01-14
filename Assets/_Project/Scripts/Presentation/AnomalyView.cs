using UnityEngine;
using _Project.Scripts.Domain;

namespace _Project.Scripts.Presentation
{
    public class AnomalyView : MonoBehaviour
    {
        [Header("Optional: Renderer/Animator etc.")]
        [SerializeField] private GameObject root;

        public AnomalyInstance instance { get; private set; }

        public void Setup(AnomalyInstance inst)
        {
            instance = inst;
            ApplyActiveState(inst != null && inst.isActive);
        }

        public void ApplyActiveState(bool active)
        {
            if (root != null) root.SetActive(active);
            else gameObject.SetActive(active);
        }

        public void PlayEffect()
        {
            //TODO
        }

        public void StopEffect()
        {
            //TODO
        }
    }
}