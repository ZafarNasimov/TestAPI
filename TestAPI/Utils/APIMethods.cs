using RestSharp;


namespace TestAPI.Utils
{
    public class APIMethods
    {
        public RestResponse ExecuteGetRequest(string _request, string url)
        {
            RestClient restClient = new RestClient(url);
            RestRequest request = new RestRequest(_request, Method.Get);
            return restClient.Execute(request);
        }

        public bool IsDataAscendingByID(dynamic data)
        {
            for (int i = 0; i < data.Count - 1; i++)
            {
                if (data[i].id > data[i + 1].id)
                {
                    return false;
                }
            }
            return true;
        }

        public dynamic FindUserById(dynamic data, string id)
        {
            for (int i = 1; i < data.Count; i++)
                if (data[i].id.ToString() == id)
                    return data[i];
            return null;
        }
    }
}
