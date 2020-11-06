using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarl.Models;

namespace Yarl.Controllers
{
    public class PlayerDungeonController : MonoBehaviour
    {
        private IInteractable interactableInFocus;
        // <summary>
        /// If an interactable object is in focus and is allowed to interact, call its Interact() method.
        /// </summary>
        public void Update()
        {
            if (interactableInFocus != null)
            {
                if (interactableInFocus.IsInteractionAllowed())
                {
                    Debug.Log("In update interact");
                    interactableInFocus.Interact();
                }
                else
                {
                    Debug.Log("In update interact else");
                    interactableInFocus.EndInteract();
                    interactableInFocus = null;
                }
            }

        }

        /// <summary>
        /// If the collision is with an interactable object that is allowed to interact,
        /// make this object the current focus of the player.
        /// </summary>
        /// <param name="collider"></param>
        public void OnTriggerEnter2D(Collider2D collider)
        {
            var interactable = collider.GetComponent<IInteractable>();
            Debug.Log("Triggered on enter");
            if (interactable == null || !interactable.IsInteractionAllowed())
            {
                return;
            }

            interactableInFocus?.EndInteract();
            interactableInFocus = interactable;
            interactableInFocus.BeginInteract();
        }

        /// <summary>
        /// If the collision is with the interactable object that is currently the focus
        /// of the player, make the focus null.
        /// </summary>
        /// <param name="collider"></param>
        public void OnTriggerExit2D(Collider2D collider)
        {
            var interactable = collider.GetComponent<IInteractable>();
            Debug.Log("Triggered on exit");
            if (interactable == interactableInFocus)
            {
                interactableInFocus?.EndInteract();
                interactableInFocus = null;
            }
        }
    }
}