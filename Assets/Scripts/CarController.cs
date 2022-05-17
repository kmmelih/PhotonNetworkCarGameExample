using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    [SerializeField] private float maxDonusAcisi;
    [SerializeField] private float motorTorku;
    [SerializeField] private float frenGucu;
    [SerializeField] private float arabaHizi;
    

    [SerializeField] private WheelCollider solOnTekerCollider;
    [SerializeField] private WheelCollider sagOnTekerCollider;
    [SerializeField] private WheelCollider solArkaTekerCollider;
    [SerializeField] private WheelCollider sagArkaTekerCollider;

    private Transform solOnTekerTransform;
    private Transform sagOnTekerTransform;
    private Transform solArkaTekerTransform;
    private Transform sagArkaTekerTransform;

    private float verticalInput;
    private float horizontalInput;
    private bool isFren;
    private float anlikFrenGucu; 
    private float anlikDonusAcisi;
    
    private PhotonView _pv;
    public Text kullaniciAdiText;

    private void Start()
    {
        CameraWork _cameraWork = GetComponent<CameraWork>();
        _pv = GetComponent<PhotonView>();
        kullaniciAdiText.text = _pv.Owner.NickName;
        if (_pv.IsMine)
        {
            solOnTekerTransform = gameObject.transform.Find("Tekerler").Find("Mesh").Find("solOnTeker");
            solArkaTekerTransform = gameObject.transform.Find("Tekerler").Find("Mesh").Find("solArkaTeker");
            sagOnTekerTransform = gameObject.transform.Find("Tekerler").Find("Mesh").Find("sagOnTeker");
            sagArkaTekerTransform = gameObject.transform.Find("Tekerler").Find("Mesh").Find("sagArkaTeker");

            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                transform.position = new Vector3(15, 0, 0);
            }
            kullaniciAdiText.text = PhotonNetwork.NickName;
        }

        if (_cameraWork != null)
        {
            if (_pv.IsMine)
            {
                _cameraWork.OnStartFollowing();
            }
        }
    }

    private void Update()
    {
        if (_pv.IsMine)
        {
            inputKontrol();
            arabaKontrol();
            donusKontrol();
            tekerDonder();
        }
    }

    void inputKontrol()
    {
        if (_pv.IsMine)
        {
            verticalInput = Input.GetAxis("Vertical") * -1 * arabaHizi;
            horizontalInput = Input.GetAxis("Horizontal");
            isFren = Input.GetKey(KeyCode.Space);
        }
    }

    void arabaKontrol()
    {
        if (_pv.IsMine)
        {
            solOnTekerCollider.motorTorque = verticalInput * motorTorku;
            sagOnTekerCollider.motorTorque = verticalInput * motorTorku;
            anlikFrenGucu = isFren ? frenGucu : 0f;
            if (isFren)
            {
                solArkaTekerCollider.brakeTorque = anlikFrenGucu;
                sagArkaTekerCollider.brakeTorque = anlikFrenGucu;
                solOnTekerCollider.brakeTorque = anlikFrenGucu;
                sagOnTekerCollider.brakeTorque = anlikFrenGucu;
            }
        }
    }

    void donusKontrol()
    {
        if (_pv.IsMine)
        {
            anlikDonusAcisi = horizontalInput * maxDonusAcisi;
            sagOnTekerCollider.steerAngle = anlikDonusAcisi;
            solOnTekerCollider.steerAngle = anlikDonusAcisi;
        }
    }

    void tekerDonder()
    {
        if (_pv.IsMine)
        {
            tekerDonder(sagOnTekerCollider,sagOnTekerTransform);
            tekerDonder(solOnTekerCollider,solOnTekerTransform);
            tekerDonder(sagArkaTekerCollider,sagArkaTekerTransform);
            tekerDonder(solArkaTekerCollider,solArkaTekerTransform);
        }
    }

    void tekerDonder(WheelCollider tekerCollider, Transform tekerlekTransform) 
    {
        if (_pv.IsMine)
        {
            Vector3 position;
            Quaternion rotation;
            tekerCollider.GetWorldPose(out position, out rotation);
            tekerlekTransform.position = position;
            tekerlekTransform.rotation = rotation;
        }
    }
}
