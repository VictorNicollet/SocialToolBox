using System;
using NUnit.Framework;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Database.Index;

namespace SocialToolBox.Core.Tests.Database.Index
{
    [TestFixture]
    public class index_field_type
    {
        [Test]
        public void integer()
        {
            var ft = new IndexFieldType(typeof (int), new IndexFieldAttribute(0));
            Assert.AreEqual("INT NOT NULL", ft.MySqlDeclaration);
            Assert.AreEqual("INT NOT NULL", ft.SqlServerDeclaration);
        }

        [Test]
        public void integer_null()
        {
            var ft = new IndexFieldType(typeof(int?), new IndexFieldAttribute(0));
            Assert.AreEqual("INT NULL", ft.MySqlDeclaration);
            Assert.AreEqual("INT NULL", ft.SqlServerDeclaration);
        }

        [Test]
        public void string_short()
        {
            var ft = new IndexFieldType(typeof(string), new IndexFieldAttribute(0){Length=30});
            Assert.AreEqual("VARCHAR(30) NULL CHARACTER SET utf8 COLLATE utf8_bin", ft.MySqlDeclaration);
            Assert.AreEqual("NVARCHAR(30) NULL COLLATE latin1_general_cs_as", ft.SqlServerDeclaration);
        }

        [Test]
        public void string_default()
        {
            var ft = new IndexFieldType(typeof(string), new IndexFieldAttribute(0));
            Assert.AreEqual("VARCHAR(255) NULL CHARACTER SET utf8 COLLATE utf8_bin", ft.MySqlDeclaration);
            Assert.AreEqual("NVARCHAR(255) NULL COLLATE latin1_general_cs_as", ft.SqlServerDeclaration);
        }

        [Test]
        public void string_not_null()
        {
            var ft = new IndexFieldType(typeof(string), new IndexFieldAttribute(0){NotNull = true});
            Assert.AreEqual("VARCHAR(255) NOT NULL CHARACTER SET utf8 COLLATE utf8_bin", ft.MySqlDeclaration);
            Assert.AreEqual("NVARCHAR(255) NOT NULL COLLATE latin1_general_cs_as", ft.SqlServerDeclaration);
        }

        [Test]
        public void string_case_insensitive()
        {
            var ft = new IndexFieldType(typeof(string), new IndexFieldAttribute(0) {IsCaseSensitive = false});
            Assert.AreEqual("VARCHAR(255) NULL CHARACTER SET utf8 COLLATE utf8_general_ci", ft.MySqlDeclaration);
            Assert.AreEqual("NVARCHAR(255) NULL COLLATE latin1_general_as", ft.SqlServerDeclaration);
        }

        [Test]
        public void id()
        {
            var ft = new IndexFieldType(typeof(Id), new IndexFieldAttribute(0));
            Assert.AreEqual("CHAR(11) NULL CHARACTER SET ascii COLLATE ascii_bin", ft.MySqlDeclaration);
            Assert.AreEqual("CHAR(11) NULL CHARACTER SET latin1 COLLATE latin1_general_cs_as", ft.SqlServerDeclaration);
        }

        [Test]
        public void id_not_null()
        {
            var ft = new IndexFieldType(typeof(Id), new IndexFieldAttribute(0) {NotNull = true});
            Assert.AreEqual("CHAR(11) NOT NULL CHARACTER SET ascii COLLATE ascii_bin", ft.MySqlDeclaration);
            Assert.AreEqual("CHAR(11) NOT NULL CHARACTER SET latin1 COLLATE latin1_general_cs_as", ft.SqlServerDeclaration);
        }

        [Test]
        public void datetime()
        {
            var ft = new IndexFieldType(typeof(DateTime), new IndexFieldAttribute(0));
            Assert.AreEqual("DATETIME NOT NULL", ft.MySqlDeclaration);
            Assert.AreEqual("DATETIME NOT NULL", ft.SqlServerDeclaration);
        }

        [Test]
        public void datetime_null()
        {
            var ft = new IndexFieldType(typeof(DateTime?), new IndexFieldAttribute(0));
            Assert.AreEqual("DATETIME NULL", ft.MySqlDeclaration);
            Assert.AreEqual("DATETIME NULL", ft.SqlServerDeclaration);
        }
    }
}
