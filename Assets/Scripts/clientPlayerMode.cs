using Cinemachine;
using StarterAssets;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.InputSystem;

public class clientPlayerMode : NetworkBehaviour
{
    #region 
    [SerializeField] private PlayerInput m_PlayerInput;
    [SerializeField] private StarterAssetsInputs m_StarterAssetsInput;
    [SerializeField] private ThirdPersonController m_ThirdPersonController;
    void Awake()
    {
        m_StarterAssetsInput.enabled = false;
        m_PlayerInput.enabled = false;
        m_ThirdPersonController.enabled = false;
        
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if(IsOwner){
            m_StarterAssetsInput.enabled = true;
            m_PlayerInput.enabled = true;
            m_ThirdPersonController.enabled = true;
        }
        if(IsServer){
            m_ThirdPersonController.enabled = true;
        }
    }

    [Rpc(SendTo.Server)]
    private void UpdateInputServerRpc(Vector2 move, Vector2 look, bool jump, bool sprint)
    {
        m_StarterAssetsInput.MoveInput(move);
        m_StarterAssetsInput.LookInput(look);
        m_StarterAssetsInput.JumpInput(jump);
        m_StarterAssetsInput.SprintInput(sprint);
    }

    private void LateUpdate()
    {
        if(!IsOwner){
            return;
        }
        UpdateInputServerRpc(m_StarterAssetsInput.move, m_StarterAssetsInput.look, m_StarterAssetsInput.jump,m_StarterAssetsInput.sprint);
    }
    #endregion
}
