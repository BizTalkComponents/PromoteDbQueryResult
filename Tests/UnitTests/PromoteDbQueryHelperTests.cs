using BizTalkComponents.Utilities.DbQueryUtility.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BizTalkComponents.PipelineComponents.PromoteDbQueryResult.PromoteDbQueryResult.Tests.UnitTests
{
    [TestClass]
    public class PromoteDbQueryHelperTests
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

            var helper = new DbQueryHelper(mock.Object);

            Assert.AreEqual("param1value",helper.Query("SELECT * FROM Test", "Key"));
        }
    }
}
