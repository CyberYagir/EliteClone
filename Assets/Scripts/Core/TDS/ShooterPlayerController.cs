using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using DG.Tweening;
using UnityEngine;

namespace Core.TDS
{
    public class ShooterPlayerController : MonoBehaviour
    {
        [SerializeField] private Rigidbody rigidbody;
        [SerializeField] private float speed;
        [SerializeField] private Transform forward;
        [SerializeField] private Camera camera;
        [SerializeField] private Transform IKPoint;
        [SerializeField] private Transform IKhandsRotator;

        private Vector3 startPointL, startPointR;
        [SerializeField] private Transform LHand, RHand;

        private ShooterWeaponSelect weaponSelect;
        private ShooterWeaponList weaponsList;
        private ShooterInventory inventory;
        
        
        public Rigidbody GetRigidbody() => rigidbody;
        
        private void Start()
        {
            weaponSelect = GetComponent<ShooterWeaponSelect>();
            weaponsList = GetComponent<ShooterWeaponList>();
            inventory = GetComponent<ShooterInventory>();
            
            startPointL = LHand.localPosition;
            startPointR = RHand.localPosition;
        }

        public Camera GetCamera() => camera;

        private void OnEnable()
        {
            IKPoint.gameObject.SetActive(true);
        }

        public void CursorState(bool visible)
        {
            Cursor.visible = visible;
            Cursor.lockState = CursorLockMode.Confined;
        }

        private void OnDisable()
        {
            IKPoint.gameObject.SetActive(false);
        }

        public void FixedUpdate()
        {
            var oldY = rigidbody.velocity.y;
            var dir = new Vector3(InputM.GetAxis(KAction.Horizontal) * speed * Time.fixedDeltaTime, 0, InputM.GetAxis(KAction.Vertical) * speed * Time.fixedDeltaTime);
            var forwardDir = forward.TransformDirection(dir);
            rigidbody.velocity = forwardDir;
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, oldY - 1, rigidbody.velocity.z);

            
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore))
            {
                if (hit.transform != transform)
                {
                    var targetRotation = Quaternion.LookRotation(new Vector3(hit.point.x, 0, hit.point.z) - new Vector3(transform.position.x, 0, transform.position.z));
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 20 * Time.fixedDeltaTime);
                    var y = IKPoint.transform.position.y;
                    IKPoint.transform.position = hit.point + Vector3.up;

                    IKPoint.transform.position = new Vector3(IKPoint.transform.position.x, y, IKPoint.transform.position.z);
                    IKPoint.transform.position = Vector3.Lerp(IKPoint.transform.position, hit.point + Vector3.up, 5 * Time.deltaTime);
                    IKhandsRotator.localEulerAngles = Vector3.zero;
                    if (!weaponSelect.IsSelected())
                    {
                        LHand.localPosition = startPointL;
                        RHand.localPosition = startPointR;
                    }
                    else
                    {
                        var options = weaponsList.weapons.Find(x => x.item.id.id == inventory.items[weaponSelect.GetCurrent()].id.id).options;
                        options.leftH.Set(LHand);
                        options.rightH.Set(RHand);
                    }

                    IKhandsRotator.LookAt(IKPoint.transform.position);
                    var lpos = LHand.position;
                    var rpos = RHand.position;

                    IKhandsRotator.localEulerAngles = Vector3.zero;

                    LHand.position = lpos;
                    RHand.position = rpos;
                }
            }
        }

        public void SetPointPose(Vector3 transformPosition)
        {
            IKPoint.transform.DOMove(transformPosition + Vector3.up, 0.5f);
        }
    }
}
