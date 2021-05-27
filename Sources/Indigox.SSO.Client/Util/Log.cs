using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml;

namespace Indigox.SSO.Client.Util
{
    public class Log
    {
        private static bool EnableLogging
        {
            get
            {
                string value = ConfigurationManager.AppSettings[ "ENABLE_LOGGING" ];
                return ( string.IsNullOrEmpty( value ) ) ? false : Convert.ToBoolean( value );
            }
        }

        public static void Debug( string message )
        {
            if ( !EnableLogging )
            {
                return;
            }

            try
            {
                Log4netLogger log = new Log4netLogger( GetCallerType() );
                log.Debug( message );
                Trace.WriteLine(message);
            }
            catch ( Exception )
            {
                // ignore
            }
        }

        public static void Info( string message )
        {
            if ( !EnableLogging )
            {
                return;
            }

            try
            {
                Log4netLogger log = new Log4netLogger( GetCallerType() );
                log.Info( message );
            }
            catch ( Exception )
            {
                // ignore
            }
        }

        public static void Warn( string message )
        {
            if ( !EnableLogging )
            {
                return;
            }

            try
            {
                Log4netLogger log = new Log4netLogger( GetCallerType() );
                log.Warn( message );
            }
            catch ( Exception )
            {
                // ignore
            }
        }

        public static void Error( string message )
        {
            if ( !EnableLogging )
            {
                return;
            }

            try
            {
                Log4netLogger log = new Log4netLogger( GetCallerType() );
                log.Error( message );
            }
            catch ( Exception )
            {
                // ignore
            }
        }

        private static Type GetCallerType()
        {
            return new StackTrace( 2, false ).GetFrame( 0 ).GetMethod().DeclaringType;
        }

        private class Log4netLogger
        {
            private const string LoggerRepository = "Indigox.SSO.Client";

            private static Type LogManager;
            private static Type XmlConfigurator;
            private static Type ILoggerRepository;
            private static Type ILog;

            private static MethodInfo LogManager_GetLogger;
            private static MethodInfo LogManager_CreateRepository;
            private static MethodInfo XmlConfigurator_Configure;
            private static MethodInfo ILog_Debug;
            private static MethodInfo ILog_Info;
            private static MethodInfo ILog_Warn;
            private static MethodInfo ILog_Error;

            private static bool InitFailed = false;

            static Log4netLogger()
            {
                InitReflectedMembers();
                InitLog4net();
            }

            private static void InitReflectedMembers()
            {
                LogManager = Type.GetType( "log4net.LogManager, log4net" );
                XmlConfigurator = Type.GetType( "log4net.Config.XmlConfigurator, log4net" );
                ILoggerRepository = Type.GetType( "log4net.Repository.ILoggerRepository, log4net" );
                ILog = Type.GetType( "log4net.ILog, log4net" );

                if ( LogManager == null )
                {
                    InitFailed = true;
                    return;
                }

                LogManager_GetLogger = LogManager.GetMethod( "GetLogger", new Type[] { typeof( string ), typeof( Type ) } );
                LogManager_CreateRepository = LogManager.GetMethod( "CreateRepository", new Type[] { typeof( string ) } );

                XmlConfigurator_Configure = XmlConfigurator.GetMethod( "Configure", new Type[] { ILoggerRepository, typeof( XmlElement ) } );

                ILog_Debug = ILog.GetMethod( "Debug", new Type[] { typeof( string ) } );
                ILog_Info = ILog.GetMethod( "Info", new Type[] { typeof( string ) } );
                ILog_Warn = ILog.GetMethod( "Warn", new Type[] { typeof( string ) } );
                ILog_Error = ILog.GetMethod( "Error", new Type[] { typeof( string ) } );
            }

            private static void InitLog4net()
            {
                try
                {
                    object repository = LogManager_CreateRepository.Invoke( null, new object[] { LoggerRepository } );
                    XmlElement element = GetConfigElement();
                    XmlConfigurator_Configure.Invoke( null, new object[] { repository, element } );
                }
                catch ( Exception )
                {
                    InitFailed = true;
                }
            }

            private static XmlElement GetConfigElement()
            {
                Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream( "Indigox.SSO.Client.Util.log4net.config.xml" );
                StreamReader reader = new StreamReader( stream );
                string xml = reader.ReadToEnd();
                XmlDocument xdoc = new XmlDocument();
                xdoc.LoadXml( xml );
                XmlElement element = xdoc.DocumentElement;
                return element;
            }

            private object log;

            public Log4netLogger( Type type )
            {
                log = GetLogger( type );
            }

            private object GetLogger( Type type )
            {
                if ( !InitFailed )
                {
                    return LogManager_GetLogger.Invoke( null, new object[] { LoggerRepository, type } );
                }
                return null;
            }

            public void Debug( string message )
            {
                if ( log != null )
                {
                    ILog_Debug.Invoke( log, new object[] { message } );
                }
            }

            public void Info( string message )
            {
                if ( log != null )
                {
                    ILog_Info.Invoke( log, new object[] { message } );
                }
            }

            public void Warn( string message )
            {
                if ( log != null )
                {
                    ILog_Warn.Invoke( log, new object[] { message } );
                }
            }

            public void Error( string message )
            {
                if ( log != null )
                {
                    ILog_Error.Invoke( log, new object[] { message } );
                }
            }
        }
    }
}