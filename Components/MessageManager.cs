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

using System.Collections.Generic;
using DotNetNuke.Data;
using DotNetNuke.Framework;
using Christoc.Modules.MessageOfTheDay.Models;

namespace Christoc.Modules.MessageOfTheDay.Components
{
    interface IMessageManager
    {
        void CreateMessage(Message t);
        void DeleteMessage(int MessageId, int moduleId);
        void DeleteMessage(Message t);
        IEnumerable<Message> GetMessages(int moduleId);
        Message GetMessage(int MessageId, int moduleId);
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
