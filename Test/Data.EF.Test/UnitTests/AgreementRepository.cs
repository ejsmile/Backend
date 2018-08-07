using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SQLite.CodeFirst.Test.UnitTests.Utility
{
    [TestClass]
    public class ConnectionStringParserTest
    {
        [TestMethod]
        public void Test()
        {
            // Arrange
            string connectionString = @"data source=.\db\footballDb\footballDb.sqlite;foreign keys=true";

            // Act
            string result = connectionString.Substring(12, 33);

            // Assert
            Assert.AreEqual(@".\db\footballDb\footballDb.sqlite", result);
        }
    }
}