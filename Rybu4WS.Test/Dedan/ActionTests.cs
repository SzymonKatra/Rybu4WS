using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Rybu4WS;
using Xunit;

namespace Rybu4WS.Test.Dedan
{
    public class ActionTests
    {
        [Fact]
        public void ToDedan()
        {
            var action = new Rybu4WS.Dedan.Action()
            {
                ServerTypeName = "MyServer",
                ServiceName = "SomeSvc",
                PreState = "Pre",
                PostState = "Post",
                OutMessageServerTypeName = "OutServer",
                OutMessageServiceName = "OutSvc"
            };

            var result = action.ToDedan(2);

            result.Should().Be("<j=1..2>{A[j].MyServer.SomeSvc, MyServer.Pre} -> {A[j].OutServer.OutSvc, MyServer.Post}");
        }
    }
}
