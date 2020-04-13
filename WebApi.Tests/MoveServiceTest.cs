
using BusinnesLogic.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebApi.Unit.Tests.MockRepository;
using Microsoft.Extensions.Configuration;
using WebApi.Unit.Tests.ConfigMock;
using WebApi.Unit.Tests.MockStringLocalizer;
using WebApi.Controllers;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using Common.Classes.BussinesLogic;
using System.IO;
using System;
using FluentAssertions;
using WebApi.Unit.Tests.Stubs;

namespace WebApi.Unit.Tests
{
    [TestClass]
    public class MoveServiceTest
    {
        private static MoveController _moveController;

        [ClassInitialize]
        public static void Setup (TestContext context ) {
            Mock<IStringLocalizer<App.Common.Resources.GlobalResource>> _globalLocalizer = new StringLocalizerMock()._globalLocalizer;
            Mock<IConfiguration> _configuration = new ConfigurationMock()._configuration;
            Mock<IMoveService> _moveService = new MoveServiceMock()._moveService;

            _moveController = new MoveController( _globalLocalizer.Object, _configuration.Object, _moveService.Object);
        }

        [TestMethod]
        public void TestGetMovingGrips()
        {
            //Preparación
            var filePath = Path.Combine(
                Directory.GetCurrentDirectory(), "Files");

            List<MoveDetailDTO> listMoveDetailDTO = new List<MoveDetailDTO>();
            using (var reader = new StreamReader(Path.Combine(filePath, "lazy_loading_example_input.txt")))
            {
                int index = 1;
                while (reader.Peek() >= 0)
                {
                    var moveDetail =
                        new MoveDetailDTO
                        {
                            IdMove = 0,
                            Value = Int32.Parse(reader.ReadLine()),
                            Position = index
                        };
                    listMoveDetailDTO.Add(moveDetail);
                    index++;
                }
            }
            //Ejecución
            var result = _moveController.GetMovingGrips(listMoveDetailDTO);
            //Aserción
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(5);
            result.Should().Equals(MoveStub.daysDetails);
        }
    }
 }


