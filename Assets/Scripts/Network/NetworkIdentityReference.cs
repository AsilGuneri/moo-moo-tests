using Mirror;
[System.Serializable]
public class NetworkIdentityReference
{
    public uint networkId { get; private set; }

    private NetworkIdentity _networkIdentityCached = null;

    public NetworkIdentity Value{
        get{
            if(networkId == 0)
                return null;
            if(_networkIdentityCached == null){
                _networkIdentityCached = NetworkIdentity.spawned[networkId];
            }

            return _networkIdentityCached;
        }
    }

    public NetworkIdentityReference(){}

    public NetworkIdentityReference(NetworkIdentity networkIdentity){
        if(networkIdentity == null)
            return;

        networkId = networkIdentity.netId;
        _networkIdentityCached = networkIdentity;
    }

    public NetworkIdentityReference(uint networkId){
        this.networkId = networkId;
    }
}

public static class NetworkIdentityReferenceReaderWriter{
    public static void WriteNetworkIdentityReference(this NetworkWriter writer, NetworkIdentityReference nir){
        if(nir == null || nir.Value == null){
            writer.WriteUInt(0);
        }
        else{
            writer.WriteUInt(nir.Value.netId);
        }
    }

    public static NetworkIdentityReference ReadNetworkIdentityReference(this NetworkReader reader){
        return new NetworkIdentityReference(reader.ReadUInt());
    }
}
