using Meeting_App.Models.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Meeting_App.Service
{
    public class AdminWingService
    {
        private HttpClient _httpClient;


        public AdminWingService(string url)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(url);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.Timeout= TimeSpan.FromMinutes(10);
        }

        public async Task<string> GetpendancyData(string url, ParameterDTO parameterDTO)
        {
            try
            {
                var body = JsonConvert.SerializeObject(new { From = parameterDTO.From, To = parameterDTO.To });

                var buffer = Encoding.UTF8.GetBytes(body);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(url, byteContent).ConfigureAwait(false);

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

        public async Task<string> GetEmployeesOnLeave(string url, PaginationDTO parameterDTO)
        {
            try
            {
                var body = JsonConvert.SerializeObject(new { Skip = parameterDTO.Skip, PageSize = parameterDTO.pagesize ,SignedBy=parameterDTO.SignedBy });

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

        public async Task<string> GetEmpOnLeaveSum(string url)
        {
            try
            {
                //var body = JsonConvert.SerializeObject(new { Skip = parameterDTO.Skip, PageSize = parameterDTO.pagesize });

                //var buffer = Encoding.UTF8.GetBytes(body);
                //var byteContent = new ByteArrayContent(buffer);
                //byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

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


        public async Task<string> GetEmpLeaveExpSum(string url)
        {
            try
            {
                //var body = JsonConvert.SerializeObject(new { Skip = parameterDTO.Skip, PageSize = parameterDTO.pagesize });

                //var buffer = Encoding.UTF8.GetBytes(body);
                //var byteContent = new ByteArrayContent(buffer);
                //byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

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

        public async Task<string> GetEmployeesLeaveExpired(string url, PaginationDTO parameterDTO)
        {
            try
            {
                var body = JsonConvert.SerializeObject(new { Skip = parameterDTO.Skip, PageSize = parameterDTO.pagesize ,SignedBy=parameterDTO.SignedBy });

                var buffer = Encoding.UTF8.GetBytes(body);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(url,byteContent);

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
        public async Task<string> GetLeavesExpired(string url)
        {
            try
            {
                //var body = JsonConvert.SerializeObject(new { Skip = parameterDTO.Skip, PageSize = parameterDTO.pagesize });

                //var buffer = Encoding.UTF8.GetBytes(body);
                //var byteContent = new ByteArrayContent(buffer);
                //byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

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
        public async Task<string> AwaitingPostingSum(string url)
        {
            try
            {
                //var body = JsonConvert.SerializeObject(new { Skip = parameterDTO.Skip, PageSize = parameterDTO.pagesize });

                //var buffer = Encoding.UTF8.GetBytes(body);
                //var byteContent = new ByteArrayContent(buffer);
                //byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

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
        public async Task<string> GetAwaitingPostingApps(string url, PaginationDTO parameterDTO)
        {
            try
            {
                var body = JsonConvert.SerializeObject(new { Skip = parameterDTO.Skip, PageSize = parameterDTO.pagesize , OfficerId=parameterDTO.OfficerId });

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


        public async Task<string> GetVpByHF(string url)
        {
            try
            {
                //var body = JsonConvert.SerializeObject(new { Skip = parameterDTO.Skip, PageSize = parameterDTO.pagesize });

                //var buffer = Encoding.UTF8.GetBytes(body);
                //var byteContent = new ByteArrayContent(buffer);
                //byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

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

        public async Task<string> GetVp(string url)
        {
            try
            {
                //var body = JsonConvert.SerializeObject(new { Skip = parameterDTO.Skip, PageSize = parameterDTO.pagesize });

                //var buffer = Encoding.UTF8.GetBytes(body);
                //var byteContent = new ByteArrayContent(buffer);
                //byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

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
        public async Task<string> GetCrrSummary(string url, ParameterDTO parameterDTO)
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

        public async Task<string> VPReport(string url)
        {
            try
            {
                //var body = JsonConvert.SerializeObject(new { From = parameterDTO.From, To = parameterDTO.To });

                //var buffer = Encoding.UTF8.;
                //var byteContent = new ByteArrayContent(buffer);
                //byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await _httpClient.GetAsync(url);

                await Task.Delay(300);

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

        public async Task<string> GetProcurementCountByType(string url)
        {
            try
            {
                //var body = JsonConvert.SerializeObject(new { From = parameterDTO.From, To = parameterDTO.To });

                //var buffer = Encoding.UTF8.;
                //var byteContent = new ByteArrayContent(buffer);
                //byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await _httpClient.GetAsync(url);

                await Task.Delay(300);

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

        public async Task<string> GetDistrictWiseProcurementCount(string url)
        {
            try
            {
                //var body = JsonConvert.SerializeObject(new { From = parameterDTO.From, To = parameterDTO.To });

                //var buffer = Encoding.UTF8.;
                //var byteContent = new ByteArrayContent(buffer);
                //byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await _httpClient.GetAsync(url);

                await Task.Delay(300);

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

        public async Task<string> GetProcurementDetail(string url )
        {
            try
            {
                //var body = JsonConvert.SerializeObject(new { type = procurementDetailDTO.type, District = procurementDetailDTO.District, Perfomatytpe = procurementDetailDTO.Perfomatytpe });

                //var buffer = Encoding.UTF8.GetBytes(body);
                //var byteContent = new ByteArrayContent(buffer);
                //byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await _httpClient.GetAsync(url);

                await Task.Delay(300);

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
        public async Task<string> GetAdpSchemes(string url)
        {
            try
            {
                //var body = JsonConvert.SerializeObject(new { From = parameterDTO.From, To = parameterDTO.To });

                //var buffer = Encoding.UTF8.;
                //var byteContent = new ByteArrayContent(buffer);
                //byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await _httpClient.GetAsync(url);

                await Task.Delay(300);

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


        public async Task<string> GetHCReport(string url)
        {
            try
            {
                //var body = JsonConvert.SerializeObject(new { From = parameterDTO.From, To = parameterDTO.To });

                //var buffer = Encoding.UTF8.;
                //var byteContent = new ByteArrayContent(buffer);
                //byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await _httpClient.GetAsync(url);

                await Task.Delay(300);

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

        public async Task<string> GetAdpSchemesCount(string url)
        {
            try
            {
                //var body = JsonConvert.SerializeObject(new { From = parameterDTO.From, To = parameterDTO.To });

                //var buffer = Encoding.UTF8.;
                //var byteContent = new ByteArrayContent(buffer);
                //byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await _httpClient.GetAsync(url);

                await Task.Delay(300);

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

        public async Task<string> GetHepatitasPatientCounts(string url)
        {
            try
            {
               

                string basicHash = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1")
                    .GetBytes("admin:admin123"));

                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {basicHash}");

                HttpResponseMessage response = await _httpClient.GetAsync(url);

                await Task.Delay(300);

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

        public async Task<string> GetDRSDailyLabCount(string url)
        {
            try
            {
                //var body = JsonConvert.SerializeObject(new { From = parameterDTO.From, To = parameterDTO.To });

                //var buffer = Encoding.UTF8.;
                //var byteContent = new ByteArrayContent(buffer);
                //byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await _httpClient.GetAsync(url);

                await Task.Delay(300);

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

        public async Task<string> GetDRSDailyPatientCount(string url)
        {
            try
            {
                //var body = JsonConvert.SerializeObject(new { From = parameterDTO.From, To = parameterDTO.To });

                //var buffer = Encoding.UTF8.;
                //var byteContent = new ByteArrayContent(buffer);
                //byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await _httpClient.GetAsync(url);

                await Task.Delay(300);

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


        public async Task<string> GetIRMNCHPatientsDistrictWise(string url, ParameterDTO parameterDTO)
        {
            try
            {
                //var body = JsonConvert.SerializeObject(new { From = parameterDTO.From, To = parameterDTO.To });

             //   var buffer = Encoding.UTF8.GetBytes(body);
               // var byteContent = new ByteArrayContent(buffer);
              //  byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                if(parameterDTO.From==null)
                {
                    parameterDTO.From = DateTime.Now.Date;
                }
                if (parameterDTO.To == null)
                {
                    parameterDTO.To = DateTime.Now.Date;

                }

                HttpResponseMessage response = await _httpClient.GetAsync(url+"?startDate=" + parameterDTO.From +"&endDate=" + parameterDTO.To).ConfigureAwait(false);

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


        public async Task<string> GetNCDDailyReport(string url)
        {
            try
            {
                //var body = JsonConvert.SerializeObject(new { From = parameterDTO.From, To = parameterDTO.To });

                //var buffer = Encoding.UTF8.;
                //var byteContent = new ByteArrayContent(buffer);
                //byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await _httpClient.GetAsync(url);

                await Task.Delay(300);

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

        public async Task<string> GetLHSReportingDistWise(string url, ParameterDTO parameterDTO)
        {
            try
            {
                //var body = JsonConvert.SerializeObject(new { From = parameterDTO.From, To = parameterDTO.To });

                //   var buffer = Encoding.UTF8.GetBytes(body);
                // var byteContent = new ByteArrayContent(buffer);
                //  byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                if (parameterDTO.From == null)
                {
                    parameterDTO.From = DateTime.Now.Date;
                }
             

                HttpResponseMessage response = await _httpClient.GetAsync(url + "?date=" + parameterDTO.From).ConfigureAwait(false);

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

        public async Task<string> GetIrmnchEmrDsr(string url, ParameterDTO parameterDTO)
        {
            try
            {
                //var body = JsonConvert.SerializeObject(new { From = parameterDTO.From, To = parameterDTO.To });

                //   var buffer = Encoding.UTF8.GetBytes(body);
                // var byteContent = new ByteArrayContent(buffer);
                //  byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                if (parameterDTO.From == null)
                {
                    parameterDTO.From = DateTime.Now.Date;
                }
                if (parameterDTO.To == null)
                {
                    parameterDTO.To = DateTime.Now.Date;

                }

                HttpResponseMessage response = await _httpClient.GetAsync(url + "?fromDate=" + parameterDTO.From + "&toDate=" + parameterDTO.To).ConfigureAwait(false);

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
        public async Task<string> GetSummries(string url)
        {
            try
            {
                //var body = JsonConvert.SerializeObject(new { From = parameterDTO.From, To = parameterDTO.To });

                //   var buffer = Encoding.UTF8.GetBytes(body);
                // var byteContent = new ByteArrayContent(buffer);
                //  byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
 

                HttpResponseMessage response = await _httpClient.GetAsync(url).ConfigureAwait(false);

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

        public async Task<string> GetSummriesDetail(string url)
        {
            try
            {
                //var body = JsonConvert.SerializeObject(new { From = parameterDTO.From, To = parameterDTO.To });

                //   var buffer = Encoding.UTF8.GetBytes(body);
                // var byteContent = new ByteArrayContent(buffer);
                //  byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");


                HttpResponseMessage response = await _httpClient.GetAsync(url).ConfigureAwait(false);

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

        public async Task<string> GetSummriesAttachment(string url)
        {
            try
            {
                //var body = JsonConvert.SerializeObject(new { From = parameterDTO.From, To = parameterDTO.To });

                //   var buffer = Encoding.UTF8.GetBytes(body);
                // var byteContent = new ByteArrayContent(buffer);
                //  byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");


                HttpResponseMessage response = await _httpClient.GetAsync(url).ConfigureAwait(false);

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

        public async Task<string> GetDSRReport(string url)
        {
            try
            {
                //var body = JsonConvert.SerializeObject(new { From = parameterDTO.From, To = parameterDTO.To });

                //var buffer = Encoding.UTF8.;
                //var byteContent = new ByteArrayContent(buffer);
                //byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await _httpClient.GetAsync(url);

                await Task.Delay(300);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    return data;
                }
               
                //if (response.IsSuccessStatusCode)
                //{


                //    var data = await response.Content.ReadAsStringAsync();
                //    XmlDocument doc = new XmlDocument();
                //    doc.LoadXml(data);
                //    var json = JsonConvert.SerializeObject(doc);
                //   response.Content = new StringContent(json,Encoding.UTF8, "application/json");
                //    json = json.Replace(@"#", @"").Replace(@"?", @"").Replace(@"@",@"").Replace(@"\",@"").Replace(@"string", @"DSRCount").Replace(@"text","data");
                //    //object transactObject1 = JsonConvert.DeserializeObject(json);

                //    //string json = JsonConvert.SerializeXmlNode(data);
                //    return json;
                //}
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
