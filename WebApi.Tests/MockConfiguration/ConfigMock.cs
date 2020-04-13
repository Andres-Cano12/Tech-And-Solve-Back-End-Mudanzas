
using BusinnesLogic.Services;
using Common.Classes.BussinesLogic;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using WebApi.Unit.Tests.Stubs;
using Microsoft.Extensions.Configuration;

namespace WebApi.Unit.Tests.ConfigMock
{
    public class ConfigurationMock
    {
        public Mock<IConfiguration> _configuration { get; set; }

        public ConfigurationMock()
        {
            _configuration = new Mock<IConfiguration>();
        }

        private void Setup() 
        {
        }
    }
}
