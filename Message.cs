namespace MasterServers
{
    public enum Message : short
    {
        GetServerListRequest = 1111,
        GetServerListResponse,
        GetServerByIdRequest,
        GetServerByIdResponse,
        AddServerRequest,
        AddServerResponse,
        RemoveServerRequest,
        RemoveServerResponse,
    }
}