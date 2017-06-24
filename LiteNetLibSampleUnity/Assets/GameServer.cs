using UnityEngine;
using LiteNetLib;
using LiteNetLib.Utils;
using System.Collections.Generic;

public class GameServer : MonoBehaviour, INetEventListener
{
	private float RotateSpeed = 1f;
	private float Radius = 1.0f;

	private Vector2 _centre;
	private float _angle;

    private NetManager _netServer;
    private NetPeer _ourPeer;
    private NetDataWriter _dataWriter;

    [SerializeField] private GameObject _serverBall;

	HashSet<NetPeer> netPeers = new HashSet<NetPeer>();

    void Start()
    {
        _dataWriter = new NetDataWriter();
        _netServer = new NetManager(this, 100, "sample_app");
        _netServer.Start(5000);
        _netServer.DiscoveryEnabled = true;
		_netServer.NatPunchEnabled = true;
        _netServer.UpdateTime = 100;
    }

    void Update()
    {
        _netServer.PollEvents();
		if (Input.GetKeyDown(KeyCode.Space)) {
			SendChimeToClients();
			//Invoke("SendChimeToClients", 1);
		}
    }

	void SendChimeToClients()
	{
		_dataWriter.Reset();
		_dataWriter.Put(true);
		foreach (NetPeer peer in netPeers) {
			peer.Send(_dataWriter, SendOptions.Unreliable);
		}
	}

    void FixedUpdate()
    {
		/*
        if (_ourPeer != null)
        {
			_angle += RotateSpeed * Time.deltaTime;
			var offset = new Vector2(Mathf.Sin(_angle), Mathf.Cos(_angle)) * Radius;
			_serverBall.transform.position = _centre + offset;
            _dataWriter.Reset();
            _dataWriter.Put(_serverBall.transform.position.x);
			foreach (NetPeer peer in netPeers) {
				peer.Send(_dataWriter, SendOptions.Sequenced);
			}


//            _serverBall.transform.Translate(1f * Time.fixedDeltaTime, 0f, 0f);
//            _dataWriter.Reset();
//            _dataWriter.Put(_serverBall.transform.position.x);
//            _ourPeer.Send(_dataWriter, SendOptions.Sequenced);
        }
        */
    }

    void OnDestroy()
    {
        if(_netServer != null)
            _netServer.Stop();
    }

    public void OnPeerConnected(NetPeer peer)
    {
        Debug.Log("[SERVER] We have new peer " + peer.EndPoint);
        _ourPeer = peer;
		netPeers.Add(peer);
    }

    public void OnPeerDisconnected(NetPeer peer, DisconnectReason reason, int socketErrorCode)
    {
		netPeers.Remove(peer);
    }

    public void OnNetworkError(NetEndPoint endPoint, int socketErrorCode)
    {
        Debug.Log("[SERVER] error " + socketErrorCode);
    }

    public void OnNetworkReceiveUnconnected(NetEndPoint remoteEndPoint, NetDataReader reader, UnconnectedMessageType messageType)
    {
        if (messageType == UnconnectedMessageType.DiscoveryRequest)
        {
            Debug.Log("[SERVER] Received discovery request. Send discovery response");
            _netServer.SendDiscoveryResponse(new byte[] {1}, remoteEndPoint);
        }
    }

    public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
    {
        
    }

    public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
    {
        Debug.Log("[SERVER] peer disconnected " + peer.EndPoint + ", info: " + disconnectInfo.Reason);
        if (peer == _ourPeer)
            _ourPeer = null;

		netPeers.Remove(peer);
    }

    public void OnNetworkReceive(NetPeer peer, NetDataReader reader)
    {
        
    }
}
