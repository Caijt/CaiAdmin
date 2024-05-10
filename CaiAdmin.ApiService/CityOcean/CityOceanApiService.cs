using CaiAdmin.ApiService.CityOcean.Dto;
using CaiAdmin.Common;
using CaiAdmin.Common.CacheHelper;
using CaiAdmin.Option;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;

namespace CaiAdmin.ApiService.CityOcean
{
    /// <summary>
    /// 鹏城海API接口服务
    /// </summary>
    public class CityOceanApiService
    {
        private readonly HttpClient _httpClient;
        private readonly CityoceanOption _cityoceanOption;
        private readonly ICacheHelper _cacheHelper;
        public CityOceanApiService(HttpClient httpClient, IOptionsSnapshot<CityoceanOption> option, ICacheHelper cacheHelper)
        {
            _httpClient = httpClient;
            _cityoceanOption = option.Value;
            _cacheHelper = cacheHelper;
            httpClient.BaseAddress = new Uri(_cityoceanOption.ApiUrl);
            httpClient.DefaultRequestHeaders.Add(".AspNetCore.Culture", "zh-Hans");
            httpClient.DefaultRequestHeaders.Add("Accept-Language", "zh-CN");
            var token = _cacheHelper.Get<string>("CityOcean:AccessToken");
            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Add("authorization", "bearer " + token);
            }
        }
        /// <summary>
        /// 同步发票状态
        /// </summary>
        /// <param name="invoiceIds"></param>
        /// <returns></returns>
        public async Task SyncInvoiceStatusFromNuoNuo(List<Guid> invoiceIds)
        {
            await PostAsync("/Fam/Invoice/SyncInvoiceStatusFromNuoNuo", invoiceIds);
        }

        /// <summary>
        /// 同步客户到ES
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task SyncCustomerToEs(Guid customerId)
        {
            var input = new SyncCustomerForEs
            {
                Id = customerId
            };
            await PostAsync("CRM/EsQuery/SyncCrmCustomers", input);
        }

        /// <summary>
        /// 同步客户审批到ES
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task SyncCustomerExamineToEs(Guid customerExamineId)
        {
            var input = new SyncCustomerForEs
            {
                Id = customerExamineId
            };
            await PostAsync("CRM/EsQuery/SyncCrmCustomerExamines", input);
        }

        /// <summary>
        /// 同步客户到ICP
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task SyncCustomerToIcp(Guid customerId)
        {
            await GetAsync($"/CRM/Customer/SyncCustomerToIcp?customerId={customerId.ToString()}");
        }
        /// <summary>
        /// 上传附件
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<UploadResultDto> UploadFileAsync(UploadFileDto dto)
        {
            MultipartFormDataContent formData = new MultipartFormDataContent();
            formData.Add(new StringContent(dto.FileName), "FileName");
            formData.Add(new StreamContent(dto.File, (int)dto.File.Length), "File", dto.FileName);
            var res = await _httpClient.PostAsync("/Storage/File/Upload", formData);
            return await HandleResultAsync<UploadResultDto>(res);
        }

        /// <summary>
        /// 合并客户
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<MergeCustomerDto> MergeCustomer(MergeCustomerDto dto)
        {
            return await PostAsync<MergeCustomerDto, MergeCustomerDto>("CRM/Customer/MergeCustomer", dto);
        }

        /// <summary>
        /// 同步工作流数据到ES
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task SyncWorkflowToEs(List<string> workflowNos)
        {
            var input = new
            {
                Nos = workflowNos
            };
            await PostAsync("Essc/Workflow/SyncByNos", input, true);
        }

        /// <summary>
        /// 同步币种账单数据到ES
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task SyncCurrencyBillToEs(List<string> nos)
        {
            var input = new
            {
                Nos = nos
            };
            await PostAsync("Essc/CurrencyBill/SyncByNos", input, true);
        }

        /// <summary>
        /// 同步提单数据到ES
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task SyncReleaseBillOfLadingsToEs(ElasticSearchDataChangedEventBase input)
        {
            await PostAsync("Fam/EsQuery/SyncReleaseBillOfLadings", input);
        }



        /// <summary>
        /// 同步明细账单数据到ES
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task SyncChargeBillToEs(List<string> nos)
        {
            var input = new
            {
                Nos = nos
            };
            await PostAsync("Essc/ChargeBill/SyncByNos", input, true);
        }

        /// <summary>
        /// 同步核销单数据到ES
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task SyncCheckAmountToEs(List<string> nos)
        {
            var input = new
            {
                Nos = nos
            };
            await PostAsync("Essc/Check/SyncByNos", input, true);
        }

        /// <summary>
        /// 诺诺开票
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task RequestInvoiceToNuonuoByJob(List<Guid> ids)
        {
            await PostAsync("fam/invoice/RequestInvoiceToNuonuoByJob", ids);
        }

        /// <summary>
        /// 发送放单邮件
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<List<ReleaseBillOfLadingEmailTemplateResult>> SendReleaseOrderEmails(List<ReleaseBillOfLadingWithEmailTemplateDto> input)
        {
            return await PostAsync<List<ReleaseBillOfLadingWithEmailTemplateDto>, List<ReleaseBillOfLadingEmailTemplateResult>>("Fam/ReleaseBills/SendReleaseEmails", input);
        }


        /// <summary>
        /// 获取登录Token
        /// </summary>
        /// <returns></returns>
        public async Task<TokenResultDto> GetTokenAsync()
        {
            var res = await _httpClient.PostAsync("sso/connect/token", new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type","password"},
                { "scope","PlatformApi  offline_access ids4-api" },
                { "username",_cityoceanOption.LoginUser },
                { "password",_cityoceanOption.LoginPassword },
                { "client_id","cityOcean" },
                { "client_secret","282F4E3E-AD56-4FE1-BAF3-FE99BBC11AD2" },
                { "platform","platform" },
            }));
            if (!res.IsSuccessStatusCode)
            {
                var error = await res.Content.ReadAsAsync<TokenErrorDto>();
                throw new ApiFailException(ApiFailCode.API_FAIL, $"{error.error_description}");
            }
            var result = await res.Content.ReadAsAsync<TokenResultDto>();
            _cacheHelper.Set("CityOcean:AccessToken", result.access_token);
            return result;
        }


        private async Task GetAsync(string url)
        {
            await GetAsync<object, object>(url, null);
        }
        private async Task GetAsync<TData>(string url, TData data)
        {
            await GetAsync<TData, object>(url, data);
        }
        private async Task<TResult> GetAsync<TResult>(string url)
        {
            return await GetAsync<object, TResult>(url, null);
        }

        private async Task<TResult> GetAsync<TData, TResult>(string url, TData data)
        {
            var res = await _httpClient.GetAsync(url);
            return await HandleResultAsync<TResult>(res);
        }

        private async Task PostAsync<TData>(string url, TData data, bool isComs = false)
        {
            await PostAsync<TData, object>(url, data, isComs);
            //var res = await _httpClient.PostAsJsonAsync<TData>(url, data);
            //await HandleResult<object>(res);
        }

        private async Task<TResult> PostAsync<TData, TResult>(string url, TData data, bool isComs = false)
        {
            var res = await _httpClient.PostAsJsonAsync<TData>(url, data);
            return await HandleResultAsync<TResult>(res, isComs);
        }

        private async Task<T> HandleResultAsync<T>(HttpResponseMessage res, bool isComs = false)
        {
            if (!res.IsSuccessStatusCode)
            {
                var apiErrorResult = await res.Content.ReadAsAsync<ApiResult>();
                if (apiErrorResult != null)
                {
                    //登录失效，重新获取token后重新请求接口
                    if (apiErrorResult.Error.Code == 0 && (apiErrorResult.Error.Message == "Current user did not login to the application!" || apiErrorResult.Error.Message == "当前用户没有登录到系统！"))
                    {
                        var token = await GetTokenAsync();
                        if (_httpClient.DefaultRequestHeaders.Contains("authorization"))
                        {
                            _httpClient.DefaultRequestHeaders.Remove("authorization");
                        }
                        _httpClient.DefaultRequestHeaders.Add("authorization", "bearer " + token.access_token);
                        //var res2 = await _httpClient.SendAsync(res.RequestMessage);
                        var reMessage = new HttpRequestMessage()
                        {
                            RequestUri = res.RequestMessage.RequestUri,
                            Content = res.RequestMessage.Content,
                            Method = res.RequestMessage.Method
                        };
                        foreach (var item in res.RequestMessage.Headers)
                        {
                            reMessage.Headers.Add(item.Key, item.Value);
                        }
                        var res2 = await _httpClient.SendAsync(reMessage);
                        return await this.HandleResultAsync<T>(res2);
                    }
                    else
                    {
                        throw new ApiFailException(ApiFailCode.API_FAIL, $"{apiErrorResult.Error.Message}");
                    }
                }
                var errorContent = await res.Content?.ReadAsStringAsync();
                throw new ApiFailException(ApiFailCode.API_FAIL, $"[{(int)res.StatusCode}]{res.StatusCode.ToString()}{errorContent}");
            }
            if (isComs)
            {
                var result = await res.Content.ReadAsAsync<T>();
                //if (apiResult?.Error != null)
                //{
                //    throw new ApiFailException(ApiFailCode.API_FAIL, apiResult.Error.Message);
                //}
                return result;
            }
            else
            {
                var apiResult = await res.Content.ReadAsAsync<ApiResult<T>>();
                if (apiResult?.Error != null)
                {
                    throw new ApiFailException(ApiFailCode.API_FAIL, apiResult.Error.Message);
                }
                return apiResult.Result;
            }

        }
    }
}
