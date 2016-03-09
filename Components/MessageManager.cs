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
using DotNetNuke.Entities.Users;
using DotNetNuke.UI.UserControls;

namespace Christoc.Modules.MessageOfTheDay.Components
{
    interface IMessageManager
    {
        void CreateMessage(Message t);
        void DeleteMessage(int MessageId, int moduleId);
        void DeleteMessage(Message t);
        IEnumerable<Message> GetMessages(int moduleId);
        Message GetMessage(int MessageId, int moduleId);
        Message GetMessageByDate(DateTime d, int moduleId);
        Message GetDailyMessage(int moduleId);
        void UpdateMessage(Message t);
    }

    class MessageManager : ServiceLocator<IMessageManager, MessageManager>, IMessageManager
    {
        public void CreateMessage(Message t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Message>();
                rep.Insert(t);
            }
        }

        public void DeleteMessage(int MessageId, int moduleId)
        {
            var t = GetMessage(MessageId, moduleId);
            DeleteMessage(t);
        }

        public void DeleteMessage(Message t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Message>();
                rep.Delete(t);
            }
        }

        public IEnumerable<Message> GetMessages(int moduleId)
        {
            IEnumerable<Message> t;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Message>();
                t = rep.Get(moduleId);
            }
            return t;
        }

        public Message GetMessage(int MessageId, int moduleId)
        {
            Message t;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Message>();
                t = rep.GetById(MessageId, moduleId);
            }
            return t;
        }


        public Message GetDailyMessage(int moduleId)
        {

            //TODO: get message schedule for Today, if not possible, get a random message

            //Message t;
            //using (IDataContext ctx = DataContext.Instance())
            //{
            //    var rep = ctx.GetRepository<Message>();
            //    t = rep.Get(moduleId);
            //}
            //return t;

            //Get today's Log, if it is Null, then we don't have a day defined already

            var l = LogManager.Instance.GetDailyLog(DateTime.Now, moduleId);
            if (l != null)
            {
                return GetMessage(l.MessageId, moduleId);
            }
            else
            {//If Log is Null, get the first Message that has Today's date.

                var m = GetMessageByDate(DateTime.Now, moduleId);

                if (m!=null)
                {
                    //Save the log

                    var newLog = new Log();
                    newLog.MessageId = m.MessageId;
                    newLog.CreatedOnDate = DateTime.UtcNow;
                    newLog.LastModifiedOnDate = DateTime.UtcNow;
                    newLog.LogDate = DateTime.Now.Date; //TODO: need to make this use the Portal time zone instead of Server time
                    newLog.ModuleId = m.ModuleId;
                    LogManager.Instance.CreateLog(newLog);
                    return m;
                }
            }
            
            //If nothing yet, get random
            var ranMes = GetRandomMessage(moduleId);
            if (ranMes != null)
            {
                //Save the log

                var newLog = new Log();
                newLog.MessageId = ranMes.MessageId;
                newLog.CreatedOnDate = DateTime.UtcNow;
                newLog.LastModifiedOnDate = DateTime.UtcNow;
                newLog.LogDate = DateTime.Now.Date; //TODO: need to make this use the Portal time zone instead of Server time
                newLog.ModuleId = ranMes.ModuleId;
                LogManager.Instance.CreateLog(newLog);
                return ranMes;
            }
            return new Message();
        }

        public Message GetMessageByDate(DateTime d, int moduleId)
        {
            Message t;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Message>();
                t = ctx.ExecuteSingleOrDefault<Message>(CommandType.Text, "SELECT top 1 * FROM MOTD_Messages WHERE moduleid=" + moduleId + " and MessageDisplayDate = '" + d.ToShortDateString() + "'" );

            }
            return t;
        }

        public Message GetRandomMessage(int moduleId)
        {
            Message t;
            using (IDataContext ctx = DataContext.Instance())
            {
                t = ctx.ExecuteSingleOrDefault<Message>(CommandType.Text, "SELECT top 1 * FROM MOTD_Messages where moduleid=" + moduleId +" order by NEWID()  ");
                
            }
            return t;
        }

        //TODO: We need to store which message is being displayed on a particular day, once selected, that message should continue to display until the next day
        
        public void UpdateMessage(Message t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Message>();
                rep.Update(t);
            }
        }


        protected override System.Func<IMessageManager> GetFactory()
        {
            return () => new MessageManager();
        }
        
    }
}
