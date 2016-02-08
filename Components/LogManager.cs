/*
' Copyright (c) 2016 Christoc.com
'  All rights reserved.
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DotNetNuke.Data;
using DotNetNuke.Framework;
using Christoc.Modules.MessageOfTheDay.Models;

namespace Christoc.Modules.MessageOfTheDay.Components
{
    interface ILogManager
    {
        void CreateLog(Log t);
        void DeleteLog(int LogId, int moduleId);
        void DeleteLog(Log t);
        IEnumerable<Log> GetLogs(int moduleId);
        Log GetLog(int LogId, int moduleId);

        Log GetDailyLog(DateTime d, int moduleId);

        void UpdateLog(Log t);
    }

    class LogManager : ServiceLocator<ILogManager, LogManager>, ILogManager
    {
        public void CreateLog(Log t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Log>();
                rep.Insert(t);
            }
        }

        public void DeleteLog(int LogId, int moduleId)
        {
            var t = GetLog(LogId, moduleId);
            DeleteLog(t);
        }

        public void DeleteLog(Log t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Log>();
                rep.Delete(t);
            }
        }

        public IEnumerable<Log> GetLogs(int moduleId)
        {
            IEnumerable<Log> t;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Log>();
                t = rep.Get(moduleId);
            }
            return t;
        }

        public Log GetLog(int LogId, int moduleId)
        {
            Log t;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Log>();
                t = rep.GetById(LogId, moduleId);
            }
            return t;
        }

        public Log GetDailyLog(DateTime theDate, int moduleId)
        {

            Log t;
            using (IDataContext ctx = DataContext.Instance())
            {
                t = ctx.ExecuteSingleOrDefault<Log>(CommandType.Text, "SELECT top 1 * FROM MOTD_Logs Where moduleid=" + moduleId + " and LogDate = '" + theDate.ToShortDateString() + "'");
            }
            return t;
        }

        public void UpdateLog(Log t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Log>();
                rep.Update(t);
            }
        }


        protected override System.Func<ILogManager> GetFactory()
        {
            return () => new LogManager();
        }

    }
}
