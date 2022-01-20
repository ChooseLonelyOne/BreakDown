using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _cameraTarget;
    [SerializeField] private CinemachineVirtualCamera _cameraBig;
    private void Awake()
    {
        TeamManager.SelectedPlayer += FollowPlayer;
        TeamManager.UnSelectedPlayer += UnFollowPlayer;
        TeamManagerrr.UnSelectedPlayer += UnFollowPlayer;
        TeamManagerrr.SelectedPlayer += FollowPlayer;
    }

    private void FollowPlayer(Transform follow)
    {
        _cameraTarget.gameObject.SetActive(true);
        _cameraBig.gameObject.SetActive(false);
        _cameraTarget.m_Follow = follow;
    }

    private void UnFollowPlayer()
    {
        _cameraBig.gameObject.SetActive(true);
        _cameraTarget.gameObject.SetActive(false);
    }

    public void DownPlusButton()
    {
        _cameraBig.gameObject.SetActive(true);
        _cameraTarget.gameObject.SetActive(false);
    }

    public void UpPlusButton()
    {
        _cameraTarget.gameObject.SetActive(true);
        _cameraBig.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        TeamManager.SelectedPlayer -= FollowPlayer;
        TeamManager.UnSelectedPlayer -= UnFollowPlayer;
        TeamManager.SelectedPlayer -= FollowPlayer;
        TeamManager.UnSelectedPlayer -= UnFollowPlayer;
    }
}
