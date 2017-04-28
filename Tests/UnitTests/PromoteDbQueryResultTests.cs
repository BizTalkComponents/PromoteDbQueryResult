using BizTalkComponents.Utilities.DbQueryUtility.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Xml.Linq;
using Winterdom.BizTalk.PipelineTesting;

namespace BizTalkComponents.PipelineComponents.PromoteDbQueryResult.PromoteDbQueryResult.Tests.UnitTests
{
    [TestClass]
    public class PromoteDbQueryResultTests
    {
        [TestMethod]
        public void TestHappyPath()
        {
            XDocument doc =
             new XDocument(
               new XElement("Result",
                 new XElement("param1", "param1value"),
                 new XElement("param2", "param2value"),
                 new XElement("param3", "param3value")
                 )
             );

            var mock = new Mock<IDbQueryRepository>();
            mock.Setup(s => s.Query("SELECT * FROM Test", "Key")).Returns(doc);

           var pipeline = PipelineFactory.CreateEmptyReceivePipeline();
            string m = "<body></body>";

            var message = MessageHelper.CreateFromString(m);

            var component = new PromoteDbQueryResult(mock.Object)
            {
                ConnectionStringConfigKey = "Key",
                ContextPropertyToPromote = "ns#property",
                Query = "SELECT * FROM Test"

            };

            pipeline.AddComponent(component, PipelineStage.Decode);

            var result = pipeline.Execute(message);


            Assert.AreEqual("param1value", result[0].Context.Read("property","ns").ToString()) ;
        }
    }
}
