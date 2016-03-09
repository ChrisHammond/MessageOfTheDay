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
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Caching;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace Christoc.Modules.MessageOfTheDay.Models
{
    [TableName("MOTD_Logs")]
    //setup the primary key for table
    [PrimaryKey("LogId", AutoIncrement = true)]
    //configure caching using PetaPoco
    [Cacheable("Logs", CacheItemPriority.Default, 20)]
    //scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    [Scope("ModuleId")]
    public class Log
    {
        ///<summary>
        /// The ID of the log
        ///</summary>
        public int LogId { get; set; } = -1;


        ///<summary>
        /// The that we are dealing with LogDate
        ///</summary>
        public DateTime LogDate { get; set; }

        ///<summary>
        /// The ModuleId of where the Log was created and gets displayed
        ///</summary>
        public int ModuleId { get; set; }

        ///<summary>
        /// The MessageId of where the Message was created and gets displayed
        ///</summary>
        public int MessageId { get; set; }

        /////<summary>
        ///// An integer for the user id of the user who created the message
        /////</summary>
        //public int CreatedByUserId { get; set; } = -1;

        /////<summary>
        ///// An integer for the user id of the user who last updated the message
        /////</summary>
        //public int LastModifiedByUserId { get; set; } = -1;

        ///<summary>
        /// The date the Log was created
        ///</summary>
        public DateTime CreatedOnDate { get; set; } = DateTime.UtcNow;

        ///<summary>
        /// The date the Log was updated
        ///</summary>
        public DateTime LastModifiedOnDate { get; set; } = DateTime.UtcNow;

        ///<summary>
        /// The date the LogWasDeleted
        ///</summary>
        public DateTime? DeletedDate { get; set; }



    }
}
