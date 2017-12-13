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
        static string _baseUrl { get { return "https://triviatraverseapi20171207014847.azurewebsites.net/api/"; } }
        //static string _baseUrl { get { return "http://192.168.0.5:57579/api/"; } }

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

        public async Task<bool> CheckEmailExists(string email)
        {
            Debug.WriteLine("Start CheckEmailExists");
            bool retVal = true;
            try
            {
                var url = _baseUrl + string.Format("Account/CheckEmailExists/?email={0}", email);
                using (var httpClient = CreateClient())
                {
                    var result = await httpClient.GetAsync(url);
                    var responseText = await result.Content.ReadAsStringAsync();
                    retVal = bool.Parse(responseText);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem " + ex.Message);
            }

            Debug.WriteLine("End CheckEmailExists");
            return retVal;
        }

        public async Task<bool> CheckUserNameExists(string username)
        {
            Debug.WriteLine("Start CheckUserNameExists");
            bool retVal = true;
            try
            {
                var url = _baseUrl + string.Format("Account/CheckUserNameExists/?username={0}", username);
                using (var httpClient = CreateClient())
                {
                    var result = await httpClient.GetAsync(url);
                    var responseText = await result.Content.ReadAsStringAsync();
                    retVal = bool.Parse(responseText);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem " + ex.Message);
            }

            Debug.WriteLine("End CheckUserNameExists");
            return retVal;
        }

        public async Task<Player> NewAccountAsync(Player inObj)
        {
            Debug.WriteLine("Start NewAccountAsync");
            Player player = null;
            try
            {
                var url = _baseUrl + String.Format("Start/NewAccount");
                using (var httpClient = CreateClient())
                {
                    var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(inObj));
                    var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

                    var result = await httpClient.PostAsync(url, httpContent);
                    string responseText = await result.Content.ReadAsStringAsync();
                    player = JsonConvert.DeserializeObject<Player>(responseText);
                }
                Debug.WriteLine("Account Created");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem " + ex.Message);
            }

            Debug.WriteLine("End NewAccountAsync");
            return player;
        }

        public async Task<string> UpdateAccountAsync(Player inObj)
        {
            Debug.WriteLine("Start UpdateAccountAsync");
            //inObj.LastStepUpdate = inObj.LastStepUpdate.ToUniversalTime();
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
            Debug.WriteLine("End UpdateAccountAsync");
            return retVal;
        }


        public async Task<GameSection> GetTutorialAsync(int playerId)
        {
            Debug.WriteLine("Start GetTutorialAsync");
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
            Debug.WriteLine("End GetTutorialAsync");
            return tutorial;
        }

        public async Task<GameSection> GetSectionAsync(int sectionId, int playerId, int playerCampaignId, bool retry)
        {
            Debug.WriteLine("Start GetSectionAsync");
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
            Debug.WriteLine("End GetSectionAsync");
            return section;
        }


        public async Task<Player> GetAccountAsync(string emailaddress, string password)
        {
            Debug.WriteLine("Start GetAccountAsync");
            Player retVal = null;
            try
            {
                System.Diagnostics.Debug.WriteLine("getting account");
                string url = _baseUrl + string.Format("Account/GetAccount?email={0}&password={1}", emailaddress, password);
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
            Debug.WriteLine("End GetAccountAsync");
            return retVal;
        }

        public async Task<string> UpdateTutorialMessageStatusAsync(TutorialMessagesStatus inObj)
        {
            Debug.WriteLine("Start UpdateTutorialMessageStatusAsync");
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
            Debug.WriteLine("End UpdateTutorialMessageStatusAsync");
            return retVal;
        }

        public async Task<VGame> UpdateVGamePlayerAsync(VGamePlayerUpdate inObj)
        {
            Debug.WriteLine("Start UpdateVGamePlayerAsync");
            //inObj.LastStepUpdate = inObj.LastStepUpdate.ToUniversalTime();
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
            Debug.WriteLine("End UpdateVGameAsync");
            return retVal;
        }

        public async Task<string> PostCampaignQuestionResults(PlayerQuestionResult inObj)
        {
            Debug.WriteLine("Start PostCampaignQuestionResults");
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
            Debug.WriteLine("End PostCampaignQuestionResults");
            return retVal;
        }

        public async Task<string> PostVGameQuestionResults(PlayerVGameQuestionResult inObj)
        {
            Debug.WriteLine("Start PostVGameQuestionResults");
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
            Debug.WriteLine("End PostVGameQuestionResults");
            return retVal;
        }

        public async Task<Campaign> GetCampaign(int playerid)
        {
            Debug.WriteLine("Start GetCampaign");
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
            Debug.WriteLine("End GetCampaign");
            return retVal;
        }

        public async Task<Dashboard> GetDashboard(int playerid)
        {
            Debug.WriteLine("Start GetDashboard");
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
            Debug.WriteLine("End GetDashboard");
            return retVal;
        }

        public async Task<VGame> JoinNewGame(int playerid, string gameType, string gameCap, bool gameAuto)
        {
            Debug.WriteLine("Start JoinNewGame");
            VGame retVal = null;
            try
            {
                if (Settings.AuthTokenExpire < DateTime.Now)
                {
                    await GetAuthToken();
                }
                var url = _baseUrl + String.Format("Game/JoinNewGame/?playerid={0}&gameType={1}&gameCap={2}&gameAuto={3}", playerid.ToString(), gameType, gameCap, gameAuto);
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
            Debug.WriteLine("End JoinNewGame");
            return retVal;
        }

        public async Task<VGame> GetGame(int vgameid, int playerid)
        {
            Debug.WriteLine("Start GetGame");
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
            Debug.WriteLine("End GetGame");
            return retVal;
        }

        public async Task<VGame> StartGame(int vgameid, int playerid)
        {
            Debug.WriteLine("Start StartGame");
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
            Debug.WriteLine("End StartGame");
            return retVal;
        }

        public async Task<VGame> PostSelectedCategories(VGameSelectedCategories inObj)
        {
            Debug.WriteLine("Start PostSelectedCategories");
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
            Debug.WriteLine("End PostSelectedCategories");
            return retVal;
        }
        
        public async Task<NewCampaignStageReturn> PostNextCampaignCategory(NewCampaignStageInfo inObj)
        {
            Debug.WriteLine("Start PostNextCampaignCategory");
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
            Debug.WriteLine("End PostNextCampaignCategory");
            return retVal;
        }
    }
}
