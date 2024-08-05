using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using PactNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderSvc_Consumer;
using System.Net.Http.Json;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;

namespace ConsumerTesting
{
    public class DiscountSvcMock : IDisposable
    {
        private readonly IPactBuilder _pactBuilder;
        private readonly int _servicePort = 7090;
        private bool _disposed = false;
        public IMockProviderService MockProviderService { get; }
        public string ServiceUri => $"http://localhost:{_servicePort}";

        public DiscountSvcMock()
        {
            var pactConfig = new PactConfig
            {
                SpecificationVersion = "2.0.0",
                PactDir = @"C:\Users\Dell\AppData\Local\Temp\pact",
                LogDir = @"C:\Users\Dell\AppData\Local\Temp\pact\logs"
            };

            _pactBuilder = new PactBuilder(pactConfig)
            .ServiceConsumer("Orders")
            .HasPactWith("Discounts");

            MockProviderService = _pactBuilder.MockService(_servicePort,
            new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            });

        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _pactBuilder.Build();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }   
}
