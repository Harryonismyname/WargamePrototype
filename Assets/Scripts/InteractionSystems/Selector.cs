using System;
using UnityEngine;
namespace PolearmStudios.SelectionSystem
{
    public class Selector : MonoBehaviour
    {
        [SerializeField] LayerMask SelectableLayers;
        public ISelectable CurrentSelection { get; private set; }

        readonly int sampleSize = 2;
        readonly Collider[] selectionColls = new Collider[1];

        public static event Action<ISelectable> OnSelectionUpdated;

        private void Awake()
        {
            // Add in here whatever system you have to handle clicks converting them to world points
            PlayerInputHandler.OnClick += HandleClick;
        }

        private void OnDestroy()
        {
            PlayerInputHandler.OnClick -= HandleClick;
        }

        private void HandleClick(Vector3 point)
        {
            if (!Physics.CheckSphere(point, sampleSize, SelectableLayers))
            {
                DeselectCurrentObject();
                return;
            }

            var count = Physics.OverlapSphereNonAlloc(point, sampleSize, selectionColls, SelectableLayers);
            if (count < 1 || !selectionColls[0].TryGetComponent(out ISelectable newSelection))
            {
                DeselectCurrentObject();
                return;
            }

            SelectObject(newSelection);
        }

        public void SelectObject(ISelectable selection)
        {
            CurrentSelection?.OnDeselect();
            CurrentSelection = selection;
            CurrentSelection?.OnSelect();
            OnSelectionUpdated?.Invoke(CurrentSelection);
        }

        public void DeselectCurrentObject()
        {
            CurrentSelection?.OnDeselect();
            CurrentSelection = null;
            OnSelectionUpdated?.Invoke(CurrentSelection);
        }
    }

}