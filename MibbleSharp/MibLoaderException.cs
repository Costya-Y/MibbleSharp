﻿//
// MibLoaderException.cs
// 
// This work is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published
// by the Free Software Foundation; either version 2 of the License,
// or (at your option) any later version.
//
// This work is distributed in the hope that it will be useful, but
// WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307
// USA
// 
// Original Java code Copyright (c) 2004-2016 Per Cederberg. All
// rights reserved.
// C# conversion Copyright (c) 2016 Jeremy Gibbons. All rights reserved
//



using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace MibbleSharp
{
    /// 
    /// <summary>
    /// A MIB loader exception. This exception is thrown when a MIB file
    /// couldn't be loaded properly, normally due to syntactical or
    /// semantical errors in the file.
    /// </summary>
    /// 
    [Serializable]
    public class MibLoaderException : Exception
    {
        private MibLoaderLog log;

        /// 
        /// <summary>
        /// Creates a new MibLoaderException
        /// </summary>
        /// <param name="log">The MIB loader log</param>
        /// 
        public MibLoaderException(MibLoaderLog log)
        {
            this.log = log;
        }

        /// 
        /// <summary>
        /// Creates a new MIB loader exception. The specified message will
        /// be added to a new MIB loader log as an error.
        /// </summary>
        /// <param name="file">The MIB file for which an exception was raised during loading</param>
        /// <param name="message">The detailed error message</param>
        /// 
        public MibLoaderException(string file, string message)
        {
            log = new MibLoaderLog();
            log.AddError(file, -1, -1, message);
        }

        /// 
        /// <summary>
        /// Deserialize a MibLoaderException
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        /// 
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        protected MibLoaderException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.log = (MibLoaderLog) info.GetValue("Log", typeof(MibLoaderLog));
        }

        /// 
        /// <summary>
        /// The MIBLoader log
        /// </summary>
        /// 
        public MibLoaderLog Log
        {
            get
            {
                return log;
            }
        }

        /// 
        /// <summary>
        /// The detailed error message
        /// </summary>
        /// 
        public override string Message
        {
            get
            {
                return "found " + log.ErrorCount + " MIB loader errors";
            }

        }

        /// <summary>
        /// Serialize a MibLoader log
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Log", log);
            base.GetObjectData(info, context);
        }
    }

}
