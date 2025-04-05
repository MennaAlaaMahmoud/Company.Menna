using Twilio.Rest.Api.V2010.Account;

namespace Company.Menna.PL.Helpers
{
    public interface ITwilioService
    {
        public MessageResource SendSms(Sms sms);
        
    }
}
