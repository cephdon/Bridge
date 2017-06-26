using System;
using System.Linq;
using System.Collections.Generic;

using Bridge.Contract;
using Bridge.Translator.Tests.Helpers;


using NUnit.Framework;
using NSubstitute;

namespace Bridge.Translator.Tests
{
    [TestFixture]
    class FileHelperTests
    {
        [Test]
        public void TranslatorFileHelper_IsMinWorks()
        {
            var helper = new FileHelper();

            Assert.True(helper.IsMin(@"a.min.js"));
            Assert.True(helper.IsMin(@"a.MIN.js"));
            Assert.True(helper.IsMin(@"a.MiN.Js"));
            Assert.True(helper.IsMin(@"_.min.js"));
            Assert.True(helper.IsMin(@"_.MIN.js"));
            Assert.True(helper.IsMin(@"_.MiN.Js"));
            Assert.True(helper.IsMin(@"C:\A\a.min.js"));
            Assert.True(helper.IsMin(@"C:\A\a.MIN.js"));
            Assert.True(helper.IsMin(@"C:\A\a.MiN.Js"));
            Assert.True(helper.IsMin(@"C:\A\a.min.css"));
            Assert.True(helper.IsMin(@"C:\A\a.MIN.css"));
            Assert.True(helper.IsMin(@"C:\A\a.MiN.cSs"));
            Assert.True(helper.IsMin(@"C:\A B\a.min.png"));
            Assert.True(helper.IsMin(@"C:\A B\a.MIN.PNg"));
            Assert.True(helper.IsMin(@"C:\A B\a.MiN.pnG"));
            Assert.True(helper.IsMin(@"Base\..\..\a.min.ttF"));
            Assert.True(helper.IsMin(@"Base\..\..\a.MIN.ttf"));
            Assert.True(helper.IsMin(@"Base\..\..\a.MiN.tTf"));
            Assert.True(helper.IsMin(@"Base\..\..\a.min.ttF"));
            Assert.True(helper.IsMin(@"Base\..\..\a.MIN.ttf"));
            Assert.True(helper.IsMin(@"Base\..\..\a.MiN.tTf"));

            Assert.False(helper.IsMin(null));
            Assert.False(helper.IsMin(""));
            Assert.False(helper.IsMin("@#"));

            Assert.False(helper.IsMin(@"min.js"));
            Assert.False(helper.IsMin(@"MIN.js"));
            Assert.False(helper.IsMin(@"MiN.Js"));
            Assert.False(helper.IsMin(@"C:\A\min.js"));
            Assert.False(helper.IsMin(@"C:\A\MIN.js"));
            Assert.False(helper.IsMin(@"C:\A\MiN.Js"));
            Assert.False(helper.IsMin(@"a.min"));
            Assert.False(helper.IsMin(@"a.MIN"));
            Assert.False(helper.IsMin(@"a.MiN"));
            Assert.False(helper.IsMin(@"a.min"));
            Assert.False(helper.IsMin(@"a.MIN"));
            Assert.False(helper.IsMin(@"C:\A\a.MiN"));
            Assert.False(helper.IsMin(@"C:\A\min"));
            Assert.False(helper.IsMin(@"C:\A\MIN"));
            Assert.False(helper.IsMin(@"MiN"));
            Assert.False(helper.IsMin(@"min"));
            Assert.False(helper.IsMin(@"MIN"));
            Assert.False(helper.IsMin(@"MiN"));
            Assert.False(helper.IsMin(@"C:\A B\a.mi.png"));
            Assert.False(helper.IsMin(@"C:\A B\a.MI.PNg"));
            Assert.False(helper.IsMin(@"C:\A B\a.Mi.pnG"));
            Assert.False(helper.IsMin(@"Base\..\..\a.min"));
            Assert.False(helper.IsMin(@"Base\..\..\a.MIN"));
            Assert.False(helper.IsMin(@"Base\..\..\a.MiN"));
            Assert.False(helper.IsMin(@"Base\..\..\a.min"));
            Assert.False(helper.IsMin(@"Base\..\..\a.MIN"));
            Assert.False(helper.IsMin(@"Base\..\..\a.MiN"));
        }

        [Test]
        public void TranslatorFileHelper_GetSymmetricFileName()
        {
            var helper = new FileHelper();

            Assert.AreEqual(@"a.js", helper.GetSymmetricFileName(@"a.min.js"));
            Assert.AreEqual(@"a.js", helper.GetSymmetricFileName(@"a.MIN.js"));
            Assert.AreEqual(@"a.Js", helper.GetSymmetricFileName(@"a.MiN.Js"));
            Assert.AreEqual(@"_.js", helper.GetSymmetricFileName(@"_.min.js"));
            Assert.AreEqual(@"_.js", helper.GetSymmetricFileName(@"_.MIN.js"));
            Assert.AreEqual(@"_.Js", helper.GetSymmetricFileName(@"_.MiN.Js"));
            Assert.AreEqual(@"C:\A\a.js", helper.GetSymmetricFileName(@"C:\A\a.min.js"));
            Assert.AreEqual(@"C:\A\a.js", helper.GetSymmetricFileName(@"C:\A\a.MIN.js"));
            Assert.AreEqual(@"C:\A\a.Js", helper.GetSymmetricFileName(@"C:\A\a.MiN.Js"));
            Assert.AreEqual(@"C:\A\a.css", helper.GetSymmetricFileName(@"C:\A\a.min.css"));
            Assert.AreEqual(@"C:\A\a.css", helper.GetSymmetricFileName(@"C:\A\a.MIN.css"));
            Assert.AreEqual(@"C:\A\a.cSs", helper.GetSymmetricFileName(@"C:\A\a.MiN.cSs"));
            Assert.AreEqual(@"C:\A B\a.png", helper.GetSymmetricFileName(@"C:\A B\a.min.png"));
            Assert.AreEqual(@"C:\A B\a.PNg", helper.GetSymmetricFileName(@"C:\A B\a.MIN.PNg"));
            Assert.AreEqual(@"C:\A B\a.pnG", helper.GetSymmetricFileName(@"C:\A B\a.MiN.pnG"));
            Assert.AreEqual(@"Base\..\..\a.ttF", helper.GetSymmetricFileName(@"Base\..\..\a.min.ttF"));
            Assert.AreEqual(@"Base\..\..\a.ttf", helper.GetSymmetricFileName(@"Base\..\..\a.MIN.ttf"));
            Assert.AreEqual(@"Base\..\..\a.tTf", helper.GetSymmetricFileName(@"Base\..\..\a.MiN.tTf"));
            Assert.AreEqual(@"Base\..\..\a.ttF", helper.GetSymmetricFileName(@"Base\..\..\a.min.ttF"));
            Assert.AreEqual(@"Base\..\..\a.ttf", helper.GetSymmetricFileName(@"Base\..\..\a.MIN.ttf"));
            Assert.AreEqual(@"Base\..\..\a.tTf", helper.GetSymmetricFileName(@"Base\..\..\a.MiN.tTf"));

            Assert.AreEqual(null, helper.GetSymmetricFileName(null));
            Assert.AreEqual("", helper.GetSymmetricFileName(""));
            Assert.AreEqual("@#", helper.GetSymmetricFileName("@#"));

            Assert.AreEqual(@"a.min.min", helper.GetSymmetricFileName(@"a.min"));
            Assert.AreEqual(@"a.min.MIN", helper.GetSymmetricFileName(@"a.MIN"));
            Assert.AreEqual(@"a.min.MiN", helper.GetSymmetricFileName(@"a.MiN"));
            Assert.AreEqual(@"a.min.min", helper.GetSymmetricFileName(@"a.min"));
            Assert.AreEqual(@"a.min.MIN", helper.GetSymmetricFileName(@"a.MIN"));
            Assert.AreEqual(@"C:\A\a.min.MiN", helper.GetSymmetricFileName(@"C:\A\a.MiN"));
            Assert.AreEqual(@"C:\A\min", helper.GetSymmetricFileName(@"C:\A\min"));
            Assert.AreEqual(@"C:\A\MIN", helper.GetSymmetricFileName(@"C:\A\MIN"));
            Assert.AreEqual(@"MiN", helper.GetSymmetricFileName(@"MiN"));
            Assert.AreEqual(@"min", helper.GetSymmetricFileName(@"min"));
            Assert.AreEqual(@"MIN", helper.GetSymmetricFileName(@"MIN"));
            Assert.AreEqual(@"MiN", helper.GetSymmetricFileName(@"MiN"));
            Assert.AreEqual(@"C:\A B\a.mi.min.png", helper.GetSymmetricFileName(@"C:\A B\a.mi.png"));
            Assert.AreEqual(@"C:\A B\a.MI.min.PNg", helper.GetSymmetricFileName(@"C:\A B\a.MI.PNg"));
            Assert.AreEqual(@"C:\A B\a.Mi.min.pnG", helper.GetSymmetricFileName(@"C:\A B\a.Mi.pnG"));
            Assert.AreEqual(@"Base\..\..\a.min.min", helper.GetSymmetricFileName(@"Base\..\..\a.min"));
            Assert.AreEqual(@"Base\..\..\a.min.MIN", helper.GetSymmetricFileName(@"Base\..\..\a.MIN"));
            Assert.AreEqual(@"Base\..\..\a.min.MiN", helper.GetSymmetricFileName(@"Base\..\..\a.MiN"));
            Assert.AreEqual(@"Base\..\..\a.min.min", helper.GetSymmetricFileName(@"Base\..\..\a.min"));
            Assert.AreEqual(@"Base\..\..\a.min.MIN", helper.GetSymmetricFileName(@"Base\..\..\a.MIN"));
            Assert.AreEqual(@"Base\..\..\a.min.MiN", helper.GetSymmetricFileName(@"Base\..\..\a.MiN"));
        }
    }
}
