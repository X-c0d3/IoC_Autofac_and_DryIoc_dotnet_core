using Autofac.Extras.Moq;
using IoC.Services;
using System;
using Xunit;
using Moq;
using IoC.Interfaces;
using System.Collections.Generic;
using IoC.Models;
using DryIoc;
using Arg = DryIoc.Arg;
using NSubstitute;
using System.Collections.Concurrent;

namespace IoC.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test_with_AutoMock()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var moqResponse = new List<Hotel>
                {
                    new Hotel {
                        HotelId = 1,
                        HotelName = "My hotel name",
                        IsActive = true
                    }
                };

                mock.Mock<IDataServices>()
                    .Setup(srv => srv.GetAllData())
                    .Returns(moqResponse);

                var h = mock.Create<HotelServices>();
                var res = h.GetHotelAll();

                Assert.Single(res);
                Assert.Equal("My hotel name", h.GetHotelById(1).HotelName);
            }
        }

        [Fact]
        public void Test_with_Moq()
        {
            var mockRepo = new Mock<IDataServices>();
            var moqResponse = new List<Hotel>
                {
                    new Hotel {
                        HotelId = 1,
                        HotelName = "My hotel name",
                        IsActive = true
                    }
                };
            mockRepo.Setup(x => x.GetAllData())
                .Returns(moqResponse);

            var h = new HotelServices(mockRepo.Object);
            var res = h.GetHotelAll();

            Assert.Single(res);
            Assert.Equal("My hotel name", h.GetHotelById(1).HotelName);
        }




        [Fact]
        public void Test_with_DryIocMoq()
        {

            var moqResponse = new List<Hotel>
                {
                    new Hotel {
                        HotelId = 1,
                        HotelName = "My hotel name",
                        IsActive = true
                    }
                };
            // ################################################33
            var mockRepo = new Mock<IDataServices>();
            mockRepo.Setup(x => x.GetAllData())
                .Returns(moqResponse);

            var h = new HotelServices(mockRepo.Object);
            var res = h.GetHotelAll();
            Assert.Single(res);
            Assert.Equal("My hotel name", h.GetHotelById(1).HotelName);



            var container = new Container();
            container.Register<IDataServices, DataServices>();
            container.Register<IHotelServices, HotelServices>();
            var sub = container.Resolve<IHotelServices>();
            var resx = sub.GetHotelAll();

    

 
        }
    }
}
