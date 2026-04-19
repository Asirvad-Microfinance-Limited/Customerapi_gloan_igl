using APIBaseClassLibrary.V1.Models.Request;
using APIBaseClassLibrary.V1.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIBaseClassLibrary.V1.MaKash
{
    public class MaKash
    {


        public MaKashService.Service1SoapClient getClient()
        {
            AgentLoginResponse response = new AgentLoginResponse();
            MaKashService.Service1SoapClient.EndpointConfiguration endpoint = new MaKashService.Service1SoapClient.EndpointConfiguration();
            MaKashService.Service1SoapClient Client = new MaKashService.Service1SoapClient(endpoint, "https://onpay.online.manappuram.com/test_ma/Makash_service.asmx");
            return Client;
        }

        public async Task<AgentLoginResponse> AgentLogin(AgentLoginRequest request)
        {
            AgentLoginResponse response = new AgentLoginResponse();
            try
            {
                MaKashService.Service1SoapClient Client = getClient();
                string x = await Client.AGENTLOGINAsync(request.agentID, request.password);
            }catch(Exception ex)
            {

            }
            return response;
        }

        public async Task<CustomerProfileResponse> CustomerProfile(CustomerProfileRequest request)
        {
            CustomerProfileResponse response = new CustomerProfileResponse();
            try
            {
                MaKashService.Service1SoapClient Client = getClient();
                string x = await Client.GETCUSTOMERPROFILEAsync(request.AgentID, request.Mobile,request.SessionID,request.Amount);
            }
            catch (Exception ex)
            {

            }
            return response;
        }

        public async Task<WalletToUpResponse> WalletToUp(WalletToUpRequest request)
        {
            WalletToUpResponse response = new WalletToUpResponse();
            try
            {
                MaKashService.Service1SoapClient Client = getClient();
                string x = await Client.WALLETTOPUPAsync(request.AgentID, request.Password, request.Mobile, request.SessionID, request.Amount, request.RequestID, request.PaymentMode);
            }
            catch (Exception ex)
            {

            }
            return response;
        }

        public async Task<AgentLogoutResponse> AgentLogout(AgentLogoutRequest request)
        {
            AgentLogoutResponse response = new AgentLogoutResponse();
            try
            {
                MaKashService.Service1SoapClient Client = getClient();
                string x = await Client.AGENTLOGOUTAsync(request.AgentID);
            }
            catch (Exception ex)
            {

            }
            return response;
        }
    }
}
