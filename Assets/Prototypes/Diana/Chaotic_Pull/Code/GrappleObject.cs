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

        

        public GrappleInteractablesType SendGrappleInteractableType(Vector3 position, float speed)
        {
            switch (GrappleInteractablesType)
            {
                case GrappleInteractablesType.PullObject:
                    PullObject(position, speed);
                    break;
                    
                case GrappleInteractablesType.TargetObject:
                    break;
            }
            return GrappleInteractablesType;
        }

        void PullObject(Vector3 targetPos, float speed)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        }

       
        #endregion

    }
}