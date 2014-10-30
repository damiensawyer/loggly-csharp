using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Loggly.Tests.Loggly.ExtensionMethods
{
    using global::Loggly.Config;

    public class DateTimeExtensionsFixture : Fixture
    {
        /// <summary>
        /// https://www.loggly.com/docs/automated-parsing/#json
        /// </summary>
        [Test]
        public void ToJsonIso8601()
        {
            var date = new DateTime(2013, 10, 11, 22, 14, 15, 3);
            Assert.AreEqual("2013-10-11T22:14:15.003000Z", date.ToJsonIso8601());
        }

        /// <summary>
        /// https://www.loggly.com/docs/tags/
        /// </summary>
        [Test]
        public void ToSyslog()
        {
            var date = new DateTime(2013, 10, 11, 22, 14, 15, 3);
            Assert.AreEqual("2013-10-11T22:14:15.003000+11:00", date.ToSyslog());
            Console.WriteLine(DateTime.UtcNow.ToSyslog());
        }

        [Test]
        public void DateTimeOffsetToSyslog()
        {
            var date = new DateTimeOffset(2013, 10, 11, 22, 14, 15, 3, TimeSpan.FromHours(10));
            Assert.AreEqual("2013-10-11T22:14:15.003000+10:00", date.ToSyslog());
            Console.WriteLine(DateTime.UtcNow.ToSyslog());
        }


        [Test]
        public void DamienTest()
        {
            // Check out in Fiddler... this isn't actually working. Appears to be sending on HTTP (not https)
            LogglyConfig.Instance.Transport.LogTransport = LogTransport.Https;
            LogglyConfig.Instance.Transport.EndpointHostname = @"logs-01.loggly.com";
            LogglyConfig.Instance.ThrowExceptions = true;
            LogglyConfig.Instance.ApplicationName = "dnstest";
            LogglyConfig.Instance.Transport.EndpointPort = 443;
            
            LogglyConfig.Instance.CustomerToken = "89ba7ea0-89df-4a37-9a54-35c97772954d";
            
            var _loggly = new LogglyClient();
            for (int i = 0; i < 10; i++)
            {
                var logEvent = new LogglyEvent();
                logEvent.Data.Add("message", "Test Simple message {0} at {1}", i, DateTime.Now);
                _loggly.Log(logEvent);    
            }
        }
    }
}
