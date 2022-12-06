using Meeting_App.Models.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Meeting_App.Service
{
    public class HttpClientIntegrated
    {
        private HttpClient _httpClient;
        

        public HttpClientIntegrated(string url) 
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(url);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> GetData(string url)
        {
            try
            {
                //GET Method  
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    return data;
                }
                else
                {
                    return "Internal server Error";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public async Task<string> GetHCPData(string url)
        {
            try
            {
                var byteArray = Encoding.ASCII.GetBytes("admin:admin123");
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                //GET Method  
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    return data;
                }
                else
                {
                    return "Internal server Error";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        
        public async Task<string> GetpendancyData(string url,ParameterDTO parameterDTO )
        {
            try
            {
                var body = JsonConvert.SerializeObject(new { From = parameterDTO.From, To = parameterDTO.To });

                var buffer = Encoding.UTF8.GetBytes(body);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(url, byteContent); 

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    return data;
                }
                else
                {
                    return "Internal server Error";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }


    }
}