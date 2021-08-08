using MUNity.Converter;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MunitySchemaTest.ConverterTests
{
    public class TimeSpanConverterTests
    {
        [Test]
        public void TestCanCreateInstance()
        {
            var instance = new TimespanConverter();
            Assert.NotNull(instance);
        }

        //[Test]
        //public void TestRead()
        //{
        //    var instance = new TimespanConverter();
        //    string json = "{\n\tTimeSpanValue: \"00:03:00\"\n}";
        //    var utf8Reader = new Utf8JsonReader(json.Select(n => (byte)n).ToArray());

        //    while (utf8Reader.TokenType == JsonTokenType.None)
        //        if (!utf8Reader.Read())
        //            break;

        //    var obj = instance.Read(ref utf8Reader, typeof(DemoClass), new JsonSerializerOptions());

        //}

        //[Test]
        //public void TestWrite()
        //{
        //    var converter = new TimespanConverter();
        //    TimeSpan span = new TimeSpan(0, 3, 0);
        //    string result;
        //    System.IO.MemoryStream stream = new MemoryStream();
            
        //    var writer = new Utf8JsonWriter(stream);
        //    converter.Write(writer, span, new JsonSerializerOptions());
        //    stream.Position = 0;
        //    var text = new System.IO.StreamReader(stream).ReadToEnd();
        //    Assert.AreNotEqual("", text);
        //}
    }

    public class DemoClass
    {
        public TimeSpan TimeSpanValue { get; set; }
    }
}
