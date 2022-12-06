using Meeting_App.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Meeting_App.Service
{
    public class DHISService
    {
        private HttpClient _httpClient;
        public DHISService()
        {
            _httpClient = new HttpClient();
           
        }
       

        public async Task<string> GetDHISData(string toDay)
        {
            string url = "http://116.58.20.67:8080/api/analytics/dataValueSet.json?dimension=ou:LEVEL-1&dimension=pe:"+toDay+"&dimension=dx:DE_GROUP-wYuARIpYW4A;DE_GROUP-ktI6z0nzTsF;DE_GROUP-ikCCbsgIfhB;DE_GROUP-pnSsWLLWo3c;DE_GROUP-seTCRsYCgp6;DE_GROUP-XjNEVQCybWj;DE_GROUP-d7bZYnTH00D;DE_GROUP-vrAQSouWHN2;DE_GROUP-kdhk5jmAqxK;DE_GROUP-OwHhNzWsmtY;DE_GROUP-kusDWjDvZfM;DE_GROUP-zJ6Y3fQvw8w;DE_GROUP-RRIpwS5lPxB;&outputIdScheme=NAME";

           
            string svcCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes("prim_sec_api_user" + ":" + "DHIS2##api@@20"));

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", svcCredentials);

           var response = await _httpClient.GetAsync(url);
           var res = await response.Content.ReadAsStringAsync();

            return res;
           
        }



        }

    }

