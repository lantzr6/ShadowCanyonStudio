using ModernHttpClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TriviaTraverse.Helpers;
using TriviaTraverse.Models;

namespace TriviaTraverse.Api
{
    public class WebApi
    {
        //Static instace so we can acces this class everywhere
        public static WebApi Instance { get; } = new WebApi();

        //Our base API url
        //static string _baseUrl { get { return "https://triviamobileapi20170617041221.azurewebsites.net/api/"; } }
        static string _baseUrl { get { return "http://192.168.0.5:57579/api/"; } }

        public HttpClient CreateClient()
        {
            var httpClient = new HttpClient(new NativeMessageHandler())
            {
                BaseAddress = new Uri(_baseUrl),

            };
            // We want the response to be JSON.
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            return httpClient;
        }

        public async Task<string> GetAuthToken()
        {
            string retVal = "";
            try
            {
                var url = _baseUrl + string.Format("token");
                using (var httpClient = CreateClient())
                {
                    // Build up the data to POST.
                    List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>();
                    postData.Add(new KeyValuePair<string, string>("grant_type", "password"));
                    postData.Add(new KeyValuePair<string, string>("username", App.PlayerObj.EmailAddr));
                    postData.Add(new KeyValuePair<string, string>("password", App.PlayerObj.Password));

                    FormUrlEncodedContent content = new FormUrlEncodedContent(postData);

                    // Post to the Server and parse the response.
                    var response = await httpClient.PostAsync("Token", content);
                    string jsonString = await response.Content.ReadAsStringAsync();
                    object responseData = JsonConvert.DeserializeObject(jsonString);

                    // return the Access Token.
                    string token = ((dynamic)responseData).access_token;

                    string seconds = ((dynamic)responseData).expires_in;

                    DateTime tokenexp = DateTime.Now.AddSeconds(Convert.ToDouble(seconds));

                    Settings.AuthToken = token;
                    Settings.AuthTokenExpire = tokenexp;

                    retVal = "Ok";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem " + ex.Message);
            }
            return retVal;
        }

        public async Task<Player> GetNewTempPlayerAsync()
        {
            Player player = null;
            try
            {
                var url = _baseUrl + string.Format("Start/NewPlayer");
                using (var httpClient = CreateClient())
                {
                    var result = await httpClient.GetAsync(url);
                    var responseText = await result.Content.ReadAsStringAsync();

                    player = JsonConvert.DeserializeObject<Player>(responseText);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem " + ex.Message);
            }
            return player;
        }

        public async Task<GameSection> GetTutorialAsync(int playerId)
        {
            GameSection tutorial = null;
            try
            {
                if (Settings.AuthTokenExpire < DateTime.Now)
                {
                    await GetAuthToken();
                }
                var url = _baseUrl + string.Format("Start/GetTutorial/?player = " + playerId.ToString());
                using (var httpClient = CreateClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Settings.AuthToken);
                    var result = await httpClient.GetAsync(url);
                    var responseText = await result.Content.ReadAsStringAsync();

                    tutorial = JsonConvert.DeserializeObject<GameSection>(responseText);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem " + ex.Message);
            }
            return tutorial;
        }

        public async Task<GameSection> GetSectionAsync(int sectionId, int playerId, int playerCampaignId, bool retry)
        {
            GameSection section = null;
            try
            {
                if (Settings.AuthTokenExpire < DateTime.Now)
                {
                    await GetAuthToken();
                }
                var url = _baseUrl + "Start/GetSection/?id=" + sectionId.ToString() + "&player=" + playerId.ToString() + "&pcId=" + playerCampaignId.ToString() + "&retry=" + retry;
                using (var httpClient = CreateClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Settings.AuthToken);
                    var result = await httpClient.GetAsync(url);
                    if (result.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    {
                        await GetAuthToken();
                        httpClient.DefaultRequestHeaders.Clear();
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Settings.AuthToken);
                        result = await httpClient.GetAsync(url);
                    }
                    var responseText = await result.Content.ReadAsStringAsync();

                    section = JsonConvert.DeserializeObject<GameSection>(responseText);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem " + ex.Message);
            }
            return section;
        }


        public async Task<Player> GetAccountAsync(string emailaddress, string password)
        {
            Player retVal = null;
            try
            {
                System.Diagnostics.Debug.WriteLine("getting account");
                string url = _baseUrl + string.Format("Account/GetAccount?email={0}&password={1}",emailaddress, password);
                HttpClient httpClient = CreateClient();
                HttpResponseMessage result = await httpClient.GetAsync(url);
                    string responseText = await result.Content.ReadAsStringAsync();

                    retVal = JsonConvert.DeserializeObject<Player>(responseText);
                    System.Diagnostics.Debug.WriteLine("finished getting: " + retVal);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem " + ex.Message);
            }
            return retVal;
        }

        public async Task<string> UpdateAccountAsync(Player inObj)
        {
            string retVal = "";
            try
            {
                if (Settings.AuthTokenExpire < DateTime.Now)
                {
                    await GetAuthToken();
                }
                var url = _baseUrl + string.Format("Account/UpdateAccount");
                using (var httpClient = CreateClient())
                {
                    var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(inObj));
                    var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

                    var result = await httpClient.PostAsync(url, httpContent);
                    var responseText = await result.Content.ReadAsStringAsync();

                    retVal = responseText;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem " + ex.Message);
            }
            Debug.WriteLine("Account Updated");
            return retVal;
        }

        public async Task<string> UpdateTutorialMessageStatusAsync(TutorialMessagesStatus inObj)
        {
            string retVal = "";
            try
            {
                if (Settings.AuthTokenExpire < DateTime.Now)
                {
                    await GetAuthToken();
                }
                var url = _baseUrl + string.Format("Start/UpdateTutorialMessageStatus");
                using (var httpClient = CreateClient())
                {
                    var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(inObj));
                    var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

                    var result = await httpClient.PostAsync(url, httpContent);
                    var responseText = await result.Content.ReadAsStringAsync();

                    retVal = responseText;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem " + ex.Message);
            }
            Debug.WriteLine("Tutorial Updated");
            return retVal;
        }
        
        public async Task<VGame> UpdateVGameAsync(VGamePlayerUpdate inObj)
        {
            VGame retVal = null;
            try
            {
                if (Settings.AuthTokenExpire < DateTime.Now)
                {
                    await GetAuthToken();
                }
                var url = _baseUrl + "Game/UpdateVGamePlayer";
                using (var httpClient = CreateClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Settings.AuthToken);

                    var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(inObj));
                    var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

                    var result = await httpClient.PostAsync(url, httpContent);
                    if (result.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    {
                        await GetAuthToken();
                        httpClient.DefaultRequestHeaders.Clear();
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Settings.AuthToken);
                        result = await httpClient.PostAsync(url, httpContent);
                    }
                    var responseText = await result.Content.ReadAsStringAsync();

                    retVal = JsonConvert.DeserializeObject<VGame>(responseText);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem " + ex.Message);
            }
            return retVal;
        }


        public async Task<string> PostCampaignQuestionResults(PlayerQuestionResult inObj)
        {
            string retVal = "";
            try
            {
                if (Settings.AuthTokenExpire < DateTime.Now)
                {
                    await GetAuthToken();
                }
                var url = _baseUrl + "Question/UpdateCampaignQuestionResult";
                using (var httpClient = CreateClient())
                {
                    var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(inObj));
                    var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Settings.AuthToken);
                    var result = await httpClient.PostAsync(url, httpContent);
                    var responseText = await result.Content.ReadAsStringAsync();

                    retVal = responseText;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem " + ex.Message);
            }
            return retVal;
        }

        public async Task<string> PostVGameQuestionResults(PlayerVGameQuestionResult inObj)
        {
            string retVal = "";
            try
            {
                if (Settings.AuthTokenExpire < DateTime.Now)
                {
                    await GetAuthToken();
                }
                var url = _baseUrl + "Question/UpdateVGameQuestionResult";
                using (var httpClient = CreateClient())
                {
                    var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(inObj));
                    var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Settings.AuthToken);
                    var result = await httpClient.PostAsync(url, httpContent);
                    var responseText = await result.Content.ReadAsStringAsync();

                    retVal = responseText;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem " + ex.Message);
            }
            return retVal;
        }

        public async Task<Campaign> GetCampaign(int playerid)
        {
            Campaign retVal = null;
            try
            {
                if (Settings.AuthTokenExpire < DateTime.Now)
                {
                    await GetAuthToken();
                }
                var url = _baseUrl + "Campaign/Retrieve/?playerid=" + playerid.ToString();
                using (var httpClient = CreateClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Settings.AuthToken);

                    HttpResponseMessage result = await httpClient.GetAsync(url);
                    if (result.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    {
                        await GetAuthToken();
                        httpClient.DefaultRequestHeaders.Clear();
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Settings.AuthToken);
                        result = await httpClient.GetAsync(url);
                    }
                    string responseText = await result.Content.ReadAsStringAsync();

                    retVal = JsonConvert.DeserializeObject<Campaign>(responseText);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem " + ex.Message);
            }
            return retVal;
        }

        public async Task<Dashboard> GetDashboard(int playerid)
        {
            Dashboard retVal = null;
            try
            {
                if (Settings.AuthTokenExpire < DateTime.Now)
                {
                    await GetAuthToken();
                }
                var url = _baseUrl + "Dashboard/GetGames/?playerid=" + playerid.ToString();
                using (var httpClient = CreateClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Settings.AuthToken);

                    HttpResponseMessage result = await httpClient.GetAsync(url);
                    if (result.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    {
                        await GetAuthToken();
                        httpClient.DefaultRequestHeaders.Clear();
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Settings.AuthToken);
                        result = await httpClient.GetAsync(url);
                    }
                    string responseText = await result.Content.ReadAsStringAsync();

                    retVal = JsonConvert.DeserializeObject<Dashboard>(responseText);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem " + ex.Message);
            }
            return retVal;
        }

        public async Task<VGame> JoinNewGame(int playerid)
        {
            VGame retVal = null;
            try
            {
                if (Settings.AuthTokenExpire < DateTime.Now)
                {
                    await GetAuthToken();
                }
                var url = _baseUrl + "Game/JoinNewGame/?playerid=" + playerid.ToString();
                using (var httpClient = CreateClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Settings.AuthToken);

                    HttpResponseMessage result = await httpClient.GetAsync(url);
                    if (result.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    {
                        await GetAuthToken();
                        httpClient.DefaultRequestHeaders.Clear();
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Settings.AuthToken);
                        result = await httpClient.GetAsync(url);
                    }
                    string responseText = await result.Content.ReadAsStringAsync();

                    retVal = JsonConvert.DeserializeObject<VGame>(responseText);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem " + ex.Message);
            }
            return retVal;
        }

        public async Task<VGame> GetGame(int vgameid, int playerid)
        {
            VGame retVal = null;
            try
            {
                if (Settings.AuthTokenExpire < DateTime.Now)
                {
                    await GetAuthToken();
                }
                var url = _baseUrl + string.Format("Game/GetGame/?vgameid={0}&playerid={1}", vgameid, playerid);
                using (var httpClient = CreateClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Settings.AuthToken);

                    HttpResponseMessage result = await httpClient.GetAsync(url);
                    if (result.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    {
                        await GetAuthToken();
                        httpClient.DefaultRequestHeaders.Clear();
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Settings.AuthToken);
                        result = await httpClient.GetAsync(url);
                    }
                    string responseText = await result.Content.ReadAsStringAsync();

                    retVal = JsonConvert.DeserializeObject<VGame>(responseText);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem " + ex.Message);
            }
            return retVal;
        }

        public async Task<VGame> StartGame(int vgameid, int playerid)
        {
            VGame retVal = null;
            try
            {
                if (Settings.AuthTokenExpire < DateTime.Now)
                {
                    await GetAuthToken();
                }
                var url = _baseUrl + string.Format("Game/StartGame/?vgameid={0}&playerid={1}", vgameid, playerid);
                using (var httpClient = CreateClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Settings.AuthToken);

                    HttpResponseMessage result = await httpClient.GetAsync(url);
                    if (result.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    {
                        await GetAuthToken();
                        httpClient.DefaultRequestHeaders.Clear();
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Settings.AuthToken);
                        result = await httpClient.GetAsync(url);
                    }
                    string responseText = await result.Content.ReadAsStringAsync();

                    retVal = JsonConvert.DeserializeObject<VGame>(responseText);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem " + ex.Message);
            }
            return retVal;
        }

        public async Task<VGame> PostSelectedCategories(VGameSelectedCategories inObj)
        {
            VGame retVal = null;
            try
            {
                if (Settings.AuthTokenExpire < DateTime.Now)
                {
                    await GetAuthToken();
                }
                var url = _baseUrl + "Game/PostSelectedCategories";
                using (var httpClient = CreateClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Settings.AuthToken);

                    var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(inObj));
                    var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

                    var result = await httpClient.PostAsync(url, httpContent);
                    if (result.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    {
                        await GetAuthToken();
                        httpClient.DefaultRequestHeaders.Clear();
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Settings.AuthToken);
                        result = await httpClient.PostAsync(url, httpContent);
                    }
                    var responseText = await result.Content.ReadAsStringAsync();

                    retVal = JsonConvert.DeserializeObject<VGame>(responseText);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem " + ex.Message);
            }
            return retVal;
        }



        public async Task<NewCampaignStageReturn> PostNextCampaignCategory(NewCampaignStageInfo inObj)
        {
            NewCampaignStageReturn retVal = null;
            try
            {
                if (Settings.AuthTokenExpire < DateTime.Now)
                {
                    await GetAuthToken();
                }
                var url = _baseUrl + "Campaign/PostNextStage";
                using (var httpClient = CreateClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Settings.AuthToken);

                    var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(inObj));
                    var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

                    var result = await httpClient.PostAsync(url, httpContent);
                    if (result.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    {
                        await GetAuthToken();
                        httpClient.DefaultRequestHeaders.Clear();
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Settings.AuthToken);
                        result = await httpClient.PostAsync(url, httpContent);
                    }
                    var responseText = await result.Content.ReadAsStringAsync();

                    retVal = JsonConvert.DeserializeObject<NewCampaignStageReturn>(responseText);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem " + ex.Message);
            }
            return retVal;
        }



        /// old

        public async Task<Dashboard> Old_GetDashboardAsync()
        {
            int playerId = App.PlayerObj.PlayerId;
            Dashboard dashboard = null;
            try
            {
                //TODO: Check network connection
                //if (await CheckNetworkConnection())
                //{
                var url = _baseUrl + "Dashboard/" + playerId;
                using (var httpClient = CreateClient())
                {
                    var result = await httpClient.GetAsync(url);
                    var responseText = await result.Content.ReadAsStringAsync();
                    //Serialize the json object to our c# classes
                    dashboard = JsonConvert.DeserializeObject<Dashboard>(responseText);
                }
                //}
            }
            catch (Exception ex)
            {
                //In case something we have a problem...
                Debug.WriteLine("Whooops! " + ex.Message);
            }
            return dashboard;
        }

        public async Task<Game> Old_GetGameAsync(int gameId)
        {
            int playerId = App.PlayerObj.PlayerId;
            Game newGame = null;
            try
            {
                var url = _baseUrl + string.Format("Game?id={0}&playerid={1}", gameId, playerId);
                using (var httpClient = CreateClient())
                {
                    var result = await httpClient.GetAsync(url);
                    var responseText = await result.Content.ReadAsStringAsync();

                    newGame = JsonConvert.DeserializeObject<Game>(responseText);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem " + ex.Message);
            }
            return newGame;
        }

        public async Task<string> Old_PostGamePlayerUpdate(GamePlayer GP)
        {
            string retVal;

            var url = _baseUrl + "GamePlayer";
            using (var httpClient = CreateClient())
            {
                var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(GP));
                var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

                var result = await httpClient.PostAsync(url, httpContent);
                var responseText = await result.Content.ReadAsStringAsync();

                retVal = responseText;
            }

            return retVal;
        }

        public async Task<string> Old_PostGamePlayerQuestionResults(GamePlayerQuestionResult GPQR)
        {
            string retVal;

            var url = _baseUrl + "GamePlayerQuestionResult";
            using (var httpClient = CreateClient())
            {
                var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(GPQR));
                var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

                var result = await httpClient.PostAsync(url, httpContent);
                var responseText = await result.Content.ReadAsStringAsync();

                retVal = responseText;
            }

            return retVal;
        }

        public async Task<Game> Old_CreateGameAsync()
        {
            int playerId = App.PlayerObj.PlayerId;
            Game newGame = null;
            try
            {
                var url = _baseUrl + "CreateGame/" + playerId;
                using (var httpClient = CreateClient())
                {
                    var result = await httpClient.GetAsync(url);
                    var responseText = await result.Content.ReadAsStringAsync();

                    newGame = JsonConvert.DeserializeObject<Game>(responseText);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem " + ex.Message);
            }
            return newGame;
        }

        public async Task<Game> Old_JoinGameAsync(int gameId)
        {
            int playerId = App.PlayerObj.PlayerId;
            Game newGame = null;
            try
            {
                var url = _baseUrl + string.Format("JoinGame?id={0}&playerid={1}", gameId, playerId);
                using (var httpClient = CreateClient())
                {
                    var result = await httpClient.GetAsync(url);
                    var responseText = await result.Content.ReadAsStringAsync();

                    newGame = JsonConvert.DeserializeObject<Game>(responseText);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem " + ex.Message);
            }
            return newGame;
        }

    }

}
