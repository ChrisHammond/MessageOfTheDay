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
using System.Linq;
using System.Web.Mvc;
using Christoc.Modules.MessageOfTheDay.Components;
using Christoc.Modules.MessageOfTheDay.Models;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Web.Mvc.Framework.Controllers;
using DotNetNuke.Web.Mvc.Framework.ActionFilters;
using DotNetNuke.Entities.Users;
using DotNetNuke.Framework.JavaScriptLibraries;
using DotNetNuke.Security;
using DotNetNuke.UI.Modules;

namespace Christoc.Modules.MessageOfTheDay.Controllers
{
    [DnnHandleError]
    public class MessageController : DnnController
    {

        public ActionResult Delete(int MessageId)
        {
            MessageManager.Instance.DeleteMessage(MessageId, ModuleContext.ModuleId);
            return RedirectToDefaultRoute();
        }

        public ActionResult Edit(int MessageId = -1)
        {
            DotNetNuke.Framework.JavaScriptLibraries.JavaScript.RequestRegistration(CommonJs.DnnPlugins);

            
            var item = (MessageId == -1)
                 ? new Message { ModuleId = ModuleContext.ModuleId }
                 : MessageManager.Instance.GetMessage(MessageId, ModuleContext.ModuleId);

            return View(item);
        }

        [HttpPost]
        [DotNetNuke.Web.Mvc.Framework.ActionFilters.ValidateAntiForgeryToken]
        public ActionResult Edit(Message m)
        {
            if (m.MessageId == -1)
            {
                m.CreatedByUserId = User.UserID;
                m.CreatedOnDate = DateTime.UtcNow;
                m.LastModifiedByUserId = User.UserID;
                m.LastModifiedOnDate = DateTime.UtcNow;
                
                MessageManager.Instance.CreateMessage(m);
            }
            else
            {
                var existingItem = MessageManager.Instance.GetMessage(m.MessageId, m.ModuleId);
                existingItem.LastModifiedByUserId = User.UserID;
                existingItem.LastModifiedOnDate = DateTime.UtcNow;
                existingItem.MessageTitle = m.MessageTitle;
                existingItem.MessageDescription = m.MessageDescription;
                existingItem.MessageUrl = m.MessageUrl;
                existingItem.MessageImageUrl = m.MessageImageUrl;
                existingItem.MessageVideoUrl = m.MessageVideoUrl;
                existingItem.MessageDisplayDate = m.MessageDisplayDate;
                MessageManager.Instance.UpdateMessage(existingItem);
            }

            return RedirectToDefaultRoute();
        }

        //[ModuleAction(ControlKey = "Edit", TitleKey = "AddMessage")]
        [HttpGet]
        [ModuleActionItems]
        public ActionResult Index() 
        {
            //var messages = MessageManager.Instance.GetMessages(ModuleContext.ModuleId);

            var message = MessageManager.Instance.GetDailyMessage(ModuleContext.ModuleId);
            return View(message);
        }

        [HttpGet]
        public ActionResult AdminList()
        {
            var messages = MessageManager.Instance.GetMessages(ModuleContext.ModuleId);
            return View(messages);
        }

        [HttpGet]
        public ActionResult ResetDailyMessage()
        {
            var log = LogManager.Instance.GetDailyLog(DateTime.Now, ModuleContext.ModuleId);
            LogManager.Instance.DeleteLog(log);
            //redirect
            Response.Redirect(DotNetNuke.Common.Globals.NavigateURL());
            return null;
        }

        public ModuleActionCollection GetIndexActions()
        {
            var actions = new ModuleActionCollection
            {
                new ModuleAction(-1)
                {
                    CommandName = ModuleActionType.AddContent,
                    CommandArgument = String.Empty,
                    Icon = String.Empty,
                    Title = LocalizeString("AddMessage"),
                    Url = ModuleContext.EditUrl("Edit"),
                    Secure = SecurityAccessLevel.Edit,
                    UseActionEvent = false,
                    Visible = true,
                    NewWindow = false
                },
                new ModuleAction(-1)
                {
                    CommandName = ModuleActionType.ContentOptions,
                    CommandArgument = String.Empty,
                    Icon = String.Empty,
                    Title = LocalizeString("AdminList"),
                    Url = ModuleContext.EditUrl("AdminList"),
                    Secure = SecurityAccessLevel.Edit,
                    UseActionEvent = false,
                    Visible = true,
                    NewWindow = false
                },
                new ModuleAction(-1)
                {
                    CommandName = ModuleActionType.ContentOptions,
                    CommandArgument = String.Empty,
                    Icon = String.Empty,
                    Title = LocalizeString("ResetDailyMessage"),
                    Url = ModuleContext.EditUrl("ResetDailyMessage"),
                    //Url = DotNetNuke.Common.Globals.NavigateURL(ModuleContext.TabId,string.Empty,"removeLog=true"),//ModuleContext.EditUrl("ResetDailyMessage"),
                    Secure = SecurityAccessLevel.Edit,
                    UseActionEvent = false,
                    Visible = true,
                    NewWindow = false
                }
            };

            return actions;
        }


    }
}

