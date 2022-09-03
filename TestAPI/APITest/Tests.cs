using NUnit.Framework;
using RestSharp;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using TestAPI.Utils;

namespace TestAPI.APITest
{
    public class Tests
    {
        private static dynamic _configData = JsonConvert.DeserializeObject<dynamic>(File.ReadAllText("../TestAPI/TestAPI/Resources/config.json"));
        private dynamic _localData = JsonConvert.DeserializeObject<dynamic>(File.ReadAllText("../TestAPI/TestAPI/Resources/testdata.json"));
        private APIMethods _aPIMethods = new APIMethods();   
        private RandomUtil _randomUtil = new RandomUtil();
        private string _url = _configData.url.ToString();
        

        [Test]
        public void Test1()
        {
            RestResponse response = _aPIMethods.ExecuteGetRequest("/posts", _url);
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Posts page was not reached");
            Assert.That(response.ContentType, Is.EqualTo("application/json"),"Content type is not .json");
            Assert.IsTrue(_aPIMethods.IsDataAscendingByID(data), "Id order is not ascending");
        }

        [Test]
        public void Test2()
        {
            RestResponse response = _aPIMethods.ExecuteGetRequest("/posts/99", _url);
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Content);
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Posts page was not reached");
            Assert.That(data.userId.Equals(_localData[1].PostData.userId), "Post userId does not match");
            Assert.That(data.id.Equals(_localData[1].PostData.id),"Post id does not match");
            Assert.That(!data.title.ToString().Equals(null), "Post title is empty");
            Assert.True(!data.body.ToString().Equals(null),"Post body is empty");
        }

        [Test]
        public void Test3()
        {
            RestResponse response = _aPIMethods.ExecuteGetRequest("/posts/150", _url);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound),"Post actually found");
            Assert.IsTrue(response.Content.ToString() == "{}","Post is not empty");
        }

        [Test]
        public void Test4()
        {
            RestClient restClient = new RestClient(_url);
            RestRequest request = new RestRequest("/posts", Method.Post);

            string randomBody = _randomUtil.RandomString();
            string randomTitle = _randomUtil.RandomString();
            int userId = _localData[2].PostData.userId;

            request.AddParameter("userId", userId);
            request.AddParameter("body", randomBody);
            request.AddParameter("title", randomTitle);
            RestResponse response = restClient.Execute(request);

            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created), "Post was not created");
            Assert.That(data.userId.ToString().Equals(userId.ToString()), "Post Id does not match");
            Assert.That(data.body.ToString().Equals(randomBody), "Post body does not match");
            Assert.That(data.title.ToString().Equals(randomTitle), "Post title does not match");
        }

        [Test]
        public void Test5()
        {
            RestResponse response = _aPIMethods.ExecuteGetRequest("/users", _url);
            dynamic Webdata = JsonConvert.DeserializeObject<dynamic>(response.Content);
            dynamic user_num = _aPIMethods.FindUserById(Webdata, _localData[0].UserData.id.ToString());

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Users page was not reached");
            Assert.That(response.ContentType, Is.EqualTo("application/json"), "Content type is not .json");
            Assert.That(user_num.ToString().Equals(_localData[0].UserData.ToString()), "User data does not match");
        }

        [Test]
        public void Test6()
        {
            RestResponse response = _aPIMethods.ExecuteGetRequest("/users/5", _url);
            dynamic Webdata = JsonConvert.DeserializeObject<dynamic>(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Users page was not reached");
            Assert.That(Webdata.ToString().Equals(_localData[0].UserData.ToString()), "User data does not match");
        }
    }
}
