
using Moq;
using Microsoft.Extensions.Localization;
using App.Common.Resources;

namespace WebApi.Unit.Tests.MockStringLocalizer
{
    public class StringLocalizerMock
    {
        public Mock<IStringLocalizer<GlobalResource>> _globalLocalizer { get; set; }
        
        public StringLocalizerMock()
        {
            _globalLocalizer = new Mock<IStringLocalizer<GlobalResource>>();
        }

        private void Setup() 
        {
        }
    }
}
