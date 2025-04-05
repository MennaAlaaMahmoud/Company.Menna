using Company.Menna.PL.Settings;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Company.Menna.PL.Helpers
{
    public class TwilioService: ITwilioService
    {
        private readonly TwilioSettings _options;

        public TwilioService(IOptions<TwilioSettings> options)
        {
            _options = options.Value;
        }

        public MessageResource SendSms(Sms sms)
        {
            // Initialize Connection

            TwilioClient.Init(_options.AccountSId, _options.AuthToken);

            // build the message
            var message =  MessageResource.Create(
               body: sms.Body,
                from: new Twilio.Types.PhoneNumber(_options.PhoneNumber),
                 to : sms.PhoneNumber
            );


            return message;
            // return Message

        }
    }
}
