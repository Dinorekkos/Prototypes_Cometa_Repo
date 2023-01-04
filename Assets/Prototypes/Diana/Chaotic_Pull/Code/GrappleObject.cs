using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Diana_ChaoticPull
{
    public class GrappleObject : MonoBehaviour, IGrappleInteractable
    {
        #region Variables

        [SerializeField] private GrappleInteractablesType _grappleInteractableType;
        
        public GrappleInteractablesType GrappleInteractablesType
        {
            get => _grappleInteractableType;
            set => _grappleInteractableType = value;
        }
        #endregion


        #region Unity Methods

        void Start()
        {

        }

        void Update()
        {

        }
        #endregion


        #region public methods

        

        public GrappleInteractablesType SendGrappleInteractableType(Vector3 position)
        {
            switch (GrappleInteractablesType)
            {
                case GrappleInteractablesType.PullObject:
                    PullObject(position);
                    break;
                    
                case GrappleInteractablesType.TargetObject:
                    SendPositionToPlayer(position);
                    break;
            }
            return GrappleInteractablesType;
        }

        public void PullObject(Vector3 targetPos)
        {
            Debug.Log("Pulling Object");
        }

        public void SendPositionToPlayer(Vector3 targetPos)
        {
            Debug.Log("Move Player to Target");

        }
        #endregion

    }
}