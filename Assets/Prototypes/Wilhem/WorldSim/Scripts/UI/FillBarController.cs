using UnityEngine;
using UnityEngine.UI;


namespace CometaPrototypes.WorldSim
{
    public class FillBarController : MonoBehaviour
    {
        [SerializeField] private Image fillImage;

        public void SetFillLevel(float fillLevel)
        {
            fillImage.fillAmount = fillLevel;
        }
    }
}