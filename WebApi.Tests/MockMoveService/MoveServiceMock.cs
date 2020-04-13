using BusinnesLogic.Services;
using Common.Classes.BussinesLogic;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using WebApi.Unit.Tests.Stubs;

namespace WebApi.Unit.Tests.MockRepository
{
    public class MoveServiceMock
    {
        public Mock<IMoveService> _moveService { get; set; }

        public MoveServiceMock()
        {
            _moveService = new Mock<IMoveService>();
            Setup();
        }

        private void Setup() 
        {
            _moveService.Setup((x) => x.GetMovingGrips(It.IsAny<List<MoveDetailDTO>>())).Returns(MoveStub.daysDetails);
        }
    }
}
