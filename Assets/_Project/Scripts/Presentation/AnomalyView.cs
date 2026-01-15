using UnityEngine;
using _Project.Scripts.Domain;

namespace _Project.Scripts.Presentation
{
    public class AnomalyView : MonoBehaviour
    {
        [Header("Optional: Renderer/Animator etc.")]
        [SerializeField] private GameObject root;

        public AnomalyInstance instance { get; private set; }

        public virtual void Setup(AnomalyInstance inst)
        {
            instance = inst;
        }

        public virtual void ApplyActiveState(bool active)
        {
            if (root != null) root.SetActive(active);
            else gameObject.SetActive(active);
        }

        public virtual void PlayEffect()
        {
            //TODO
        }

        public virtual void StopEffect()
        {
            //TODO
        }
    }
}
